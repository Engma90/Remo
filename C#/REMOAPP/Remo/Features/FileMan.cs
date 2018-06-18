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
using System.Net;

namespace Remo.Features
{
    public partial class FileMan : Form,IFeature
    {

        private string CurrentPath = "/";
        public int DATA_TYPE { get; set; }
        public IConnection MainConnection { get; set; }

        

        public FileMan()
        {
            InitializeComponent();
            DATA_TYPE = (int)DataHandler.eDataType.FM_LIST;
            CheckForIllegalCrossThreadCalls = false;
        }
        
        public void onErrorReceived(string error)
        {
            throw new NotImplementedException();
        }

        public void onDataReceived(int Flag, byte[] data)
        {

            this.Invoke((MethodInvoker)delegate
            {
                //Console.WriteLine(Encoding.UTF8.GetString(data));
                Console.WriteLine(CurrentPath);
                dataGridView1.Rows.Clear();
                string[] dirs = Encoding.UTF8.GetString(data).Split('\\')[0].Split('/');
                string[] files = Encoding.UTF8.GetString(data).Split('\\')[1].Split('/');

                foreach (string s in dirs)
                {
                    if (!String.Empty.Equals(s))
                        dataGridView1.Rows.Add(new String[] { s, "Folder" });
                }
                foreach (string s in files)
                {
                    if (!String.Empty.Equals(s))
                        dataGridView1.Rows.Add(new String[] { s, "File" });
                }

                dataGridView1.ScrollBars = ScrollBars.Both; // runs on UI thread

                txtAddressBar.Text = CurrentPath;

            });
            
            //dataGridView1.Enabled = false;
            //dataGridView1.ScrollBars = ScrollBars.None;
            //dataGridView1.Enabled = true;
            //dataGridView1.ScrollBars = ScrollBars.Both;
            //dataGridView1.Refresh();




            //Console.WriteLine("FileMan");
            //ServerFactory.GetInstance().send(((int)DataHandler.eDataType.FM_LIST).ToString(),
            //    ((int)DataHandler.eOrderType.STOP).ToString(),
            //        MainConnection.tcpClient);
        }

        private void FileMan_Load(object sender, EventArgs e)
        {
            Console.WriteLine("FileMan");
            ServerFactory.GetInstance().send(((int)DataHandler.eDataType.FM_LIST).ToString(),
                ((int)DataHandler.eOrderType.START).ToString(),"L="+ CurrentPath,
                    MainConnection.tcpClient);


            ServerFactory.GetInstance().send(((int)DataHandler.eDataType.FM_DOWN).ToString(),
                ((int)DataHandler.eOrderType.START).ToString(), //Current_Downloading_Obj.MobPath,
                    MainConnection.tcpClient);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //var dataIndexNo = dataGridView1.Rows[e.RowIndex].Index.ToString();
            string fileName = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            string fileType = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            if (fileType.Equals("Folder"))
            {
                if(CurrentPath.Substring(CurrentPath.Length - 1).Equals("/"))
                    CurrentPath += fileName;
                else
                    CurrentPath += "/" + fileName;
                ServerFactory.GetInstance().send(((int)DataHandler.eDataType.FM_LIST).ToString(),
                ((int)DataHandler.eOrderType.UPDATE).ToString(), "L=" + CurrentPath,
                    MainConnection.tcpClient);
            }
        }

        private void btnUP_Click(object sender, EventArgs e)
        {
            if (! CurrentPath.Equals("/"))
            {
                string[] pathComponents = CurrentPath.Split('/');
                CurrentPath = "/";
                for(int i=1; i<pathComponents.Length - 1 ;i++)
                {
                    CurrentPath += pathComponents[i];
                    if (i < pathComponents.Length - 2)
                        CurrentPath += "/";
                }
                ServerFactory.GetInstance().send(((int)DataHandler.eDataType.FM_LIST).ToString(),
                ((int)DataHandler.eOrderType.UPDATE).ToString(), "L=" + CurrentPath,
                    MainConnection.tcpClient);

            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are You Sure ?","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.Yes)
            {


                foreach(DataGridViewRow r in dataGridView1.SelectedRows)
                {
                    ServerFactory.GetInstance().send(((int)DataHandler.eDataType.FM_LIST).ToString(),
                ((int)DataHandler.eOrderType.UPDATE).ToString(), "D=" + CurrentPath + "/" + r.Cells[0].Value.ToString() ,
                MainConnection.tcpClient);

                }

                
            }
        }



        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            if (fbd.ShowDialog() == DialogResult.OK)
            {

                if (MainForm.FileDownloader == null || MainForm.FileDownloader.IsDisposed)
                {
                    MainForm.FileDownloader = ServerFactory.GetInstance().startFeature(
                        (MainConnection.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString(),
                        (int)DataHandler.eDataType.FM_DOWN);
                    MainForm.FileDownloader.Show();
                    foreach (DataGridViewRow r in dataGridView1.SelectedRows)
                    {
                        ((FileDownloader)MainForm.FileDownloader).addToList(fbd.SelectedPath , CurrentPath , r.Cells[0].Value.ToString(), r.Cells[1].Value.ToString().Equals("Folder"));
                    }
                }
                else
                {

                    //+ "/" + r.Cells[0].Value.ToString()
                    foreach (DataGridViewRow r in dataGridView1.SelectedRows)
                    {
                        ((FileDownloader)MainForm.FileDownloader).addToList(fbd.SelectedPath , CurrentPath , r.Cells[0].Value.ToString(), r.Cells[1].Value.ToString().Equals("Folder"));
                    }
                }
            }




        }



        private void FileMan_FormClosing(object sender, FormClosingEventArgs e)
        {
            ServerFactory.GetInstance().send(((int)DataHandler.eDataType.FM_LIST).ToString(),
                ((int)DataHandler.eOrderType.STOP).ToString(),
                    MainConnection.tcpClient);
        }


    }
}
