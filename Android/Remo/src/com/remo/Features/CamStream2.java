package com.remo.Features;

import android.graphics.ImageFormat;
import android.hardware.Camera;
import android.os.AsyncTask;
import android.util.Log;
import com.remo.Connections.Feature;

import static android.content.ContentValues.TAG;


public class CamStream2{

}


//
//public class CamStream2 extends Feature implements Camera.PreviewCallback {
//
//    Camera camera = null;
//
//    @Override
//    public void AsyncTaskFunc() {
//
//    }
//
//
//        @Override
//        public void onPreviewFrame(byte[] data, Camera camera) {
//
//            if (imageFormat == ImageFormat.NV21) {
//                Log.i(TAG, "onPreviewFrame");
//                if (mProcessInProgress) {
//                    mCamera.addCallbackBuffer(data);
//                }
//                if (data == null) {
//                    return;
//                }
//
//                mProcessInProgress = true;
//                if (mBitmap == null) {
//                    mBitmap = Bitmap.createBitmap(width, height,
//                            Bitmap.Config.ARGB_8888);
//                    myCameraPreview.setImageBitmap(mBitmap);
//                }
//                myCameraPreview.invalidate();
//
//                mCamera.addCallbackBuffer(data);
//
//                // set flag for processing
//                mProcessInProgress = true;
//
//                // call AsyncTask
//                new ProcessPreviewDataTask().execute(data);
//            }
//        }
//
//    private class ProcessPreviewDataTask
//        extends AsyncTask<byte[], Void, Boolean> {
//
//        @Override
//        protected Boolean doInBackground(byte[]... datas) {
//            Log.i(TAG, "background process started");
//
//            byte[] data = datas[0];
//
//            // process data function
//            convertGray(width, height, data, pixels);
//
//            mCamera.addCallbackBuffer(data);
//
//            // change processing flag
//            mProcessInProgress = false;
//
//            return true;
//        }
//
//        @Override
//        protected void onPostExecute(Boolean result) {
//            Log.i(TAG, "running onPostExecute");
//            // set pixels once processing is done
//
//            myCameraPreview.invalidate();
//            mBitmap.setPixels(pixels, 0, previewSizeWidth,
//                    0, 0, previewSizeWidth, previewSizeHeight);
//            myCameraPreview.setImageBitmap(mBitmap);
//        }
//    }