package com.remo.Connections;

import android.content.Context;
import android.util.Log;
import com.remo.Features.*;

import java.io.UnsupportedEncodingException;

public class DataHandler {

    public static void distribute(int order, Context context) {

        if (order == eDataType.DATA_TYPE_INFO.ordinal()) {

            MobileInfo mi = new MobileInfo();
            Log.d("REMODROID", mi.getInfo(context));
            mi.sendInfo(context);


        }else if (order == eDataType.DATA_TYPE_CAM_START.ordinal()) {
            //Intent intent = new Intent(context, CamStream.class);
            //intent.addFlags(FLAG_ACTIVITY_NEW_TASK);
            //context.startService(intent);
            Feature c = new CamStream2();

        } else if (order == eDataType.DATA_TYPE_MIC_START.ordinal()) {
            //Intent intent = new Intent(context, com.remo.Features.MicStream.class);
            //intent.addFlags(FLAG_ACTIVITY_NEW_TASK);
            //context.startService(intent);
            Feature c = new MicStream2();

        } else if (order == eDataType.DATA_TYPE_MIC_STOP.ordinal()) {
            //Intent intent = new Intent(context, com.remo.Features.MicStream.class);
            //intent.addFlags(FLAG_ACTIVITY_NEW_TASK);
            //context.startService(intent);
            MicStream2.stopStream();

        } else if (order == eDataType.DATA_TYPE_CAM_STOP.ordinal()) {
            CamStream2.stopCam();

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