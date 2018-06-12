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

namespace Remo.Features
{
    public partial class FileDownloader : Form, IFeature
    {
        public FileDownloader()
        {
            InitializeComponent();
            DATA_TYPE = (int)DataHandler.eDataType.FM_DOWN;
        }

        public IMConnection MainConnection
        {
            get;

            set;
        }

        public int DATA_TYPE { get; set; }

        public void onDataReceived(byte[] data)
        {
            throw new NotImplementedException();
        }

        public void onErrorReceived(String error)
        {

        }

        private void FileDownloader_Load(object sender, EventArgs e)
        {
            
            DataGridViewProgressColumn column = new DataGridViewProgressColumn();
            column.HeaderText = "Progress";
            column.FillWeight = 30;
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


        private enum Flages
        {
            FOLDER_START,
            FILE_START,
            FILE_END,
            FOLDER_END,
        }


        private class DownloadObject
        {
            String Name;
            String Location;
            bool isDir;
            int size;
            int progress;




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
