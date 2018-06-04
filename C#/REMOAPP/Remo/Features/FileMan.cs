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
            throw new NotImplementedException();
        }

        private void FileMan_Load(object sender, EventArgs e)
        {

        }
    }
}
