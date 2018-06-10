package com.remo.Connections;

import android.util.Log;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.net.InetSocketAddress;
import java.net.Socket;

/**
 * Created by Mohamed on 3/2/2018.
 */

public class TCP_Transceiver {

    //private static final TCP_Transceiver instance  = new TCP_Transceiver();
    private int port;
    private String ip;
    public boolean isConnected = false;
    private InputStream in;
    private DataOutputStream bufferedWriter;
    private static TCP_Transceiver MainConn;
    public boolean tcpStopFlag = false;
    private Socket socket;
    public int Feature_type;

//public static enum eDataType{
//    DATA_TYPE_INFO ,
//    DATA_TYPE_CAM ,
//    DATA_TYPE_MIC
//}
    //public static final int DATA_TYPE_INFO = 0;
    //public static final int DATA_TYPE_SMS = 1;

    public TCP_Transceiver(boolean isMainConn) {
        ServerInfo si = new ServerInfo();
        si.init();
        this.ip = si.getIp();
        this.port = si.getPort();
        //this.isMainConn = isMainConn;
        //this.context = App.get().getApplicationContext();
        if (isMainConn) {
            MainConn = this;
            Feature_type = -1;
        }
    }

//    public static TCP_Transceiver GetInstance(boolean isMainConn){
//        this.isMainConn = isMainConn;
//        //t.Type = Type;
//        return new TCP_Transceiver();
//        //return instance;
//    }


    //@Override
    //public Object clone(){
    //return GetInstance();
    //}




    void send(int DATA_TYPE, byte[] data) {

//        try {

        try {
            Log.d("REMODROID", "Sending Data of type: " + DATA_TYPE);
            //Thread.sleep(50);
            //SystemClock.sleep(1500);
            //Log.d("REMODROID", "Sending Message of Length: " + (int)(4 + 4 + data.length));
            //int totalLen = 4 + data.length;
            bufferedWriter.writeInt(data.length);//Max Size 2147483647 = 2 GiB
            //       SystemClock.sleep(100);
 //           Thread.sleep(50);
            bufferedWriter.writeInt(DATA_TYPE);
  //          Thread.sleep(50);
            //         SystemClock.sleep(50);
            bufferedWriter.write(data);
    //        Thread.sleep(50);
            //        SystemClock.sleep(50);
            bufferedWriter.flush();
    //        Thread.sleep(50);
            //         SystemClock.sleep(10);
            //socket.close();
        } catch (Exception e) {
            Log.d("REMODROID", "Sending Exception " + e.getMessage());
            tcpStopFlag = true;
        }
//    } catch (Exception e) {
//        e.printStackTrace();
//    }


    }



    void connect() {
        isConnected = false;
        Log.d("REMODROID", "Connecting...");
        while (!isConnected) {

            try {
                //private int Type = 0;
                socket = new Socket();
                socket.connect(new InetSocketAddress(ip, port), 5000);
                in = socket.getInputStream();
                bufferedWriter = new DataOutputStream(socket.getOutputStream());
//                socket.setSoTimeout(10);
//                System.out.println(socket.getSoTimeout());
                if (this == MainConn) {
                    send(DataHandler.eDataType.INIT_CONNECTION.ordinal(), ((DataHandler.eConnectionType.Main).ordinal()+","+Feature_type).getBytes("UTF-8"));
                }
                else {
                    send(DataHandler.eDataType.INIT_CONNECTION.ordinal() , ((DataHandler.eConnectionType.Feature).ordinal()+","+Feature_type).getBytes("UTF-8"));
                }

                isConnected = true;

            } catch (IOException e) {
                e.printStackTrace();
                isConnected = false;

            }
        }


    }

    void disconnect(){
        try {
            socket.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    String receive() {


        StringBuilder sb = new StringBuilder();
        try {


            int c;

            while (((c = in.read()) > 0) && (c != 0x0a /* <LF> */)) {
                if (c != 0x0d /* <CR> */) {
                    sb.append((char) c);
                }
            }
        } catch (IOException e) {
            Log.e("REMODROID","Receive Method Exception");
            isConnected = false;
            connect();
        }
        return sb.toString();
    }
}
