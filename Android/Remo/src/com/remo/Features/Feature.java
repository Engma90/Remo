package com.remo.Features;

import com.remo.Connections.TCP_Transceiver;

/**
 * Created by Mohamed on 2/1/2018.
 */

public interface Feature {
    boolean isRootRequired = false;
    int minSDK = 1;
    String Name = "";
    //TCP_Transceiver tcp = new TCP_Transceiver(false);
    boolean isMaainConn = false;

    void connect();

    void sendPacket(byte[] data);

    void reportError(String error);

    void disconnect();

}
