package com.remo.Connections;

import android.annotation.TargetApi;
import android.os.AsyncTask;
import android.os.Build;
import android.util.Log;

import java.io.IOException;

/**
 * Created by Mohamed on 2/1/2018.
 */

public abstract class Feature {
    boolean isRootRequired = false;
    int minSDK = 1;
    String Name = "";
    //TCP_Transceiver tcp = new TCP_Transceiver(false);
    public boolean UseMainConnection = false;
    public boolean stopFlag = false;
    public int Feature_type;
    public TCP_Transceiver tcp;

    public void init(int Feature_type){
        this.Feature_type = Feature_type;
        if(UseMainConnection) {
            tcp = TCP_Transceiver.MainConn;
            Feature_type = 1;
        }
        else {
            tcp = new TCP_Transceiver(UseMainConnection);
            connect();
        }

        tcp.tcpStopFlag = false;
        stopFlag = false;

//        Task task = new Task();
//
//        executeAsyncTask(task);
        Thread t = new Thread(new Runnable() {
            @Override
            public void run() {
                AsyncTaskFunc();
            }
        });
        t.start();
    }




    public void stopStream() {

        //audiorecord.stop();
        //tcp.tcpStopFlag = true;
        stopFlag = true;
        //audiorecord.release();
        Log.e("REMODROID", "Stopped");
    }

    public abstract void AsyncTaskFunc();

    public void connect(){
         tcp.Feature_type = Feature_type;
         tcp.connect();
    }

    public void sendPacket(byte[] data){
        tcp.send(Feature_type, data);
//        Log.d("REMODROID", "Waiting For Ack");
//        String Ack = null;
//        try {
//            Ack = tcp.receive();
//        } catch (IOException e) {
//            Log.e("REMODROID", "Ack Ack Error");
//        }
//        if(Ack.equals("OK")){
//            Log.d("REMODROID", "Ack Ok");
//        }
    }

//    abstract void reportError(String error);


    @TargetApi(Build.VERSION_CODES.HONEYCOMB) // API 11
    public static <T> void executeAsyncTask(AsyncTask<T, ?, ?> asyncTask, T... params) {
        if(Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB)
            asyncTask.executeOnExecutor(AsyncTask.THREAD_POOL_EXECUTOR, params);
        else
            asyncTask.execute(params);
    }
    private class Task extends AsyncTask<String, Void, String> {

        @Override
        protected String doInBackground(String... params) {
                    AsyncTaskFunc();
            return null;
        }
    }

}
