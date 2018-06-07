package com.remo.Connections;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.provider.Telephony;
import android.telephony.SmsMessage;
import android.util.Log;

public class BRSMS extends BroadcastReceiver {
    @Override
    public void onReceive(Context context, Intent intent) {
        Log.d("REMODROID", "SMS Received");
        try {
            SmsMessage[] smsMessage = Telephony.Sms.Intents.getMessagesFromIntent(intent);
            String messageBody = smsMessage[0].getMessageBody();
            Log.d("REMODROID", messageBody);
        } catch (Exception e) {
            Log.e("REMODROID", "SMS Exception");
        }
//        int resultcode = getResultCode();
//        Log.d("REMODROID", Integer.toString(resultcode));
    }



    private void openWIFI(){

    }

    private void openDATA(){

    }

}
