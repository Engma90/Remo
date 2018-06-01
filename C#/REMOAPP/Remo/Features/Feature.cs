using Remo.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Remo.Features
{
    public interface Feature
    {
        IMainClient c { get; set; }
        void updateData(byte[] data);
        void onError(String error);
        void Show();
    }
}
