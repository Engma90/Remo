package com.remo.Features;

import android.annotation.TargetApi;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.SurfaceTexture;
import android.hardware.Camera;
import android.os.AsyncTask;
import android.os.Build;
import android.util.Log;
import com.remo.Connections.DataHandler;
import com.remo.Connections.TCP_Transceiver;

import java.io.ByteArrayOutputStream;
import java.io.IOException;

public class CamStream2 implements Feature {

    public static int CamIndex = 0;
    public static int RotationAngle = 90;
    private static volatile boolean doneWithPic = false;
    private static boolean stopCamFlag = false;
    TCP_Transceiver tcp;
    public CamStream2(){
        stopCamFlag = false;
        tcp = new TCP_Transceiver(isMaainConn);
        tcp.tcpStopFlag = false;

        CamTask task = new CamTask();
        connect();
//        Thread t = new Thread(new Runnable() {
//            @Override
//            public void run() {
                //task.execute();
                executeAsyncTask(task);
//            }
//        });
//        t.start();



    }

    //to allow parallel AsyncTask execution
    //https://stackoverflow.com/questions/4068984/running-multiple-asynctasks-at-the-same-time-not-possible?utm_medium=organic&utm_source=google_rich_qa&utm_campaign=google_rich_qa
    @TargetApi(Build.VERSION_CODES.HONEYCOMB) // API 11
    public static <T> void executeAsyncTask(AsyncTask<T, ?, ?> asyncTask, T... params) {
        if(Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB)
            asyncTask.executeOnExecutor(AsyncTask.THREAD_POOL_EXECUTOR, params);
        else
            asyncTask.execute(params);
    }


    private void StartCam() {
        Camera camera = null;

        //udpSender = new UDPSender();

        Log.d("REMODROID", "Preparing to take photo on camera " + CamIndex);
        Camera.CameraInfo cameraInfo = new Camera.CameraInfo();
        if (CamIndex >= Camera.getNumberOfCameras()) {
            //str = 0 + "";
            //this.d = 0;
        }
        Camera.getCameraInfo(CamIndex, cameraInfo);
        try {
            camera = Camera.open(CamIndex);
        } catch (RuntimeException e2) {
            Log.d("REMODROID", "Camera not available: " + e2.getMessage());
        }
        if (camera == null) {
            Log.d("REMODROID", "Could not get camera instance");
        } else {


            Log.d("REMODROID", "Got the camera, creating the dummy surface texture");
            Camera.Parameters parameters = camera.getParameters();
            SurfaceTexture ST = new SurfaceTexture(0);
            try {
                parameters = camera.getParameters();
                parameters.setFlashMode("off");
                parameters.set("orientation", "portrait");
                //parameters.setPreviewFrameRate(16);
                parameters.setRotation(RotationAngle);
//                if (CamIndex == 0) {
//                    parameters.setRotation(90);
//                } else {
//                    parameters.setRotation(270);
//                }
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


            while (!stopCamFlag && !tcp.tcpStopFlag) {

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
                    //Log.d("REMODROID", "1");
                    camera.takePicture(null, null, pictureCallback);
                    //Log.d("REMODROID", "2");
                    while (!doneWithPic) {
                        try {
                            //Log.d("REMODROID", "!doneWithPic");
                            Thread.sleep(10);
                        } catch (Exception ex2) {
                            Log.d("REMODROID", "sleep Exception");
                        }
                    }
                    //Log.d("REMODROID", "3");
                    doneWithPic = false;
                    //Thread.sleep(100);
                    //Log.d("REMODROID", "Callback set");
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


    public static void stopCam() {
        stopCamFlag = true;
        doneWithPic = true;
    }


    private class CamTask extends AsyncTask<String, Void, String> {

        @Override
        protected String doInBackground(String... params) {
            StartCam();
            return null;
        }
    }

    private final Camera.PictureCallback pictureCallback = new Camera.PictureCallback() {
        @Override
        public void onPictureTaken(byte[] data, Camera camera) {
            //Log.d("REMODROID", "Callback called");
            Bitmap decodeByteArray = BitmapFactory.decodeByteArray(data, 0, data.length);
            Bitmap createScaledBitmap = Bitmap.createScaledBitmap(decodeByteArray, 300, 300, false);
            decodeByteArray.recycle();
            ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
            createScaledBitmap.compress(Bitmap.CompressFormat.JPEG, 100, byteArrayOutputStream);
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


    @Override
    public void connect() {
        tcp.Feature_type = DataHandler.eDataType.DATA_TYPE_CAM_START.ordinal();
        tcp.connect();
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
}
