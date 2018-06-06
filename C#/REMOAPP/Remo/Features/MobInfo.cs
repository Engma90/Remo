using Remo.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remo.Features
{
    class MobInfo 
    {
        public int DATA_TYPE {get; set;}
        

        public IMClient mc { get; set; }

        public void onError(string error)
        {
            throw new NotImplementedException();
        }

        public void Show()
        {
            throw new NotImplementedException();
        }

        public void updateData(byte[] data)
        {
            //MainForm.
        }
    }
}
