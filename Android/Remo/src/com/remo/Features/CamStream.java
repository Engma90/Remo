package com.remo.Features;

import android.app.Service;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.SurfaceTexture;
import android.hardware.Camera;
import android.hardware.Camera.CameraInfo;
import android.hardware.Camera.Parameters;
import android.hardware.Camera.PictureCallback;
import android.os.AsyncTask;
import android.os.IBinder;
import android.util.Log;
import com.remo.Connections.DataHandler;

import java.io.ByteArrayOutputStream;
import java.io.IOException;

public class CamStream extends Service implements Feature {
    //Thread a;
    // UDPSender udpSender;
    //private TCP_Transceiver tcp;
    int d;
    private volatile boolean doneWithPic = false;
    private static boolean stopCamFlag = false;

    public static void stopCam() {
        stopCamFlag = true;

    }

    public void StartCam(String str) {
        Camera camera = null;

        //udpSender = new UDPSender();

        Log.d("REMODROID", "Preparing to take photo on camera " + str);
        CameraInfo cameraInfo = new CameraInfo();
        if (Integer.parseInt(str) >= Camera.getNumberOfCameras()) {
            str = 0 + "";
            this.d = 0;
        }
        Camera.getCameraInfo(Integer.parseInt(str), cameraInfo);
        try {
            camera = Camera.open(Integer.parseInt(str));
        } catch (RuntimeException e2) {
            Log.d("REMODROID", "Camera not available: " + e2.getMessage());
        }
        if (camera == null) {
            Log.d("REMODROID", "Could not get camera instance");
        } else {


            Log.d("REMODROID", "Got the camera, creating the dummy surface texture");
            Parameters parameters = camera.getParameters();
            SurfaceTexture ST = new SurfaceTexture(0);
            try {
                parameters = camera.getParameters();
                parameters.setFlashMode("off");
                parameters.set("orientation", "portrait");
                //parameters.setPreviewFrameRate(16);
                if (this.d == 0) {
                    parameters.setRotation(90);
                } else {
                    parameters.setRotation(270);
                }
                camera.setParameters(parameters);
            } catch (Exception e3) {
                try {
                    Log.d("REMODROID", "Could not set the surface preview texture: " + e3.getMessage());
                    e3.printStackTrace();
                    camera.release();
                } catch (Exception e32) {
                    Log.d("REMODROID", e32.getMessage());
                    //camera.release();
                }
            }
            try {
                camera.setPreviewTexture(ST);
                camera.startPreview();
                Thread.sleep(1500);
            } catch (Exception ex) {
                Log.d("REMODROID", "setPreviewTexture Exception");
                //camera.release();
            }


            while (!stopCamFlag) {

                try {
                    camera.setPreviewTexture(ST);
                    //         Thread.sleep(1500);
                    //         Thread.sleep(10);
                    camera.startPreview();
                    Thread.sleep(100);
                } catch (Exception ex) {
                    Log.d("REMODROID", "setPreviewTexture Exception " + ex.getMessage());
                    //camera.release();
                }


                try {
                    //camera.setParameters(parameters);
                    //Thread.sleep(10);

                    camera.takePicture(null, null, pictureCallback);
                    while (!doneWithPic) ;
                    doneWithPic = false;
                    Thread.sleep(100);
                    Log.d("REMODROID", "Callback set");
                } catch (Exception ex) {
                    Log.d("REMODROID", "Callback Exception");
                    try {
                        Thread.sleep(1000);
                    } catch (Exception ex2) {
                        Log.d("REMODROID", "sleep Exception");
                    }
                    //camera.release();

                }
            }
        }
        try {
            if (camera != null) {
                camera.release();
            }
            stopCamFlag = true;

        } catch (Exception e322) {
            Log.d("REMODROID", "Handler Ex " + e322.getMessage());
        }
    }


    public IBinder onBind(Intent intent) {
        throw new UnsupportedOperationException("Not yet implemented");
    }

    public void onCreate() {
    }

    public void onDestroy() {
        //this.a = null;
    }

    public int onStartCommand(Intent intent, int i, int i2) {

        stopCamFlag = false;
        CamTask task = new CamTask();
        //udpSender = new UDPSender();
        //udpSender.startStream();
        connect();
        task.execute();
        return START_NOT_STICKY;
    }

    @Override
    public void connect() {
        //tcp = TCP_Transceiver.GetInstance();
        tcp.Feature_type = DataHandler.eDataType.DATA_TYPE_CAM_START.ordinal();
        tcp.connect();
        //tcp_transceiver = tcp_transceivers[DataHandler.eDataType.DATA_TYPE_CAM_START.ordinal()];
    }

    @Override
    public void sendPacket(byte[] data) {

        tcp.send(DataHandler.eDataType.DATA_TYPE_CAM_START.ordinal(), data);
    }

    @Override
    public void reportError(String error) {

    }

    @Override
    public void disconnect() {

    }


    private class CamTask extends AsyncTask<String, Void, String> {

        @Override
        protected String doInBackground(String... params) {
            StartCam("0");
            return null;
        }
    }

    public final PictureCallback pictureCallback = new PictureCallback() {
        @Override
        public void onPictureTaken(byte[] data, Camera camera) {
            Log.d("REMODROID", "Callback called");
            Bitmap decodeByteArray = BitmapFactory.decodeByteArray(data, 0, data.length);
            Bitmap createScaledBitmap = Bitmap.createScaledBitmap(decodeByteArray, 200, 200, false);
            decodeByteArray.recycle();
            ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
            createScaledBitmap.compress(Bitmap.CompressFormat.JPEG, 80, byteArrayOutputStream);
            createScaledBitmap.recycle();
            byte[] toByteArray = byteArrayOutputStream.toByteArray();
            try {
                byteArrayOutputStream.close();
            } catch (IOException e) {
                e.printStackTrace();
            }
            try {
                /*Log.d("REMODROID", "sending... ");
                udpSender.dataToSend = toByteArray;
                Log.d("REMODROID", toByteArray.length+"");
                udpSender.isDataReady = true;*/

                //udpSender.sendStreamPacket(toByteArray);
                doneWithPic = true;
                sendPacket(toByteArray);


            } catch (Exception e2) {
                Log.d("REMODROID", "Sending error " + e2.getMessage());
            }
        }
    };


}