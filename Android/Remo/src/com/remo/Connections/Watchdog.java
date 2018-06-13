package com.remo.connections;

import android.app.Service;
import android.content.Intent;
import android.os.IBinder;


public class Watchdog extends Service {
    public Watchdog() {
    }

    public int onStartCommand(Intent intent, int flags, int startId) {

        try {
            //stopService(new Intent(getApplicationContext(),com.remo.connections.MainConnectionService.class));
        } catch (Exception ex) {

        }
        //startService(new Intent(getApplicationContext(),com.remo.connections.MainConnectionService.class));
        return START_NOT_STICKY;
    }


    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }
}
