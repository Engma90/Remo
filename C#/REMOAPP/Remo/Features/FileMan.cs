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
        }

        public IMClient mc { get; set; }

        public int DATA_TYPE { get; set; }

        public void onError(string error)
        {
            throw new NotImplementedException();
        }

        public void updateData(byte[] data)
        {
            Console.WriteLine(Encoding.UTF8.GetString(data));
            
            string[] dirs = Encoding.UTF8.GetString(data).Split('\\')[0].Split('/');
            string[] files = Encoding.UTF8.GetString(data).Split('\\')[1].Split('/');

            foreach(string s in dirs)
            {
                dataGridView1.Rows.Add(new String[] { s, "-" });
            }
            foreach (string s in files)
            {
                dataGridView1.Rows.Add(new String[] { s, "file" });
            }


            //Console.WriteLine("FileMan");
            //mTCPHandler.GetInstance().send(((int)DataHandler.eDataType.FM_LIST).ToString(),
            //    ((int)DataHandler.eOrderType.STOP).ToString(),
            //        mc.tcpClient);
        }

        private void FileMan_Load(object sender, EventArgs e)
        {
            Console.WriteLine("FileMan");
            mTCPHandler.GetInstance().send(((int)DataHandler.eDataType.FM_LIST).ToString(),
                ((int)DataHandler.eOrderType.START).ToString(), "Internal",
                    mc.tcpClient);
        }
    }
}
