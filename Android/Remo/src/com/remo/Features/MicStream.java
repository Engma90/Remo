package com.remo.Features;

import android.app.Service;
import android.content.Intent;
import android.media.AudioFormat;
import android.media.AudioRecord;
import android.media.MediaRecorder;
import android.os.AsyncTask;
import android.os.IBinder;
import android.util.Log;
import com.remo.Connections.UDPSender;

/**
 * Created by Mohamed on 3/3/2018.
 */

public class MicStream extends Service implements Feature {
    //private TCP_Transceiver tcp;
    private AudioRecord audiorecord;
    private static int SAMPLER = 8000;//44100; //Sample Audio Rate
    private int channelConfig = AudioFormat.CHANNEL_IN_MONO;
    private int audioFormat = AudioFormat.ENCODING_PCM_16BIT;
    private int minBufSize = AudioRecord.getMinBufferSize(SAMPLER, channelConfig, audioFormat);

    private UDPSender udpSender;

    public MicStream() {
        //tcp = TCP_Transceiver.GetInstance();
        audiorecord = new AudioRecord(MediaRecorder.AudioSource.MIC, SAMPLER, channelConfig, audioFormat, minBufSize);
    }


    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }

    public int onStartCommand(Intent intent, int i, int i2) {
        startStream();
        return START_NOT_STICKY;
    }

    public void startStream() {
        AudioTask task = new AudioTask();
        udpSender = new UDPSender();
        task.execute();
    }

    public void stopStream() {
        audiorecord.stop();
        udpSender.stopStream();
        audiorecord.release();
        Log.e("Audio Socket Closed", "");
    }

    @Override
    public void connect() {

    }

    @Override
    public void sendPacket(byte[] data) {

    }

    @Override
    public void reportError(String error) {

    }

    @Override
    public void disconnect() {

    }


    private class AudioTask extends AsyncTask<String, Void, String> {

        @Override
        protected String doInBackground(String... params) {
            try {
                //minBufSize = 640;
                byte[] buffer = new byte[minBufSize];
                audiorecord.startRecording();
                udpSender.startStream();

                while (!udpSender.Stop) {
                    audiorecord.read(buffer, 0, minBufSize);
                    udpSender.dataToSend = buffer;
                    udpSender.isDataReady = true;
                    //Log.e("SendingBytes: ", buffer.length + "");
                }
                udpSender.stopStream();
                audiorecord.stop();
                Log.e("Socket Closed", "");
            } catch (Exception e) {
                Log.e("Exception: ", e.toString());
                stopStream();
            }
            return null;
        }
    }
}
