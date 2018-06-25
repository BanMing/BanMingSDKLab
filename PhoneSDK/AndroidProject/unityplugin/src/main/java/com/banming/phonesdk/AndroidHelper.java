package com.banming.phonesdk;

import android.app.Activity;
import android.content.Intent;
import android.widget.Toast;

public class AndroidHelper {
    private AndroidHelper() {
    }
    private static  class singlon{
        private  static  final AndroidHelper INSTANCE=new AndroidHelper();
    }

    public static AndroidHelper Instance() {
        return singlon.INSTANCE;
    }


    /************************调用第三方app *****************************/

    public void CallThirdAppWithActivity(Activity activity, String packageName) {
        Intent intent = activity.getPackageManager().getLaunchIntentForPackage(packageName);
        activity.startActivity(intent);
    }

    /*
     * 创建Toast
     */
    public void CreateToast(final Activity activity, final String toast) {
        activity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                //onCoderReturn("CreateToast()");
                Toast.makeText(activity,
                        toast, Toast.LENGTH_LONG).show();
            }
        });
    }

    public static void CreateToastStatic(final Activity activity, final String toast) {
        activity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                //onCoderReturn("CreateToast()");
                Toast.makeText(activity,
                        toast, Toast.LENGTH_LONG).show();
            }
        });
    }

    public static void CallThirdApp(Activity activity, String packageName) {
        Intent intent = activity.getPackageManager().getLaunchIntentForPackage(packageName);
        activity.startActivity(intent);
    }

    public int TestAdd(int a, int b) {
        return a + b;
    }

    public static int TestStaticAdd(int a, int b) {
        return a + b;
    }
}
