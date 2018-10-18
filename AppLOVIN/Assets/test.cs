using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{

    public InputField inputField;
    private string debugStr;
    // Use this for initialization
    void Start()
    {
        Application.logMessageReceived += DebugInfo;
    }

    private void DebugInfo(string condition, string stackTrace, LogType type)
    {
        debugStr += condition;
        inputField.text = debugStr;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Init()
    {
        AppLovin.InitializeSdk();
    }
    public void Show()
    {
		Debug.Log("HasPreloadedInterstitial:"+AppLovin.HasPreloadedInterstitial());
        if (AppLovin.HasPreloadedInterstitial())
        {
            AppLovin.ShowInterstitial();
        }

    }
    public void PreInterstial()
    {
        AppLovin.PreloadInterstitial();
    }
    // /// <summary>
    // /// OnGUI is called for rendering and handling GUI events.
    // /// This function can be called multiple times per frame (one call per event).
    // /// </summary>
    // void OnGUI()
    // {
    // 	if(GUILayout.Button("PreInterstial")){

    // 	}
    // 	GUILayout.Label("Has Interstial Ad:"+AppLovin.HasPreloadedInterstitial());
    // 	if (GUILayout.Button("ShowInterstial"))
    // 	{
    // 		if (AppLovin.HasPreloadedInterstitial())
    // 		{
    // 			AppLovin.ShowInterstitial();
    // 		}
    // 	}
    // }
}
