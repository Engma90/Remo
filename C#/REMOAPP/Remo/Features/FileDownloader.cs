using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Remo.Connections;
using System.IO;

namespace Remo.Features
{
    public partial class FileDownloader : Form, IFeature
    {
        public FileDownloader()
        {
            InitializeComponent();
            DATA_TYPE = (int)DataHandler.eDataType.FM_DOWN;
            DownloadList = new List<DownloadObject>();
        }
        List<DownloadObject> DownloadList;
        static string Current_Downloading_file;
        static string Current_Downloading_Dir;

        public IConnection MainConnection
        {
            get;

            set;
        }

        public int DATA_TYPE { get; set; }





        public void addToList(string PCPath, string MobPath,bool isDir)
        {
            DownloadObject DO = new DownloadObject();
            DO.PCPath = PCPath;
            DO.MobPath = MobPath;
            DO.progress = 22;
            DO.isDir = isDir;
            DownloadList.Add(DO);
            dataGridView1.Rows.Add(DO.getInfo());
        }
        public void onDataReceived(int Flag, byte[] data)
        {

            if (Flag == (int)Flages.DOWNLOAD_START)
            {
                //current file.append binary packet
            }
            else if (Flag == (int)Flages.DOWNLOAD_FINISHED)
            {

            }

            else if (Flag == (int)Flages.FILE_START)
            {
                Current_Downloading_file = Encoding.UTF8.GetString(data);
                if(!File.Exists(Current_Downloading_Dir+Current_Downloading_file))
                    File.Create(Current_Downloading_Dir+Current_Downloading_file);
            }
            else if (Flag == (int)Flages.FILE_PACKET)
            {
                using (var stream = new FileStream(Current_Downloading_Dir + Current_Downloading_file, FileMode.Append))
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            else if (Flag == (int)Flages.CD)
            {
                if (Directory.Exists(Current_Downloading_Dir))
                    Directory.CreateDirectory(Current_Downloading_Dir);
            }









        }

        public void onErrorReceived(String error)
        {

        }

        private void FileDownloader_Load(object sender, EventArgs e)
        {
            
            DataGridViewProgressColumn column = new DataGridViewProgressColumn();
            column.HeaderText = "Progress";
            column.FillWeight = 30;
            column.Frozen = false;
            column.ReadOnly = true;
            dataGridView1.Columns.Add(column);

            object[] row1 = new object[] { "test1", "test2", 50 };
            object[] row2 = new object[] { "test1", "test2", 55 };
            object[] row3 = new object[] { "test1", "test2", 22 };
            object[] rows = new object[] { row1, row2, row3 };

            foreach (object[] row in rows)
            {
                dataGridView1.Rows.Add(row);
            }
        }

        private class DownloadObject
        {
           public string Name ,PCPath, MobPath;
            public bool isDir;
            public int size;
            public int progress;

            public string[] getInfo ()
            {
                return new string[] { PCPath,(isDir?"Folder":"File"),progress.ToString()};
            }
        }

        private enum Flages
        {
            DOWNLOAD_START,
            FOLDER_START,
            FILE_START,
            FILE_PACKET,
            FILE_END,
            FOLDER_END,
            DOWNLOAD_FINISHED,

            CD
            
        }





        //private static bool CheckWellFormed(string input)
        //{
        //    var stack = new Stack<Flages>();
        //    // dictionary of matching starting and ending pairs
        //    var allowedChars = new Dictionary<Flages, Flages>() { { Flages.FOLDER_START, Flages.FOLDER_END }, { Flages.FILE_START, Flages.FILE_END } };

        //    var wellFormated = true;
        //    foreach (var chr in input)
        //    {
        //        if (allowedChars.ContainsKey(chr))
        //        {
        //            // if starting char then push on stack
        //            stack.Push(chr);
        //        }
        //        // ContainsValue is linear but with a small number is faster than creating another object
        //        else if (allowedChars.ContainsValue(chr))
        //        {
        //            // make sure something to pop if not then know it's not well formated
        //            wellFormated = stack.Any();
        //            if (wellFormated)
        //            {
        //                // hit ending char grab previous starting char
        //                var startingChar = stack.Pop();
        //                // check it is in the dictionary
        //                wellFormated = allowedChars.Contains(new KeyValuePair<char, char>(startingChar, chr));
        //            }
        //            // if not wellformated exit loop no need to continue
        //            if (!wellFormated)
        //            {
        //                break;
        //            }
        //        }
        //    }
        //    return wellFormated;
        //}
    }
}
