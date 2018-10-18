AppLovin Unity Plugin

https://www.applovin.com/

================


- Getting Started -


Android/FireOS -

1) Open the AndroidManifest file and put your AppLovin SDK key where it says YOUR_SDK_KEY.

2) Replace all instances of "YOUR_PACKAGE_NAME" with your application's package name.


iOS -

You need to call AppLovin.setSdkKey("YOUR_SDK_KEY") or set AppLovinSdkKey to your SDK Key in your applications info.plist every time after compiling from Unity.


Both -

We recommend you call AppLovin.InitializeSdk() before calling any of the showAd/showInterstitial methods.
This will allow the SDK to perform initial start-up tasks like pre-caching the first ad, resulting in
a more responsive initial ad display.

For complete integration instructions, please refer to the following links:

Android - https://applovin.com/integration#unity3dIntegration
iOS - https://applovin.com/integration#iosUnity3dIntegration
FireOS - https://applovin.com/integration#fireosUnity3dIntegration


---------------------------


If you have any questions regarding the Unity Plugin, contact AppLovin support at support@applovin.com

https://www.applovin.com/
