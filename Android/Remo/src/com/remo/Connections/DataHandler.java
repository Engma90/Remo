package com.remo.Connections;

import android.content.Context;
import android.util.Log;
import com.remo.Features.*;

import java.util.HashMap;
import java.util.Map;

public class DataHandler {

    static Map<Integer, Feature> FDict = new HashMap<>();

    static void distribute(int order, Context context) {

        if (order == eDataType.DATA_TYPE_INFO.ordinal()) {

//            MobileInfo mi = new MobileInfo();
//            Log.d("REMODROID", mi.getInfo(context));
//            mi.sendInfo(context);

//            MobileInfo mi = new MobileInfo();
//            mi.init();
            try {
                Feature c = new MobileInfo();
                c.isMaainConn = true;
                c.init(order);
                FDict.put(order,c);
            }catch (Exception ex){
                Log.e("REMODROID", ex.getMessage());
            }



        }else if (order == eDataType.DATA_TYPE_CAM_START.ordinal()) {
            Log.d("REMODROID", "Cam");
            Feature c = new CamStream();
            FDict.put(order,c);
            c.init(order);

        } else if (order == eDataType.DATA_TYPE_MIC_START.ordinal()) {
            Feature c = new MicStream();
            FDict.put(order,c);
            c.init(order);

        } else if (order == eDataType.DATA_TYPE_MIC_STOP.ordinal()) {
            FDict.get(eDataType.DATA_TYPE_MIC_START.ordinal()).stopStream();
            FDict.remove(eDataType.DATA_TYPE_MIC_START.ordinal());


        } else if (order == eDataType.DATA_TYPE_CAM_STOP.ordinal()) {
            FDict.get(eDataType.DATA_TYPE_CAM_START.ordinal()).stopStream();
            FDict.remove(eDataType.DATA_TYPE_CAM_START.ordinal());

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