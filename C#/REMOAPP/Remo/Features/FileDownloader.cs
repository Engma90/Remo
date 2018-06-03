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
    public partial class FileDownloader : Form, IFeature
    {
        public FileDownloader()
        {
            InitializeComponent();
            DATA_TYPE = (int)DataHandler.eDataType.DATA_TYPE_FM_DOWN_START;
        }

        public IMClient mc
        {
            get;

            set;
        }

        public int DATA_TYPE { get; set; }

        public void updateData(byte[] data)
        {
            throw new NotImplementedException();
        }

        public void onError(String error)
        {

        }

        private void FileDownloader_Load(object sender, EventArgs e)
        {

        }
    }
}
