package com.remo.features.broadcastReceivers;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.util.Log;
import com.remo.MainActivity;
import com.remo.MainService;

public class startup extends BroadcastReceiver {

    @Override
    public void onReceive(Context context, Intent intent) {

        Log.d("REMODROID", "startup Received");
        context.startService(new Intent(context,MainService.class));
    }
}
