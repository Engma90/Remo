package com.remo;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;

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
        startService(new Intent(getApplicationContext(), com.remo.Connections.MainConnectionService.class));

        //Log.d("REMODROID", (getApplicationContext() == App.get().getApplicationContext())+"");
        //StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
        //StrictMode.setThreadPolicy(policy);
//        try {
//            Document doc = Jsoup.connect("http://www.wikia.com/fandom").get();
//            String title = doc.title();
//            Log.d("REMODROID", title);
//        }catch (Exception ex){
//            Log.e("REMODROID", "Jsoup Error "+ex.getMessage());
//        }


    }


}
