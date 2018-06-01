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
    public partial class FileMan : Form
    {
        public FileMan()
        {
            InitializeComponent();
        }

        public IClient c
        {
            get;
            set;
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
