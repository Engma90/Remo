package com.remo.connections;

import android.util.Log;

import java.io.*;
import java.net.InetSocketAddress;
import java.net.Socket;

/**
 * Created by Mohamed on 3/2/2018.
 */

public class TCP_Transceiver {

    private int port;
    private String ip;
    public boolean isConnected = false;
    private BufferedReader br;
    private DataOutputStream bufferedWriter;
    private static TCP_Transceiver MainConn;
    public boolean tcpStopFlag = false;
    private Socket socket;
    public int Feature_type;


    //public static final int DATA_TYPE_INFO = 0;
    //public static final int DATA_TYPE_SMS = 1;

    public TCP_Transceiver(boolean isMainConn) {
        ServerInfo si = new ServerInfo();
        si.init();
        this.ip = si.getIp();
        this.port = si.getPort();
        if (isMainConn) {
            MainConn = this;
            Feature_type = -1;
        }
    }


    void send(int DATA_TYPE,int flag, byte[] data) {

        try {
            Log.d("REMODROID", "Sending Data of type: " + DATA_TYPE +":"+flag);
            bufferedWriter.writeInt(data.length);//Max Size 2147483647 = 2 GiB
            bufferedWriter.writeInt(DATA_TYPE);
            bufferedWriter.writeInt(flag);
            bufferedWriter.write(data);
            bufferedWriter.flush();
        } catch (Exception e) {
            Log.d("REMODROID", "Sending Exception " + e.getMessage());
            tcpStopFlag = true;
        }

    }



    public void connect() {
        isConnected = false;
        Log.d("REMODROID", "Connecting...");
        while (!isConnected) {

            try {
//                try {
//                    socket.shutdownInput();
//                    socket.shutdownOutput();
//                    socket.close();
//                }catch (Exception e){
//                    Log.e("REMODROID", "socket.close() Exception");
//                }
                socket = new Socket();
                socket.connect(new InetSocketAddress(ip, port), 5000);
                InputStream in = socket.getInputStream();
                bufferedWriter = new DataOutputStream(socket.getOutputStream());
                br = new BufferedReader(new InputStreamReader(in, "UTF-8"));
                if (this == MainConn) {
                    send(DataHandler.eDataType.INIT_CONNECTION.ordinal(),(DataHandler.eConnectionType.Main).ordinal(), "".getBytes("UTF-8"));
                }
                else {
                    send(DataHandler.eDataType.INIT_CONNECTION.ordinal() ,(DataHandler.eConnectionType.Feature).ordinal(),(""+Feature_type).getBytes("UTF-8"));
                }

                isConnected = true;

            } catch (IOException e) {
                //Log.e("REMODROID", "Connect Exception");
                isConnected = false;

            }
        }


    }

    public void disconnect(){
        try {
            tcpStopFlag= true;
            socket.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public String receive() throws IOException {
        return br.readLine();
    }
}
