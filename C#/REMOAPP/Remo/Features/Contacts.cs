using Remo.Connections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Remo.Features
{
    public partial class Contacts : Form, IFeature
    {
        public Contacts()
        {
            InitializeComponent();
            DATA_TYPE = (int)DataHandler.eDataType.CONTACTS;
            CheckForIllegalCrossThreadCalls = false;
        }

        public IMClient mc { get; set; }

        public int DATA_TYPE { get; set; }

        public void onError(string error)
        {
            throw new NotImplementedException();
        }

        public void updateData(byte[] data)
        {
            this.Invoke((MethodInvoker)delegate
            {
                Console.WriteLine(Encoding.UTF8.GetString(data));

                string[] Name_Num = Encoding.UTF8.GetString(data).Split('\\');
               // string[] files = Encoding.UTF8.GetString(data).Split('\\')[1].Split('/');

                foreach (string s in Name_Num)
                {
                    if (!String.Empty.Equals(s))
                    {
                        dataGridView1.Rows.Add(new String[] { s.Split('/')[0], s.Split('/')[1] });
                    }
                }
                dataGridView1.ScrollBars = ScrollBars.Both; // runs on UI thread
            });
        }

        private void Contacts_Load(object sender, EventArgs e)
        {
            //Console.WriteLine("FileMan");
            mTCPHandler.GetInstance().send(((int)DataHandler.eDataType.CONTACTS).ToString(),
                ((int)DataHandler.eOrderType.START).ToString(),
                    mc.tcpClient);
        }
    }
}
