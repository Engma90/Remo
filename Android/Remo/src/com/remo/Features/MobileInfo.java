package com.remo.Features;

import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.os.BatteryManager;
import android.os.Build;
import android.util.Log;
import com.remo.Connections.DataHandler;
import com.remo.Connections.TCP_Transceiver;

/**
 * Created by Mohamed on 3/3/2018.
 */

public class MobileInfo implements Feature {
    private String Manufacturer = "";
    private String Battery = "";
    private TCP_Transceiver tcp;

    public MobileInfo() {

    }

    public String getInfo(Context context) {
        IntentFilter ifilter = new IntentFilter(Intent.ACTION_BATTERY_CHANGED);
        Intent batteryStatus = context.registerReceiver(null, ifilter);

        int level = batteryStatus.getIntExtra(BatteryManager.EXTRA_LEVEL, -1);
        int scale = batteryStatus.getIntExtra(BatteryManager.EXTRA_SCALE, -1);

        float batteryPct = (level / (float) scale) * 100;

        String device = Build.MANUFACTURER + " " + Build.MODEL;
        return device + "/" + ((int) batteryPct);
    }

    public void sendInfo(Context context) {
        Log.d("REMODROID", "4");
        connect();
        Log.d("REMODROID", tcp.isConnected + "");
        Log.d("REMODROID", "5");
        try {
            Log.d("REMODROID", "6");
            sendPacket(((getInfo(context)).getBytes("UTF-8")));
            Log.d("REMODROID", "7");
        } catch (Exception e) {
            Log.e("REMODROID", e.getLocalizedMessage());
            reportError("Error");
        }

    }

    @Override
    public void connect() {
        try {
            this.tcp = TCP_Transceiver.MainConn;
        } catch (Exception ex) {
            Log.e("REMODROID", ex.getMessage());
        }

    }

    @Override
    public void sendPacket(byte[] data) {
        tcp.send(DataHandler.eDataType.DATA_TYPE_INFO.ordinal(), data);
    }

    @Override
    public void reportError(String error) {

    }

    @Override
    public void disconnect() {

    }
}
