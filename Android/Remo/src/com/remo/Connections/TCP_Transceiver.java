package com.remo.Connections;

import android.os.SystemClock;
import android.util.Log;
import com.remo.App;

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
    public static TCP_Transceiver MainConn;
    public boolean stop = false;
    private boolean isMainConn;
    //private Context context;

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
        this.isMainConn = isMainConn;
        //this.context = App.get().getApplicationContext();
        if (isMainConn)
            MainConn = this;
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

    public boolean send(int DATA_TYPE, byte[] data) {

//        try {

        try {
            SystemClock.sleep(100);
            //Log.d("REMODROID", "Sending Message of Length: " + (int)(4 + 4 + data.length));
            bufferedWriter.writeInt((int) (4 + data.length));//Max Size 2147483647 = 2 GiB
            SystemClock.sleep(10);
            bufferedWriter.writeInt(DATA_TYPE);
            SystemClock.sleep(10);
            bufferedWriter.write(data);
            SystemClock.sleep(10);
            bufferedWriter.flush();
            //socket.close();
        } catch (Exception e) {
            e.printStackTrace();
            stop = true;
            return false;
        }
//    } catch (Exception e) {
//        e.printStackTrace();
//    }


        return true;
    }

    public void connect() {
        isConnected = false;
        Log.d("REMODROID", "Connecting...");
        while (!isConnected) {

            try {
                //private int Type = 0;
                Socket socket = new Socket();
                socket.connect(new InetSocketAddress(ip, port), 5000);
                in = socket.getInputStream();
                bufferedWriter = new DataOutputStream(socket.getOutputStream());
//                socket.setSoTimeout(10);
//                System.out.println(socket.getSoTimeout());
                if (this == MainConn) {
                    DataHandler.distribute(DataHandler.eDataType.DATA_TYPE_INFO.ordinal(), App.get().getApplicationContext());
                }

                isConnected = true;

            } catch (IOException e) {
                e.printStackTrace();
                isConnected = false;

            }
        }


    }

    String receive() throws IOException {
        String MessageFromServer;
//            DataInputStream in = new DataInputStream(new BufferedInputStream(socket.getInputStream()));
//            MessageFromServer = in.readUTF();

        StringBuilder sb = new StringBuilder();
        try {


            int c;

            while (((c = in.read()) > 0) && (c != 0x0a /* <LF> */)) {
                if (c != 0x0d /* <CR> */) {
                    sb.append((char) c);
                } else {
                    // Ignore <CR>.
                }
            }
        } catch (IOException e) {
            e.printStackTrace();
            isConnected = false;
            connect();
        }
        return sb.toString();
    }
}
