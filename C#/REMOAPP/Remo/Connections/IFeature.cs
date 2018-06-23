using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Remo.Connections
{
    /// <summary>
    /// Feature interface.
    /// </summary>
    public interface IFeature
    {

        /// <summary>
        /// Client main connection reference
        /// </summary>
        IConnection MainConnection { get; set; }

        /// <summary>
        /// Feature type
        /// </summary>
        int DATA_TYPE { get; set; }


        /// <summary>
        /// Called when data received from client to this feature
        /// </summary>
        /// <param name="Flag">An int number determines the flage of data eg (CD,FILE_PACKET).</param>
        /// <param name="data">A byte array contains data.</param>
        void onDataReceived(int Flag,byte[] data);

        /// <summary>
        /// Called when error received from client to this feature
        /// </summary>
        /// <param name="error">A string contains the error.</param>
        void onErrorReceived(String error);

        void Show();
        bool IsDisposed { get; }
        void Dispose();
    }
}
