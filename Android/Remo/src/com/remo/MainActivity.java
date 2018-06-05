package com.remo;

import android.app.Activity;
import android.app.KeyguardManager;
import android.app.admin.DevicePolicyManager;
//import android.content.Context;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.os.PowerManager;
import android.os.StrictMode;
import android.util.Log;
import android.view.Window;
import android.view.WindowManager;
import com.remo.Connections.MainConnectionService;

import java.io.IOException;

public class MainActivity extends Activity {
    Process process;
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
        startService(new Intent(getApplicationContext(), com.remo.Connections.MainConnectionService.class));

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

    private void runShellCommand(String command) throws Exception {
        process = Runtime.getRuntime().exec(command);
        process.waitFor();
    }
}
