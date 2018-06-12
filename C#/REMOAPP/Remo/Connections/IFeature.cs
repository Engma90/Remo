using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Remo.Connections
{
    public interface IFeature
    {
        IMConnection MainConnection { get; set; }
        int DATA_TYPE { get; set; }
        void onDataReceived(byte[] data);
        void onErrorReceived(String error);
        void Show();
    }
}
