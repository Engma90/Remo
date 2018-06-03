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
        IMClient mc { get; set; }
        int DATA_TYPE { get; set; }
        void updateData(byte[] data);
        void onError(String error);
        void Show();
    }
}
