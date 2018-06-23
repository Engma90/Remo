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
using System.Threading;

namespace Remo.Features
{
    public partial class FileDownloader : Form, IFeature
    {

        public IConnection MainConnection { get; set; }
        public int DATA_TYPE { get; set; }
        private Thread RefreshDownloadListThread;
        private bool Stop = false;

        static List<DownloadObject> DownloadList;
        static string current_file_name = "";
        static string current_sub_dir = "";
        static DownloadObject Current_Downloading_Obj;


        public FileDownloader()
        {
            InitializeComponent();
            DATA_TYPE = (int)DataHandler.eDataType.FM_DOWN;
            DownloadList = new List<DownloadObject>();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void Download()
        {
            foreach (DownloadObject d in DownloadList.ToList())
            {
                if (!d.isCompleated)
                {
                    Current_Downloading_Obj = d;
                    d.startDownload(MainConnection, dataGridView1.Rows[Current_Downloading_Obj.RowIndex].Cells[3]);
                }
               // DownloadList.Remove(d);
            }
        }



        private void FileDownloader_Load(object sender, EventArgs e)
        {

            //DataGridViewProgressColumn column = new DataGridViewProgressColumn();
            //column.HeaderText = "Progress";
            //column.FillWeight = 30;
            //column.Frozen = false;
            //column.ReadOnly = true;
            //dataGridView1.Columns.Add(column);



            RefreshDownloadListThread = new Thread(delegate ()
            {
                while (!Stop)
                {
                    try
                    {

                        Download();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("DownloadThread Ex" + ex.ToString());
                    }
                    Thread.Sleep(5000);



                }
            });

            RefreshDownloadListThread.Start();

        }

        //public void updateDGVProgress(int progress)
        //{
        //        Current_Downloading_Obj.getInfo(progress);
        //}




        public void addToList(string PCPath, string MobPath, string Name, bool isDir)
        {
            DownloadObject DO = new DownloadObject();

            if (MobPath.StartsWith("/"))
            {
                DO.MobPath = MobPath.Remove(0,1);
            }
            DO.PCPath = PCPath;
            
            DO.Name = Name;
            DO.isDir = isDir;
            DownloadList.Add(DO);
            DO.RowIndex = dataGridView1.Rows.Add(DO.getInfo());


        }
        public void onDataReceived(int Flag, byte[] data)
        {
            Invoke(new Action(() =>
            {
              //  Console.WriteLine("FM_DOWN onDataReceived Flag {0} Data {1}", Flag, Encoding.UTF8.GetString(data));
                if (Flag == (int)Flages.FILE_START)
                {
                    Console.WriteLine(getFullFilePath());
                    Current_Downloading_Obj.isStarted = true;
                    current_file_name = Encoding.UTF8.GetString(data);
                    var myFile = File.Create(getFullFilePath());
                    myFile.Close();
                 //   dataGridView1.Rows[Current_Downloading_Obj.RowIndex].Cells[3].Value = "Downloading";
                }
                else if (Flag == (int)Flages.FILE_PACKET)
                {
                    Console.WriteLine("getFullFilePath() : "+ getFullFilePath());
                    using (var stream = new FileStream(getFullFilePath(), FileMode.Append))
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
                else if (Flag == (int)Flages.FILE_FINISHED)
                {
                    // dataGridView1.Rows[Current_Downloading_Obj.RowIndex].Cells[3].Value = "Finished";
                   // Current_Downloading_Obj.isCompleated = true;//
                }

                else if (Flag == (int)Flages.CD)
                {
                    Console.WriteLine("MobPath = " + Current_Downloading_Obj.MobPath);
                    current_sub_dir = Encoding.UTF8.GetString(data).Remove(0,Current_Downloading_Obj.MobPath.Length).Replace("/","\\");
                    if (!Directory.Exists(getFullDirPath()))
                        Directory.CreateDirectory(getFullDirPath());
                }

                if (Flag == (int)Flages.DOWNLOAD_OBJ_FINISHED)
                {
                    // dataGridView1.Rows[Current_Downloading_Obj.RowIndex].Cells[3].Value = "Finished";
                     Current_Downloading_Obj.isCompleated = true;
                }

            }));



        }



        private String getFullDirPath()
        {
            //   Current_Downloading_Obj = DownloadList[0];
            return Current_Downloading_Obj.PCPath
                + "\\"
                + current_sub_dir;
        }
        private String getFullFilePath()
        {
           // Current_Downloading_Obj = DownloadList[0];
            return Current_Downloading_Obj.PCPath
                + "\\"
                + current_sub_dir
                + "\\"
                + current_file_name;
        }


        public void onErrorReceived(String error)
        {

        }




        private enum Flages
        {
            DOWNLOAD_OBJ_START,
            FILE_START,
            FILE_PACKET,
            FILE_FINISHED,
            DOWNLOAD_OBJ_FINISHED,
            CD

        }

        private void FileDownloader_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop = true;
            RefreshDownloadListThread.Abort();
        }




        private class DownloadObject
        {
            public string Name, PCPath, MobPath;
            public bool isDir;
            public int size = 309238;
            public bool isStarted;
            public bool isCompleated;
            public int RowIndex;

            public string[] getInfo()
            {
                //   this.Downloaded_size += Downloaded_size;
                // progress = (int)(((double)Downloaded_size / (double)size) * 100);
                if (!isStarted)
                    return new string[] { PCPath + "\\" + Name, (isDir ? "Folder" : "File"), size.ToString(), "Pending" };
                return new string[] { PCPath +"\\"+ Name, (isDir ? "Folder" : "File"), size.ToString(), isCompleated ? "Finished" : "Downloading" };
            }

            public void startDownload(IConnection MainConnection,DataGridViewCell Status)
            {
                isCompleated = false;
                MainForm.mTCPH.send(((int)DataHandler.eDataType.FM_DOWN).ToString(),
                    ((int)DataHandler.eOrderType.UPDATE).ToString(), MobPath +"/"+ Name,
                    MainConnection.tcpClient);

                Status.Value = "Downloading";
                while (!isCompleated)
                {
                    Thread.Sleep(1000);
                }
                Status.Value = "Finished";

            }
        }




    }
}
