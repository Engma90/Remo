package com.remo.Features;

import android.util.Log;
import com.remo.Connections.DataHandler;
import com.remo.Connections.Feature;

import java.io.File;
import java.io.UnsupportedEncodingException;
import java.nio.charset.Charset;

public class FileMan extends Feature {
    private static final char NamesSplitChar = '/'; //names
    private static final char TypeSplitChar = '\\'; // dir or file
    private String path;
    public FileMan(String params){
        this.path = params;
        UseMainConnection = false;
        Feature_type = DataHandler.eDataType.FM_LIST.ordinal();

    }

    @Override
    public void AsyncTaskFunc() {

        //Log.d("REMODROID",getList(path));
        try {
            String toSend = getList(path);

            sendPacket(toSend.getBytes("UTF-8"));
            Log.d("REMODROID",toSend);
        } catch (UnsupportedEncodingException e) {
            e.printStackTrace();
        }
    }

    private String getList(String dir){
        StringBuilder sb = new StringBuilder();
        switch (dir) {
            case "/":
                sb.append("Internal");
                sb.append(NamesSplitChar);
                if (isSDMounted()) {
                    sb.append("SDCard");
                }
                sb.append(TypeSplitChar);
                return sb.toString();
            case "Internal":

                sb.append(getDirs(System.getenv("EXTERNAL_STORAGE")));
                sb.append(TypeSplitChar);
                sb.append(getFiles(System.getenv("EXTERNAL_STORAGE")));

                break;
            case "SDCard":
//                sb.append(getDirs(android.os.Environment.getExternalStorageDirectory().getAbsolutePath()));
//                sb.append(TypeSplitChar);
//                sb.append(getFiles(android.os.Environment.getExternalStorageDirectory().getAbsolutePath()));
                sb.append(getDirs(System.getenv("SECONDARY_STORAGE")));
                sb.append(TypeSplitChar);
                sb.append(getFiles(System.getenv("SECONDARY_STORAGE")));
                break;
            default:
                sb.append(getDirs(dir));
                sb.append(TypeSplitChar);
                sb.append(getFiles(dir));
                break;
        }
        return sb.toString();
    }

    private String getDirs(String dir){
        StringBuilder sb = new StringBuilder();
        File f = new File(dir);
        if( f.exists() ) {
            File[] files = f.listFiles();
            for(int i=0; i<files.length; i++) {
                if(files[i].isDirectory()) {
                    sb.append(files[i].getName());
                    if(i < files.length - 1)
                        sb.append(NamesSplitChar);
                }
            }
        }
        return sb.toString();
    }
    private String getFiles(String dir){
        StringBuilder sb = new StringBuilder();
        File f = new File(dir);
        if( f.exists() ) {
            File[] files = f.listFiles();
            for(int i=0; i<files.length; i++) {
                if(files[i].isFile()) {
                    sb.append(files[i].getName());
                    if(i < files.length - 1)
                        sb.append(NamesSplitChar);
                }
            }
        }
        return sb.toString();
    }

    private boolean isSDMounted(){
       return android.os.Environment.getExternalStorageState().equals(android.os.Environment.MEDIA_MOUNTED);
    }


}
