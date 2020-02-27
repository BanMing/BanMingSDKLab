using UnityEngine;
using System;
using System.Collections.Generic;

namespace AudienceNetwork
{
    [Serializable]
    public class ExtraHints
    {
        public struct Keyword
        {
            public const string ACCESSORIES = "accessories";
            public const string ART_HISTORY = "art_history";
            public const string AUTOMOTIVE = "automotive";
            public const string BEAUTY = "beauty";
            public const string BIOLOGY = "biology";
            public const string BOARD_GAMES = "board_games";
            public const string BUSINESS_SOFTWARE = "business_software";
            public const string BUYING_SELLING_HOMES = "buying_selling_homes";
            public const string CATS = "cats";
            public const string CELEBRITIES = "celebrities";
            public const string CLOTHING = "clothing";
            public const string COMIC_BOOKS = "comic_books";
            public const string DESKTOP_VIDEO = "desktop_video";
            public const string DOGS = "dogs";
            public const string EDUCATION = "education";
            public const string EMAIL = "email";
            public const string ENTERTAINMENT = "entertainment";
            public const string FAMILY_PARENTING = "family_parenting";
            public const string FASHION = "fashion";
            public const string FINE_ART = "fine_art";
            public const string FOOD_DRINK = "food_drink";
            public const string FRENCH_CUISINE = "french_cuisine";
            public const string GOVERNMENT = "government";
            public const string HEALTH_FITNESS = "health_fitness";
            public const string HOBBIES = "hobbies";
            public const string HOME_GARDEN = "home_garden";
            public const string HUMOR = "humor";
            public const string INTERNET_TECHNOLOGY = "internet_technology";
            public const string LARGE_ANIMALS = "large_animals";
            public const string LAW = "law";
            public const string LEGAL_ISSUES = "legal_issues";
            public const string LITERATURE = "literature";
            public const string MARKETING = "marketing";
            public const string MOVIES = "movies";
            public const string MUSIC = "music";
            public const string NEWS = "news";
            public const string PERSONAL_FINANCE = "personal_finance";
            public const string PETS = "pets";
            public const string PHOTOGRAPHY = "photography";
            public const string POLITICS = "politics";
            public const string REAL_ESTATE = "real_estate";
            public const string ROLEPLAYING_GAMES = "roleplaying_games";
            public const string SCIENCE = "science";
            public const string SHOPPING = "shopping";
            public const string SOCIETY = "society";
            public const string SPORTS = "sports";
            public const string TECHNOLOGY = "technology";
            public const string TELEVISION = "television";
            public const string TRAVEL = "travel";
            public const string VIDEO_COMPUTER_GAMES = "video_computer_games";
        }

        private const int KEYWORDS_MAX_COUNT = 5;

        public List<string> keywords;
        public string extraData;
        public string contentURL;

        internal AndroidJavaObject GetAndroidObject()
        {
            AndroidJavaObject builderExtraHintsAndroid = new AndroidJavaObject("com.facebook.ads.ExtraHints$Builder");
            if (builderExtraHintsAndroid != null)
            {
                if (keywords != null)
                {
                    AndroidJavaClass androidKeywordEnum = new AndroidJavaClass("com.facebook.ads.ExtraHints$Keyword");
                    AndroidJavaObject[] androidKeywordArray = androidKeywordEnum.CallStatic<AndroidJavaObject[]>("values");
                    AndroidJavaObject list = new AndroidJavaObject("java.util.ArrayList");
                    int currentCount = 0;
                    foreach (string keyword in keywords)
                    {
                        if (currentCount == KEYWORDS_MAX_COUNT)
                        {
                            break;
                        }

                        foreach (AndroidJavaObject obj in androidKeywordArray)
                        {
                            if (obj.Call<string>("toString").ToLower() == keyword)
                            {
                                list.Call<bool>("add", obj);
                                currentCount++;
                                break;
                            }
                        }
                    }

                    builderExtraHintsAndroid = builderExtraHintsAndroid
                        .Call<AndroidJavaObject>("keywords", list);

                }
                if (extraData != null)
                {
                    builderExtraHintsAndroid = builderExtraHintsAndroid
                        .Call<AndroidJavaObject>("extraData", extraData);
                }
                if (contentURL != null)
                {
                    builderExtraHintsAndroid = builderExtraHintsAndroid
                        .Call<AndroidJavaObject>("contentUrl", contentURL);
                }
            }

            return builderExtraHintsAndroid.Call<AndroidJavaObject>("build");
        }
    }
}
