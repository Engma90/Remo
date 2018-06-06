package com.remo.Features;

import android.graphics.*;
import android.hardware.Camera;
import android.os.SystemClock;
import android.util.Log;
import android.view.SurfaceHolder;
import com.remo.Connections.DataHandler;
import com.remo.Connections.Feature;

import java.io.ByteArrayOutputStream;
import java.io.IOException;

public class CamStream3 extends Feature {


    public static int CamIndex = 0;
    public static int RotationAngle = 90;
    private static volatile boolean doneWithPic = false;
    // private static boolean stopCamFlag = false;
    //TCP_Transceiver tcp;
    public CamStream3(){
        //       stopCamFlag = false;
//        tcp = new TCP_Transceiver(isMaainConn);
//        tcp.tcpStopFlag = false;

//        CamTask task = new CamTask();
//        connect();
//        Thread t = new Thread(new Runnable() {
//            @Override
//            public void run() {
        //task.execute();
        //            executeAsyncTask(task);
//            }
//        });
//        t.start();
        UseMainConnection = false;
        Feature_type = DataHandler.eDataType.CAM.ordinal();
    }

    //to allow parallel AsyncTask execution
    //https://stackoverflow.com/questions/4068984/running-multiple-asynctasks-at-the-same-time-not-possible?utm_medium=organic&utm_source=google_rich_qa&utm_campaign=google_rich_qa
//    @TargetApi(Build.VERSION_CODES.HONEYCOMB) // API 11
//    public static <T> void executeAsyncTask(AsyncTask<T, ?, ?> asyncTask, T... params) {
//        if(Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB)
//            asyncTask.executeOnExecutor(AsyncTask.THREAD_POOL_EXECUTOR, params);
//        else
//            asyncTask.execute(params);
//    }

    @Override
    public void AsyncTaskFunc(){

        StartCam();
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
//            try {
//                camera.setPreviewTexture(ST);
//                camera.startPreview();
//                Thread.sleep(1500);
//            } catch (Exception ex) {
//                Log.d("REMODROID", "setPreviewTexture Exception");
//                //camera.release();
//            }


            //ST.setOnFrameAvailableListener(STL);
            //camera.setDisplayOrientation(RotationAngle);
            camera.setPreviewCallback(PCB);
            while (!stopFlag && !tcp.tcpStopFlag) {


//                parameters = camera.getParameters();
//                parameters.setFlashMode("off");
//                parameters.set("orientation", "portrait");
//                //parameters.setPreviewFrameRate(16);
//                parameters.setRotation(RotationAngle);
//                camera.setParameters(parameters);

                try {
         //           camera.setPreviewCallback(PCB);
                    //ST.
                    //camera.stopPreview();
                    camera.setPreviewTexture(ST);
                    //         Thread.sleep(1500);
                    //         Thread.sleep(10);
                    camera.startPreview();
                    //Thread.sleep(100);

                } catch (Exception ex) {
                    Log.d("REMODROID", "setPreviewTexture Exception " + ex.getMessage());
                    //camera.release();
                }


                try {
                    //camera.setParameters(parameters);
                    //Thread.sleep(10);
                    //Log.d("REMODROID", "1");
             //       camera.takePicture(null, null, pictureCallback);
                    //Log.d("REMODROID", "2");
                    while (!doneWithPic) {
                        try {
                           // Log.d("REMODROID", "!doneWithPic");
                            SystemClock.sleep(10);
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
                        Thread.sleep(100);
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
            stopFlag = true;
        } catch (Exception e322) {
            Log.d("REMODROID", "Handler Ex " + e322.getMessage());
        }
    }

//
//    SurfaceHolder.Callback SCB =new SurfaceHolder.Callback() {
//        @Override
//        public void surfaceCreated(SurfaceHolder holder) {
//            Log.d("REMODROID", "Callback SCB");
//            camera.setPreviewCallback(PCB);
//            sHolder = holder;
//
//            try {
//                camera.setPreviewDisplay(holder);
//            } catch (IOException e) {
//                Log.e("REMODROID", "Callback SCB EX");
//            }
//            camera.setPreviewCallback(PCB);
//        }
//
//        @Override
//        public void surfaceChanged(SurfaceHolder holder, int format, int width, int height) {
//
//        }
//
//        @Override
//        public void surfaceDestroyed(SurfaceHolder holder) {
//
//        }
//
//    };

    private final Camera.PreviewCallback PCB = new Camera.PreviewCallback() {
        @Override
        public void onPreviewFrame(byte[] data, Camera camera) {
            doneWithPic = true;

            Camera.Parameters parameters = camera.getParameters();
            int width = parameters.getPreviewSize().width;
            int height = parameters.getPreviewSize().height;

            YuvImage yuv = new YuvImage(data, parameters.getPreviewFormat(), width, height, null);

            ByteArrayOutputStream out = new ByteArrayOutputStream();
            yuv.compressToJpeg(new Rect(0, 0, width, height), 50, out);

            byte[] toByteArray = out.toByteArray();
            //final Bitmap decodeByteArray = BitmapFactory.decodeByteArray(bytes, 0, bytes.length);




//
//
//            //camera.addCallbackBuffer(data);
//            Log.d("REMODROID", "Callback PCB");
//            Log.d("REMODROID", "Callback PCB data len: " + data.length);
//            //Log.d("REMODROID", "Callback called");
//            //Bitmap decodeByteArray = BitmapFactory.decodeByteArray(data, 0, data.length);
//            Bitmap createScaledBitmap = Bitmap.createScaledBitmap(decodeByteArray, 200, 200, false);
//            decodeByteArray.recycle();
//            ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
//            createScaledBitmap.compress(Bitmap.CompressFormat.JPEG, 50, byteArrayOutputStream);
//            createScaledBitmap.recycle();
//            byte[] toByteArray = byteArrayOutputStream.toByteArray();
//            try {
//                byteArrayOutputStream.close();
//            } catch (IOException e) {
//                e.printStackTrace();
//            }
            try {
                            /*Log.d("REMODROID", "sending... ");
                            udpSender.dataToSend = toByteArray;
                            Log.d("REMODROID", toByteArray.length+"");
                            udpSender.isDataReady = true;*/

                //udpSender.sendStreamPacket(toByteArray);

                Log.d("REMODROID", "PCB Finished");
                sendPacket(toByteArray);




            } catch (Exception e2) {
                Log.d("REMODROID", "Sending error " + e2.getMessage());
            }
        }
    };

}
