package com.remo.connections;

import android.util.Log;

/**
 * Created by Mohamed on 2/1/2018.
 */

public abstract class Feature {
    protected boolean isRootRequired = false;
    protected int minSDK = 1;
    protected boolean UseMainConnection = false;
    protected boolean stopFlag = false;
    protected int Feature_type;
    protected TCP_Transceiver tcp;

    void start(int Feature_type,String Params){//int Feature_type
        this.Feature_type = Feature_type;
        tcp = new TCP_Transceiver(UseMainConnection);
        connect();
        tcp.tcpStopFlag = false;
        stopFlag = false;

        Thread t = new Thread(new Runnable() {
            @Override
            public void run() {
                AsyncTaskFunc(Params);
               // tcp.disconnect();
            }
        });
        t.start();
    }








    private void connect(){
         tcp.Feature_type = Feature_type;
         tcp.connect();
    }

    public void sendPacket(int flag,byte[] data){
        tcp.send(Feature_type,flag, data);
    }


    protected void stop() {
        stopFlag = true;
        //tcp.disconnect();
        Log.e("REMODROID", "Stopped");
    }


    public abstract void AsyncTaskFunc(String Params);
    public abstract void update(String Params);

//    abstract void reportError(String error);
}
