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

        private string url = String.Empty, Latitude = String.Empty, Longitude = String.Empty;
        private readonly string Key = "AIzaSyAs_UvGLCbaWSEWPpvDHCUGPxlllRdUrSw";

        public GPS()
        {
            InitializeComponent();
            DATA_TYPE = (int)DataHandler.eDataType.GPS;
        }

        public int DATA_TYPE { get; set; }

        public IMConnection MainConnection { get; set; }

        public void onErrorReceived(string error)
        {
            throw new NotImplementedException();
        }

        public void onDataReceived(int Flag, byte[] data)
        {
            try
            {
                Latitude = Encoding.UTF8.GetString(data).Split('/')[0];
                Longitude = Encoding.UTF8.GetString(data).Split('/')[1];
                string keyPart = "&key=" + Key;
                url = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + Latitude + "," + Longitude;
                using (WebClient wc = new WebClient())
                {
                    var json = wc.DownloadString(url + keyPart);
                    JObject data1 = JObject.Parse(json);
                    if (data1["status"].ToString().Equals("OK"))
                    {
                        Console.WriteLine("Getting Result");
                        String Address = data1["results"][0]["formatted_address"].ToString();

                        textBox1.Text = Address;
                    }
                    else
                    {
                        MessageBox.Show("Error");
                    }
                }






            }catch(Exception e)
            {
                Console.WriteLine("UPDATE DATA EX: "+ e.Message);
            }


            // 
        }

        private void GPS_Load(object sender, EventArgs e)
        {
            mTCPHandler.GetInstance().send(((int)DataHandler.eDataType.GPS).ToString(),
                ((int)DataHandler.eOrderType.START).ToString(),
                    MainConnection.tcpClient);
        }

        private void btnMap_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.google.com/maps/@"+Latitude+","+Longitude + ",15z");
        }
    }
}
