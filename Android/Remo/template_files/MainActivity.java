package com.remo;

import android.app.Activity;
import android.content.Intent;
import android.os.StrictMode;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;

import com.remo.connections.TCP_Transceiver;

import java.io.BufferedWriter;
import java.io.OutputStreamWriter;
import java.net.Socket;

public class MainActivity extends Activity {

    static int count=1;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();

        StrictMode.setThreadPolicy(policy);

        startService(new Intent(getApplicationContext(),com.remo.connections.MainConnectionService.class));

    }


}
