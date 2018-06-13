package com.remo.connections;

import android.app.Service;
import android.content.Intent;
import android.os.IBinder;
import android.util.Log;

public class MainConnectionService extends Service {
    private Thread mainThread = null;
    private TCP_Transceiver tcp_transceiver;

    public MainConnectionService() {
    }
    @Override
    public IBinder onBind(Intent intent) {
        // TODO: Return the communication channel to the service.
        throw new UnsupportedOperationException("Not yet implemented");
    }


    @Override
    public int onStartCommand(final Intent intent, int flags, int startId) {
        Log.d("REMODROID", "Main Service Started");

        mainThread = new Thread(new Runnable() {
            @Override
            public void run() {
                tcp_transceiver = new TCP_Transceiver(true);
                tcp_transceiver.connect();
                Log.d("REMODROID", "Receiving Orders");
                while (true) {
                    try {
                        String order = tcp_transceiver.receive();
                        Log.d("REMODROID", "Received " + order);

                        if (order.equals("")) {
                            tcp_transceiver.connect();
                        } else {
                            DataHandler.distribute(order, getApplicationContext());
                        }

                    } catch (Exception ex) {
                        Log.e("REMODROID","Receive Exception: " + ex.getMessage());
                        tcp_transceiver.connect();
                        break;
                    }
                }
            }
        });
        mainThread.setUncaughtExceptionHandler(new Thread.UncaughtExceptionHandler() {
            @Override
            public void uncaughtException(Thread t, Throwable e) {
                Log.e("REMODROID", "UncaughtException :" + e.getMessage());
                tcp_transceiver.connect();
            }
        });

        mainThread.start();

        return START_STICKY;
    }
    @Override
    public void onDestroy(){
        try {
            Log.d("REMODROID","Main Service Destroy ");
            mainThread.interrupt();
            super.onDestroy();
        }catch (Exception ex){
            Log.e("REMODROID","Main Service Destroy Exception: " + ex.getMessage());
        }
    }
}
