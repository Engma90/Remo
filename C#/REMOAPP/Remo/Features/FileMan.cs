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
    public partial class FileMan : Form,IFeature
    {
        public FileMan()
        {
            InitializeComponent();
            DATA_TYPE = (int)DataHandler.eDataType.FM_LIST;
            CheckForIllegalCrossThreadCalls = false;
        }

        public IMConnection MainConnection { get; set; }

        public int DATA_TYPE { get; set; }

        public void onErrorReceived(string error)
        {
            throw new NotImplementedException();
        }

        public void onDataReceived(byte[] data)
        {

            this.Invoke((MethodInvoker)delegate
            {
                Console.WriteLine(Encoding.UTF8.GetString(data));

                string[] dirs = Encoding.UTF8.GetString(data).Split('\\')[0].Split('/');
                string[] files = Encoding.UTF8.GetString(data).Split('\\')[1].Split('/');

                foreach (string s in dirs)
                {
                    if (!String.Empty.Equals(s))
                        dataGridView1.Rows.Add(new String[] { s, "-" });
                }
                foreach (string s in files)
                {
                    if (!String.Empty.Equals(s))
                        dataGridView1.Rows.Add(new String[] { s, "file" });
                }

                dataGridView1.ScrollBars = ScrollBars.Both; // runs on UI thread
            });
            
            //dataGridView1.Enabled = false;
            //dataGridView1.ScrollBars = ScrollBars.None;
            //dataGridView1.Enabled = true;
            //dataGridView1.ScrollBars = ScrollBars.Both;
            //dataGridView1.Refresh();




            //Console.WriteLine("FileMan");
            //mTCPHandler.GetInstance().send(((int)DataHandler.eDataType.FM_LIST).ToString(),
            //    ((int)DataHandler.eOrderType.STOP).ToString(),
            //        MainConnection.tcpClient);
        }

        private void FileMan_Load(object sender, EventArgs e)
        {
            Console.WriteLine("FileMan");
            mTCPHandler.GetInstance().send(((int)DataHandler.eDataType.FM_LIST).ToString(),
                ((int)DataHandler.eOrderType.START).ToString(), "Internal",
                    MainConnection.tcpClient);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var dataIndexNo = dataGridView1.Rows[e.RowIndex].Index.ToString();
            string fileName = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
        }
    }
}
