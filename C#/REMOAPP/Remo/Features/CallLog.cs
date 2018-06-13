using Remo.Connections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Remo.Features
{
    public partial class CallLog : Form, IFeature
    {
        public CallLog()
        {
            InitializeComponent();
            DATA_TYPE = (int)DataHandler.eDataType.CALL_LOG;
            CheckForIllegalCrossThreadCalls = false;
        }

        public IMConnection MainConnection { get; set; }

        public int DATA_TYPE { get; set; }

        public void onErrorReceived(string error)
        {
            throw new NotImplementedException();
        }

        public void onDataReceived(int Flag, byte[] data)
        {
            this.Invoke((MethodInvoker)delegate
            {
             //   Console.WriteLine(Encoding.UTF8.GetString(data));

                string[] Name_Num = Encoding.UTF8.GetString(data).Split('\\');
               // string[] files = Encoding.UTF8.GetString(data).Split('\\')[1].Split('/');

                foreach (string s in Name_Num)
                {
                    if (!String.Empty.Equals(s))
                    {
                        
                        dataGridView1.Rows.Add(s.Split('/'));
                    }
                }
                dataGridView1.ScrollBars = ScrollBars.Both; // runs on UI thread
            });
        }


        private void SMS_Load(object sender, EventArgs e)
        {
            //Console.WriteLine("FileMan");
            mTCPHandler.GetInstance().send(((int)DataHandler.eDataType.CALL_LOG).ToString(),
                ((int)DataHandler.eOrderType.START).ToString(),
                    MainConnection.tcpClient);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
           
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.DefaultExt = "csv";
            sfd.Filter = "csv|*.csv";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
                dataGridView1.SelectAll();
                DataObject dataObject = dataGridView1.GetClipboardContent();
                File.WriteAllText(sfd.FileName, dataObject.GetText(TextDataFormat.CommaSeparatedValue), Encoding.UTF8);
            }
        }
    }
}
