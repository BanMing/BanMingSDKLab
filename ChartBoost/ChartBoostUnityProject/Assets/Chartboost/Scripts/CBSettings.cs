using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ChartboostSDK {

#if UNITY_EDITOR	
	[InitializeOnLoad]
#endif
	public class CBSettings : ScriptableObject
	{
		const string cbSettingsAssetName = "ChartboostSettings";
	    const string cbSettingsPath = "Chartboost/Resources";
	    const string cbSettingsAssetExtension = ".asset";

	    const string iOSExampleAppIDLabel = "CB_IOS_APP_ID";
	    const string iOSExampleAppSignatureLabel = "CB_IOS_APP_SIGNATURE";
	    const string iOSExampleAppID = "4f21c409cd1cb2fb7000001b";
	    const string iOSExampleAppSignature = "92e2de2fd7070327bdeb54c15a5295309c6fcd2d";

		const string androidExampleAppIDLabel = "CB_ANDROID_APP_ID";
	    const string androidExampleAppSignatureLabel = "CB_ANDROID_APP_SIGNATURE";
	    const string androidExampleAppID = "4f7b433509b6025804000002";
	    const string androidExampleAppSignature = "dd2d41b69ac01b80f443f5b6cf06096d457f82bd";

		const string amazonExampleAppIDLabel = "CB_AMAZON_APP_ID";
	    const string amazonExampleAppSignatureLabel = "CB_AMAZON_APP_SIGNATURE";
	    const string amazonExampleAppID = "542ca35d1873da32dbc90488";
	    const string amazonExampleAppSignature = "90654a340386c9fb8de33315e4210d7c09989c43";
	    
	    const string credentialsWarningDefaultFormat = "CHARTBOOST: You are using the Chartboost {0} example {1}! Go to the Chartboost dashboard and replace these with an App ID & App Signature from your account! If you need help, check out answers.chartboost.com"; 
	    const string credentialsWarningEmptyFormat = "CHARTBOOST: You are using an empty string for the {0} {1}! Go to the Chartboost dashboard and replace these with an App ID & App Signature from your account! If you need help, check out answers.chartboost.com"; 
	    const string credentialsWarningIOS = "IOS"; 
	    const string credentialsWarningAndroid = "Android"; 
	    const string credentialsWarningAmazon = "Amazon"; 
	    const string credentialsWarningAppID = "App ID"; 
	    const string credentialsWarningAppSignature = "App Signature"; 

	    private static bool credentialsWarning = false;

	    private static CBSettings instance;

	    static CBSettings Instance
	    {
	        get
	        {
	            if (instance == null)
	            {
	                instance = Resources.Load(cbSettingsAssetName) as CBSettings;
	                if (instance == null)
	                {
	                    // If not found, autocreate the asset object.
	                    instance = CreateInstance<CBSettings>();
	#if UNITY_EDITOR
	                    string properPath = Path.Combine(Application.dataPath, cbSettingsPath);
	                    if (!Directory.Exists(properPath))
	                    {
	                        AssetDatabase.CreateFolder("Assets/Chartboost", "Resources");
	                    }

	                    string fullPath = Path.Combine(Path.Combine("Assets", cbSettingsPath),
	                                                   cbSettingsAssetName + cbSettingsAssetExtension
	                                                  );
	                    AssetDatabase.CreateAsset(instance, fullPath);
	#endif
	                }
	            }
	            return instance;
	        }
	    }

	#if UNITY_EDITOR
	    [MenuItem("Chartboost/Edit Settings")]
	    public static void Edit()
	    {
	        Selection.activeObject = Instance;
	    }

	    [MenuItem("Chartboost/SDK Documentation")]
	    public static void OpenDocumentation()
	    {
	        string url = "https://answers.chartboost.com/en-us/child_article/unity";
	        Application.OpenURL(url);
	    }
	#endif

	    #region App Settings
		[SerializeField]
		public string iOSAppId = iOSExampleAppIDLabel;
		[SerializeField]
		public string iOSAppSecret = iOSExampleAppSignatureLabel;
		[SerializeField]
		public string androidAppId = androidExampleAppIDLabel;
		[SerializeField]
		public string androidAppSecret = androidExampleAppSignatureLabel;
		[SerializeField]
		public string amazonAppId = amazonExampleAppIDLabel;
		[SerializeField]
		public string amazonAppSecret = amazonExampleAppSignatureLabel;
		[SerializeField]
		public bool isLoggingEnabled = false;
		[SerializeField]
		public string[] androidPlatformLabels = new[] { "Google Play", "Amazon" };
		[SerializeField]
		public int selectedAndroidPlatformIndex = 0;

		// allow mediation partners to set the appId and appSignature from code
		// if set, overrides the values set in the editor
		public static void setAppId(string appId, string appSignature)
		{
#if UNITY_IPHONE
			Debug.Log("Overriding IOS AppId: " + appId);
			Instance.SetIOSAppId(appId);
			Instance.SetIOSAppSecret(appSignature);
#elif UNITY_ANDROID
			// Google
			if (Instance.selectedAndroidPlatformIndex == 0)
			{
				Debug.Log("Overriding Google AppId: " + appId);
				Instance.SetAndroidAppId(appId);
				Instance.SetAndroidAppSecret(appSignature);
			}
			else
			{
				Debug.Log("Overriding Amazon AppId: " + appId);
				Instance.SetAmazonAppId(appId);
				Instance.SetAmazonAppSecret(appSignature);
			}
#endif			
		}

		public void SetAndroidPlatformIndex(int index)
		{
			if (selectedAndroidPlatformIndex != index)
			{
				selectedAndroidPlatformIndex = index;
				DirtyEditor();
			}
		}

		public int SelectedAndroidPlatformIndex
		{
			get { return selectedAndroidPlatformIndex; }
		}

		public string[] AndroidPlatformLabels
		{
			get { return androidPlatformLabels; }
			set
			{
				if (!androidPlatformLabels.Equals(value))
				{
					androidPlatformLabels = value;
					DirtyEditor();
				}
			}
		}

		// iOS
	    public void SetIOSAppId(string id)
	    {
	        if (!Instance.iOSAppId.Equals(id))
	        {
	            Instance.iOSAppId = id;
	            DirtyEditor();
	        }
	    }

		public static string getIOSAppId()
		{
			if(Instance.iOSAppId.Equals(iOSExampleAppIDLabel))
			{
				CredentialsWarning(credentialsWarningDefaultFormat, credentialsWarningIOS, credentialsWarningAppID);

				return iOSExampleAppID;
			}
			if(Instance.iOSAppId.Equals(""))
			{
				CredentialsWarning(credentialsWarningEmptyFormat, credentialsWarningIOS, credentialsWarningAppID);
				// use it anyway
			}
			return Instance.iOSAppId;
		}

	    public void SetIOSAppSecret(string secret)
	    {
	        if (!Instance.iOSAppSecret.Equals(secret))
	        {
	            Instance.iOSAppSecret = secret;
	            DirtyEditor();
	        }
	    }

		public static string getIOSAppSecret()
		{
			if(Instance.iOSAppSecret.Equals(iOSExampleAppSignatureLabel))
			{
				CredentialsWarning(credentialsWarningDefaultFormat, credentialsWarningIOS, credentialsWarningAppSignature);

				return iOSExampleAppSignature;
			}
			if(Instance.iOSAppSecret.Equals(""))
			{
				CredentialsWarning(credentialsWarningEmptyFormat, credentialsWarningIOS, credentialsWarningAppSignature);
				// use it anyway
			}

			return Instance.iOSAppSecret;
		}

		// Android
		public void SetAndroidAppId(string id)
		{
			if (!Instance.androidAppId.Equals(id))
			{
				Instance.androidAppId = id;
				DirtyEditor();
			}
		}
		
		public static string getAndroidAppId()
		{	
			if(Instance.androidAppId.Equals(androidExampleAppIDLabel))
			{
				CredentialsWarning(credentialsWarningDefaultFormat, credentialsWarningAndroid, credentialsWarningAppID);

				return androidExampleAppID;
			}
			if(Instance.androidAppId.Equals(""))
			{
				CredentialsWarning(credentialsWarningEmptyFormat, credentialsWarningAndroid, credentialsWarningAppID);
				// use it anyway
			}

			return Instance.androidAppId;
		}
		
		public void SetAndroidAppSecret(string secret)
		{
			if (!Instance.androidAppSecret.Equals(secret))
			{
				Instance.androidAppSecret = secret;
				DirtyEditor();
			}
		}
		
		public static string getAndroidAppSecret()
		{
			if(Instance.androidAppSecret.Equals(androidExampleAppSignatureLabel))
			{
				CredentialsWarning(credentialsWarningDefaultFormat, credentialsWarningAndroid, credentialsWarningAppSignature);

				return androidExampleAppSignature;
			}
			if(Instance.androidAppSecret.Equals(""))
			{
				CredentialsWarning(credentialsWarningEmptyFormat, credentialsWarningAndroid, credentialsWarningAppSignature);
				// use it anyway
			}

			return Instance.androidAppSecret;
		}

		// Amazon
		public void SetAmazonAppId(string id)
		{
			if (!Instance.amazonAppId.Equals(id))
			{
				Instance.amazonAppId = id;
				DirtyEditor();
			}
		}
		
		public static string getAmazonAppId()
		{
			if(Instance.amazonAppId.Equals(amazonExampleAppIDLabel))
			{
				CredentialsWarning(credentialsWarningDefaultFormat, credentialsWarningAmazon, credentialsWarningAppID);

				return amazonExampleAppID;
			}
			if(Instance.amazonAppId.Equals(""))
			{
				CredentialsWarning(credentialsWarningEmptyFormat, credentialsWarningAmazon, credentialsWarningAppID);
				// use it anyway
			}

			return Instance.amazonAppId;
		}
		
		public void SetAmazonAppSecret(string secret)
		{
			if (!Instance.amazonAppSecret.Equals(secret))
			{
				Instance.amazonAppSecret = secret;
				DirtyEditor();
			}
		}
		
		public static string getAmazonAppSecret()
		{
			if(Instance.amazonAppSecret.Equals(amazonExampleAppSignatureLabel))
			{
				CredentialsWarning(credentialsWarningDefaultFormat, credentialsWarningAmazon, credentialsWarningAppSignature);
				
				return amazonExampleAppSignature;
			}
			if(Instance.amazonAppSecret.Equals(""))
			{
				CredentialsWarning(credentialsWarningEmptyFormat, credentialsWarningAmazon, credentialsWarningAppSignature);
				// use it anyway
			}

			return Instance.amazonAppSecret;
		}

		public static string getSelectAndroidAppId()
		{
			// Google
			if (Instance.selectedAndroidPlatformIndex == 0)
			{
				return getAndroidAppId();
			}
			// Amazon
			else 
			{
				return getAmazonAppId();
			}
		}

		public static string getSelectAndroidAppSecret()
		{
			// Google
			if (Instance.selectedAndroidPlatformIndex == 0)
			{
				return getAndroidAppSecret();
			}
			// Amazon
			else 
			{
				return getAmazonAppSecret();
			}
		}

		public static void enableLogging(bool enabled)
		{
			Instance.isLoggingEnabled = enabled;

			DirtyEditor();
		}

		public static bool isLogging()
		{
			return Instance.isLoggingEnabled;
		}

	    private static void DirtyEditor()
	    {
	#if UNITY_EDITOR
	        EditorUtility.SetDirty(Instance);
	#endif
	    }

	    private static void CredentialsWarning(string warning, string platform, string field)
	    {
	    	if(credentialsWarning == false) 
	    	{
				credentialsWarning = true;

				// Substitute the platform name in the warning
				Debug.LogWarning( string.Format(warning, platform, field));
			}
	    }

		public static void resetSettings()
		{	
			// iOS
			if(Instance.iOSAppId.Equals(iOSExampleAppID))
			{
				Instance.SetIOSAppId(iOSExampleAppIDLabel);
			}
			
			if(Instance.iOSAppSecret.Equals(iOSExampleAppSignature))
			{
				Instance.SetIOSAppSecret(iOSExampleAppSignatureLabel);
			}

			// Android
			if(Instance.androidAppId.Equals(androidExampleAppID))
			{
				Instance.SetAndroidAppId(androidExampleAppIDLabel);
			}
			
			if(Instance.androidAppSecret.Equals(androidExampleAppSignature))
			{
				Instance.SetAndroidAppSecret(androidExampleAppSignatureLabel);
			}

			// Amazon
			if(Instance.amazonAppId.Equals(amazonExampleAppID))
			{
				Instance.SetAmazonAppId(amazonExampleAppIDLabel);
			}

			if(Instance.amazonAppSecret.Equals(amazonExampleAppSignature))
			{
				Instance.SetAmazonAppSecret(amazonExampleAppSignatureLabel);
			}
		}

	    #endregion
	}
}
