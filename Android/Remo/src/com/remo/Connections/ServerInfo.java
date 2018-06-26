package com.remo.connections;

import com.remo.App;
import com.remo.R;

/**
 * Created by Mohamed on 3/3/2018.
 */

class ServerInfo {

    private String ip;
    private int port;

    void init() {
        this.ip = App.get().getApplicationContext().getString(R.string.IP);//"192.168.43.185";//Resources.getSystem().getString(R.string.IP);
        this.port = Integer.parseInt(App.get().getApplicationContext().getString(R.string.Port));//4447;//Integer.parseInt(Resources.getSystem().getString(R.string.Port));
    }

    String getIp() {
        return this.ip;
    }

    int getPort() {
        return this.port;
    }
}
