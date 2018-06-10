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
    public partial class GPS : Form,IFeature
    {
        public GPS()
        {
            InitializeComponent();
            DATA_TYPE = (int)DataHandler.eDataType.GPS;
        }

        public int DATA_TYPE { get; set; }

        public IMClient mc { get; set; }

        public void onError(string error)
        {
            throw new NotImplementedException();
        }

        public void updateData(byte[] data)
        {
            textBox1.Text = Encoding.UTF8.GetString(data);
        }

        private void GPS_Load(object sender, EventArgs e)
        {
            mTCPHandler.GetInstance().send(((int)DataHandler.eDataType.GPS).ToString(),
                ((int)DataHandler.eOrderType.START).ToString(),
                    mc.tcpClient);
        }

        private void btnMap_Click(object sender, EventArgs e)
        {

        }
    }
}
