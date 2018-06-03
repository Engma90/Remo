package com.remo.Features;

import com.remo.Connections.TCP_Transceiver;

/**
 * Created by Mohamed on 2/1/2018.
 */

public abstract class Feature {
    boolean isRootRequired = false;
    int minSDK = 1;
    String Name = "";
    //TCP_Transceiver tcp = new TCP_Transceiver(false);
    boolean isMaainConn = false;

    abstract void connect();

    abstract void sendPacket(byte[] data);

    abstract void reportError(String error);

    abstract void disconnect();

}
