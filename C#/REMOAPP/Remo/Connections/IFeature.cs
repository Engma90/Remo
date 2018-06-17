﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Remo.Connections
{
    public interface IFeature
    {
        IConnection MainConnection { get; set; }
        int DATA_TYPE { get; set; }
        void onDataReceived(int Flag,byte[] data);
        void onErrorReceived(String error);
        void Show();
        bool IsDisposed { get; }
        void Dispose();
    }
}
