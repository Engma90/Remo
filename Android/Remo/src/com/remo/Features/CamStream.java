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

public class CamStream extends Feature {


    private int Quality = 50;
    private int CamIndex = 0;
    private static volatile boolean doneWithPic = false;
    public CamStream(){
        UseMainConnection = false;
        Feature_type = DataHandler.eDataType.CAM.ordinal();
    }

    @Override
    public void AsyncTaskFunc(String Params){
        Log.d("REMODROID", "Params of camera " + Params);
        this.Quality = Integer.parseInt(Params.split("/")[0]);
        this.CamIndex = Integer.parseInt(Params.split("/")[1]);

        StartCam();
    }

    @Override
    public void update(String Params) {
        this.Quality = Integer.parseInt(Params.split("/")[0]);
    }

    private void StartCam() {

        Camera camera = null;
        //udpSender = new UDPSender();

        Log.d("REMODROID", "Preparing to take photo on camera " + CamIndex);
        Camera.CameraInfo cameraInfo = new Camera.CameraInfo();
        if (CamIndex >= Camera.getNumberOfCameras()) {
            //report error
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

            SurfaceTexture ST = new SurfaceTexture(0);

            try {
                Camera.Parameters parameters = camera.getParameters();
                parameters.setFlashMode("off");
                if (parameters.getSupportedFocusModes().contains(
                        Camera.Parameters.FOCUS_MODE_CONTINUOUS_VIDEO)) {
                    parameters.setFocusMode(Camera.Parameters.FOCUS_MODE_CONTINUOUS_VIDEO);
                }
                else{
                    parameters.setFocusMode(Camera.Parameters.FOCUS_MODE_AUTO);
                }
                camera.setParameters(parameters);



                camera.setPreviewCallback(PCB);
                camera.startPreview();


            } catch (Exception e3) {
 //               try {
                    Log.d("REMODROID", "Could not set the surface preview texture: " + e3.getMessage());
                    camera.release();
//                } catch (Exception e) {
//                    Log.d("REMODROID", e.getMessage());
//                    camera.release();
 //               }
            }


            while (!stopFlag && !tcp.tcpStopFlag) {
                try {
                    camera.setPreviewTexture(ST);
                } catch (Exception ex) {
                    Log.d("REMODROID", "Refresh PreviewTexture Exception " + ex.getMessage());
                    //camera.release();
                }

                    while (!doneWithPic && !stopFlag && !tcp.tcpStopFlag) {
                        try {
                            SystemClock.sleep(10);
                        } catch (Exception ex) {
                            Log.d("REMODROID", "sleep Exception");
                        }
                    }
                    doneWithPic = false;
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


    private final Camera.PreviewCallback PCB = new Camera.PreviewCallback() {
        @Override
        public void onPreviewFrame(byte[] data, Camera camera) {

            try {
            Camera.Parameters parameters = camera.getParameters();
            int width = parameters.getPreviewSize().width;
            int height = parameters.getPreviewSize().height;

            YuvImage yuv = new YuvImage(data, parameters.getPreviewFormat(), width, height, null);

            ByteArrayOutputStream out = new ByteArrayOutputStream();
            yuv.compressToJpeg(new Rect(0, 0, width, height), 100, out);

            byte[] toByteArray1 = out.toByteArray();
            //final Bitmap decodeByteArray = BitmapFactory.decodeByteArray(bytes, 0, bytes.length);



            Bitmap decodeByteArray = BitmapFactory.decodeByteArray(toByteArray1, 0, toByteArray1.length);
            if(Quality == 0)
                Quality = 1;
            Bitmap createScaledBitmap = Bitmap.createScaledBitmap(decodeByteArray, (int) (width  / (2 * 100.0 / Quality)) , (int) (height  / (2 * 100.0 / Quality)), false);
            decodeByteArray.recycle();
            ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
            createScaledBitmap.compress(Bitmap.CompressFormat.JPEG, 100, byteArrayOutputStream);
            createScaledBitmap.recycle();
            byte[] toByteArray = byteArrayOutputStream.toByteArray();
            try {
                byteArrayOutputStream.close();
            } catch (IOException e) {
               // e.printStackTrace();
            }

                            /*Log.d("REMODROID", "sending... ");
                            udpSender.dataToSend = toByteArray;
                            Log.d("REMODROID", toByteArray.length+"");
                            udpSender.isDataReady = true;*/

                //udpSender.sendStreamPacket(toByteArray);

//                Log.d("REMODROID", "PCB Finished");
                sendPacket(toByteArray);
                doneWithPic = true;



            } catch (Exception e2) {
                Log.d("REMODROID", "Sending error " + e2.getMessage());
                stop();
            }
        }
    };

}
