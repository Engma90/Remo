package com.remo.features.Root;

import android.content.Context;
import android.net.ConnectivityManager;
import android.util.Log;

import java.lang.reflect.Field;
import java.lang.reflect.Method;

public class MobileDataSwitch {

    boolean DataOnOff(boolean status, Context context) {
        int bv = 0;
        try {
            final ConnectivityManager conman = (ConnectivityManager) context
                    .getSystemService(Context.CONNECTIVITY_SERVICE);
            final Class<?> conmanClass = Class.forName(conman.getClass()
                    .getName());
            final Field iConnectivityManagerField = conmanClass
                    .getDeclaredField("mService");
            iConnectivityManagerField.setAccessible(true);
            final Object iConnectivityManager = iConnectivityManagerField
                    .get(conman);
            final Class<?> iConnectivityManagerClass = Class
                    .forName(iConnectivityManager.getClass().getName());
            final Method setMobileDataEnabledMethod = iConnectivityManagerClass
                    .getDeclaredMethod("setMobileDataEnabled", Boolean.TYPE);
            setMobileDataEnabledMethod.setAccessible(true);
            setMobileDataEnabledMethod.invoke(iConnectivityManager, status);

            return true;

        } catch (Exception e) {
            Log.e("REMODROID", "error turning on/off data");

            return false;
        }

    }
}
