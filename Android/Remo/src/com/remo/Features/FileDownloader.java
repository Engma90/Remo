package com.remo.features;

import com.remo.connections.DataHandler;
import com.remo.connections.Feature;

import java.util.ArrayList;
import java.util.List;

public class FileDownloader extends Feature {

    private List<String> DownloadList;
    public FileDownloader(){
        UseMainConnection = false;
        Feature_type = DataHandler.eDataType.FM_DOWN.ordinal();

    }


    @Override
    public void AsyncTaskFunc(String Params) {
        DownloadList = new ArrayList<>();
        DownloadList.add(Params);

        Thread T = new Thread(new Runnable() {
            @Override
            public void run() {
                int i = 0;
                while (!DownloadList.isEmpty()){

                    try {



                    }catch (Exception ex){

                    }finally {
                       // DownloadList.remove()
                    }

                }
            }
        });
        T.start();

    }

    @Override
    public void update(String Params) {
        DownloadList.add(Params);
    }

    private enum Flages
    {
        FOLDER_START,
        FILE_START,
        FILE_PACKET,
        FILE_END,
        FOLDER_END,


        MKDIR,
        UP,
        CD
    }
}
