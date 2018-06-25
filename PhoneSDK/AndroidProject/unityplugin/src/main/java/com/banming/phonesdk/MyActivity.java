package com.banming.phonesdk;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.widget.Toast;

import com.unity3d.player.UnityPlayerActivity;

public class MyActivity extends UnityPlayerActivity {
    @Override
    protected void onCreate(Bundle bundle) {
        super.onCreate(bundle);
    }

    /*
     * 创建Toast
     */
    public void CreateToast(final String toast) {
        runOnUiThread(new Runnable() {
            @Override
            public void run() {
                //onCoderReturn("CreateToast()");
                Toast.makeText(
                        MyActivity.this,
                        toast, Toast.LENGTH_LONG).show();
            }
        });
    }

    /************************调用第三方app *****************************/

    public void CallThirdApp(String packageName)//packageName
    {
        Intent intent = getPackageManager().getLaunchIntentForPackage(packageName);
        startActivity(intent);
    }

}
