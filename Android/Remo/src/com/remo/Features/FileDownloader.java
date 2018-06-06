package com.remo.Features;

import com.remo.Connections.DataHandler;
import com.remo.Connections.Feature;

public class FileDownloader extends Feature {

    public FileDownloader(){
        UseMainConnection = false;
        Feature_type = DataHandler.eDataType.FM_DOWN.ordinal();
    }


    @Override
    public void AsyncTaskFunc() {

    }
}
