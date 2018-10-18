using UnityEngine;
using System.Collections;


/**
 * Class containing constants for use with {@link AppLovinEventService#trackEvent(string, Map)}.
 */
public class AppLovinEvents
{
	public class Types
	{
		/**
         * @name Authentication Events
         */
        
		/**
         * Event signifying that the user logged in to an existing account.
         *
         * Suggested parameters: UserAccountIdentifier.
         */
		public const string UserLoggedIn = "login";
        
		/**
         * Event signifying that the finished a registration flow and created a new account.
         *
         * Suggested parameters: UserAccountIdentifier.
         */
		public const string UserCreatedAccount = "registration";
        
		/**
         * @name Gaming Events
         */
        
		/**
         * Event signifying that the user completed a tutorial or introduction sequence.
         *
         * Suggested parameters: None.
         */
		public const string UserCompletedTutorial = "tutorial";
        
		/**
         * Event signifying that the user completed a given level or game sequence.
         *
         * Suggested parameters: CompletedLevelIdentifier.
         */
		public const string UserCompletedLevel = "level";
        
		/**
         * Event signifying that the user completed (or "unlocked") a particular achievement.
         *
         * Suggested parameters: CompletedAchievementIdentifier.
         */
		public const string UserCompletedAchievement = "achievement";
        
		/**
         * Event signifying that the user spent virtual currency on an in-game purchase.
         *
         * Suggested parameters: VirtualCurrencyAmount.
         */
		public const string UserSpentVirtualCurrency = "vcpurchase";
        
		/**
         * Event signifying that the user completed an iTunes in-app purchase using StoreKit.
         *
         * Note that this event implies an in-app content purchase; for purchases of general products completed using Apple Pay, use kALEventTypeUserCompletedCheckOut instead.
         *
         * Suggested parameters: InAppPurchaseProductIdentifier.
         */
		public const string UserCompletedInAppPurchase = "iap";
        
		/**
         * @name Social Events
         */
        
		/**
         * Event signifying that the user sent an invitation to use your app to a friend.
         *
         * Suggested parameters: None.
         */
		public const string UserSentInvitation = "invite";
        
		/**
         * Event signifying that the user shared a link or deep-link to some content within your app.
         *
         * Suggested parameters: None.
         */
		public const string UserSharedLink = "share";
	}

	public class Parameters
	{
		/**
         * Dictionary key which represents the username or account ID of the user. All keys and values in the parameter map should be of type String.
         */
		public const string UserAccountIdentifier = "username";
        
		/**
         * Dictionary key which represents a search query executed by the user. All keys and values in the parameter map should be of type String.
         */
		public const string SearchQuery = "query";
        
		/**
         * Dictionary key which represents an identifier of the level the user has just completed. All keys and values in the parameter map should be of type String.
         */
		public const string CompletedLevelIdentifier = "level_id";
        
		/**
         * Dictionary key which represents an identifier of the achievement the user has just completed/unlocked. All keys and values in the parameter map should be of type String.
         */
		public const string CompletedAchievementIdentifier = "achievement_id";
        
		/**
         * Dictionary key which represents the amount of virtual currency that a user spent on an in-game purchase. All keys and values in the parameter map should be of type String.
         */
		public const string VirtualCurrencyAmount = "vcamount";
        
		/**
         * Dictionary key which represents the name of the virtual currency that a user spent on an in-game purchase. All keys and values in the parameter map should be of type String.
         */
		public const string VirtualCurrencyName = "vcname";
        
		/**
         * Dictionary key which identifies a transaction ID. All keys and values in the parameter map should be of type String.
         * For Android this is the transaction ID provided by Google Play In-app Billing. For iOS this is the transactionIdentifier property on an SKPaymentTransaction.
         */
		public const string InAppPurchaseTransactionIdentifier = "store_id";
        
		/**
         * Dictionary key which identifies a product ID of an in app purchase. This should be the ID defined in Google Play / iTunes Connect.
         */
		public const string InAppPurchaseProductIdentifier = "product_id";
        
		/**
         * Dictionary key which identifiers Google Play In-app Billing purchase data. This should be the value for key INAPP_PURCHASE_DATA in your IAP buy intent. All keys and values in the parameter map should be of type String.
         *
         * You may notice there is no equivalent key for iOS App Store In-app Purchases; this is because our iOS SDK will automatically collect the app store receipt for your app, so you don't have to do anything.
         */
		public const string GooglePlayInAppPurchaseData = "receipt_data";
        
		/**
         * Dictionary key which identifiers Google Play In-app Billing purchase data. This should be the value for key INAPP_DATA_SIGNATURE in your IAP buy intent. All keys and values in the parameter map should be of type String.
         *
         * You may notice there is no equivalent key for iOS App Store In-app Purchases; this is because our iOS SDK will automatically collect the app store receipt for your app, so you don't have to do anything.
         */
		public const string GooglePlayInAppPurchaseDataSignature = "receipt_data_signature";
        
		/**
         * Dictionary key which represents the amount of revenue generated by a purchase event. All keys and values in the parameter map should be of type String.
         */
		public const string RevenueAmount = "amount";
        
		/**
         * Dictionary key which represents the currency of the revenue event. All keys and values in the parameter map should be of type String.
         *
         * Ideally this should be an ISO 4217 3-letter currency code (for instance, USD, EUR, GBP...)
         */
		public const string RevenueCurrency = "currency";
	}
}
