using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remo.Connections
{

    static class DataHandler
    {

        ////public DataHandler()
        ////{

        ////}



        public static void distribute(int dataType,byte[] data, IClient c)
        {
            //Console.WriteLine(eDataType.DATA_TYPE_INFO);
            if (dataType == (int) eDataType.DATA_TYPE_INFO)
            {
                Console.WriteLine(Encoding.UTF8.GetString(data));
                c.MANUFACTURER = Encoding.UTF8.GetString(data).Split('/')[0];
                c.BATTERY_LEVEL = Encoding.UTF8.GetString(data).Split('/')[1];
                c.LastChecked = DateTime.Now;
                c.isMainConn = true;
                Console.WriteLine("Last Cheked Updated: "+ DateTime.Now);
            }

            else if (dataType == (int) eDataType.DATA_TYPE_CAM_START)
            {
                c.MainConnection.cam.updateData(data);

            }

            else if (dataType == (int)eDataType.DATA_TYPE_MIC_START)
            {
                c.MainConnection.Mic.updateData(data);

            }

        }



        public enum eDataType
        {
            DATA_TYPE_INFO,
            DATA_TYPE_ERROR,
            DATA_TYPE_CAM_START,
            DATA_TYPE_MIC_START,
            DATA_TYPE_CAM_STOP,
            DATA_TYPE_MIC_STOP,
            DATA_TYPE_FM_LIST,
            DATA_TYPE_FM_DOWN_START,
            DATA_TYPE_FM_DOWN_STOP
        }
    }
}
