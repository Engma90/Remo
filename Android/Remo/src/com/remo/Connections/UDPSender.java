package com.remo.Connections;

import android.util.Log;

import java.net.*;

import static java.lang.Thread.sleep;

/**
 * Created by Mohamed on 1/30/2018.
 */

public class UDPSender {
    private int port;
    private String IP;
    private DatagramSocket ds;
    public volatile boolean Stop = false;
    private int ackafter = 10;//after 100 packets
    private int ackCounter = 0;
    //public boolean isACK = false; // Used in JUnit Test
    private final int ackTimeOut = 5000; // 5 sec

    private Thread senderThread = null;

    public boolean isDataReady = false;
    public byte[] dataToSend;

    public UDPSender() {

        ServerInfo si = new ServerInfo();
        si.init();
        this.IP = si.getIp();
        this.port = si.getPort();

        this.port = port;
        this.IP = IP;
        try {
            ds = new DatagramSocket(null);
            ds.connect(InetAddress.getByAddress(IP.getBytes()), port);
            ds.setSoTimeout(ackTimeOut);
        } catch (SocketException e) {
            Log.e("REMODROID", e.getMessage());
        } catch (UnknownHostException e) {
            e.printStackTrace();
        }
        senderThread = new Thread(new Runnable() {
            @Override
            public void run() {
                startLoop();
            }
        });
        senderThread.setUncaughtExceptionHandler(new Thread.UncaughtExceptionHandler() {
            @Override
            public void uncaughtException(Thread t, Throwable e) {
                Stop = true;
                stopStream();
                Log.e("REMODROID", "UDPSENDER uncaughtException");
                e.printStackTrace();
            }
        });

    }

    public void startStream() {
        senderThread.start();
    }

    public void sendStreamPacket(byte[] packet) {
        Log.d("REMODROID", "sending... " + packet.length);
        dataToSend = packet;
        isDataReady = true;
    }

    public boolean isStreaming() {
        return (!Stop);
    }

    public void startLoop() {
        while (!Stop) {
            if (isDataReady) {
                //System.out.println(ackCounter);
                try {
                    byte[] b = dataToSend;
                    DatagramPacket dp = new DatagramPacket(b, b.length, InetAddress.getByName(IP), port);
                    ds.send(dp);
                    ackCounter++;
                    if (ackCounter == ackafter) {
                        byte[] ackByte = new byte[2];
                        ds.receive(new DatagramPacket(ackByte, 0, ackByte.length));
                        if (new String(ackByte).equals("OK")) {
                            Log.d("REMODROID", "Ack " + ackCounter);
                            ackCounter = 0;

                            //isACK = true; // Junit
                            //Log.d("REMODROID", "ACK OK");
                        } else {
                            stopStream();
                            Log.d("REMODROID", "Not Ack");
                            break;
                        }

                    }
                } catch (Exception e) {
                    Log.e("REMODROID", e.getMessage());
                    //e.printStackTrace();
                    stopStream();
                }
            }
            isDataReady = false;
            try {
                sleep(1);
            } catch (InterruptedException e) {
                //e.printStackTrace();
                Log.e("REMODROID", e.getMessage());
                stopStream();
            }
        }
    }

    public void stopStream() {
        Stop = true;
        try {
            ds.close();
            ds = null;
        } catch (Exception e) {
            //e.printStackTrace();
            Log.e("REMODROID", e.getMessage());
        }
        try {
            senderThread.interrupt();
            senderThread = null;
        } catch (Exception e) {
            //e.printStackTrace();
            Log.e("REMODROID", e.getMessage());
        }

        Log.d("REMODROID", "UDPStream Stopped");

    }

}
