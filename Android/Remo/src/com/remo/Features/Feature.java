package com.remo.Features;

import android.annotation.TargetApi;
import android.os.AsyncTask;
import android.os.Build;
import android.util.Log;
import com.remo.Connections.TCP_Transceiver;

/**
 * Created by Mohamed on 2/1/2018.
 */

public abstract class Feature {
    boolean isRootRequired = false;
    int minSDK = 1;
    String Name = "";
    //TCP_Transceiver tcp = new TCP_Transceiver(false);
    boolean isMaainConn = false;
    static boolean stopFlag = false;
    private int Feature_type;
    TCP_Transceiver tcp;

    void init(int Feature_type){
        this.Feature_type = Feature_type;
        tcp = new TCP_Transceiver(isMaainConn);
        tcp.tcpStopFlag = false;
        boolean stopFlag = false;

        Task task = new Task();
        connect();
        executeAsyncTask(task);
    }

    public static void stopStream() {

        //audiorecord.stop();
        //tcp.tcpStopFlag = true;
        stopFlag = true;
        //audiorecord.release();
        Log.e("REMODROID", "Stopped");
    }

    private void AsyncTaskFunc(){
        connect();
    }

    abstract void connect();

    abstract void sendPacket(byte[] data);

    abstract void reportError(String error);

    abstract void disconnect();



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
