package com.remo.Features;

import android.media.AudioFormat;
import android.media.AudioRecord;
import android.media.MediaRecorder;
import android.util.Log;
import com.remo.Connections.DataHandler;
import com.remo.Connections.Feature;

public class MicStream extends Feature {

    private AudioRecord audiorecord;
    //private static boolean stopMicFlag = false;
    private static int SAMPLER = 8000;//44100; //Sample Audio Rate
    private int channelConfig = AudioFormat.CHANNEL_IN_MONO;
    private int audioFormat = AudioFormat.ENCODING_PCM_16BIT;
    //private int minBufSize = AudioRecord.getMinBufferSize(SAMPLER, channelConfig, audioFormat);
    private int minBufSize = 8192;
    //TCP_Transceiver tcp;

    public MicStream(){

        UseMainConnection = false;
        Feature_type = DataHandler.eDataType.MIC.ordinal();
        audiorecord = new AudioRecord(MediaRecorder.AudioSource.MIC, SAMPLER, channelConfig, audioFormat, minBufSize);
//        tcp = new TCP_Transceiver(isMaainConn);
//        tcp.tcpStopFlag = false;
//        stopMicFlag = false;
//        AudioTask task = new AudioTask();
//        connect();
//        //task.execute();
//        executeAsyncTask(task);
    }

//    @TargetApi(Build.VERSION_CODES.HONEYCOMB) // API 11
//    public static <T> void executeAsyncTask(AsyncTask<T, ?, ?> asyncTask, T... params) {
//        if(Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB)
//            asyncTask.executeOnExecutor(AsyncTask.THREAD_POOL_EXECUTOR, params);
//        else
//            asyncTask.execute(params);
//    }

//    public static void stopStream() {
//
//        //audiorecord.stop();
//        //tcp.tcpStopFlag = true;
//        stopMicFlag = true;
//        //audiorecord.release();
//        Log.e("REMODROID", "Stopped");
//    }


//    private class AudioTask extends AsyncTask<String, Void, String> {
//
//        @Override
//        protected String doInBackground(String... params) {
//            try {
//                //minBufSize = 640;
//                byte[] buffer = new byte[minBufSize];
//                audiorecord.startRecording();
//
//                while (!tcp.tcpStopFlag && !stopMicFlag) {
//                    audiorecord.read(buffer, 0, minBufSize);
//                    sendPacket(buffer);
//                }
//                audiorecord.stop();
//                audiorecord.release();
//                Log.e("Socket Closed", "");
//            } catch (Exception e) {
//                Log.e("Exception: ", e.toString());
//                stopStream();
//            }
//            return null;
//        }
//    }


//    @Override
//    public void connect() {
//        tcp.Feature_type = DataHandler.eDataType.DATA_TYPE_MIC_START.ordinal();
//        tcp.connect();
//    }

//    @Override
//    public void sendPacket(byte[] data) {
//        tcp.send(DataHandler.eDataType.DATA_TYPE_MIC_START.ordinal(), data);
//    }

    @Override
    public void AsyncTaskFunc() {
        try {
            //minBufSize = 640;
            byte[] buffer = new byte[minBufSize];
            audiorecord.startRecording();

            while (!tcp.tcpStopFlag && !stopFlag) {
                audiorecord.read(buffer, 0, minBufSize);
                sendPacket(buffer);
            }
            audiorecord.stop();
            audiorecord.release();
            Log.e("Socket Closed", "");
        } catch (Exception e) {
            Log.e("Exception: ", e.toString());
            stopStream();
        }
    }

//    @Override
//    public void reportError(String error) {
//
//    }

}
