package com.remo.Connections;

import android.content.Context;
import android.util.Log;
import com.remo.Features.*;

public class DataHandler {

    public static void distribute(int order, Context context) {

        if (order == eDataType.DATA_TYPE_INFO.ordinal()) {

            MobileInfo mi = new MobileInfo();
            Log.d("REMODROID", mi.getInfo(context));
            mi.sendInfo(context);


        }else if (order == eDataType.DATA_TYPE_CAM_START.ordinal()) {
            Feature c = new CamStream();

        } else if (order == eDataType.DATA_TYPE_MIC_START.ordinal()) {
            Feature c = new MicStream();

        } else if (order == eDataType.DATA_TYPE_MIC_STOP.ordinal()) {
            MicStream.stopStream();

        } else if (order == eDataType.DATA_TYPE_CAM_STOP.ordinal()) {
            CamStream.stopCam();

        }

    }


    public enum eConnectionType
    {
        connection_type_Main,
        connection_type_Feature

    }
    public enum eDataType
    {
        DATA_TYPE_CAM_START,
        DATA_TYPE_MIC_START,
        DATA_TYPE_FM_LIST,
        DATA_TYPE_FM_DOWN_START,
        DATA_TYPE_INFO,
        DATA_TYPE_INIT_CONNECTION,
        DATA_TYPE_ERROR,
        DATA_TYPE_CAM_STOP,
        DATA_TYPE_MIC_STOP,
        DATA_TYPE_FM_DOWN_STOP
    }
}