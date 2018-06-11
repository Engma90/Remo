using Newtonsoft.Json.Linq;
using Remo.Connections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
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
            string Latitude = Encoding.UTF8.GetString(data).Split('/')[0];
            string Longitude = Encoding.UTF8.GetString(data).Split('/')[1];
            //textBox1.Text = Encoding.UTF8.GetString(data);
            string url = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + Latitude + "," + Longitude;//+ ",15z"
            Console.WriteLine(url);

            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString(url);
               // Console.WriteLine(json);
                dynamic data1 = JObject.Parse(json);
                Console.WriteLine(data1.results);
                Console.WriteLine(data1.formatted_address);
                Console.WriteLine(data1.address_components);



                textBox1.Text = data1.results;
                textBox1.ScrollBars = ScrollBars.Both;
                textBox1.Anchor = AnchorStyles.Top;
                textBox1.Anchor = AnchorStyles.Right;
                textBox1.Anchor = AnchorStyles.Left;
                textBox1.Anchor = AnchorStyles.Bottom;
            }









            // 
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
