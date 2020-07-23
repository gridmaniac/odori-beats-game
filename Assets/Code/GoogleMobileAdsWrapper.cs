using System;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class GoogleMobileAdsDemoHandler : IDefaultInAppPurchaseProcessor
{
    private readonly string[] validSkus = { "android.test.purchased" };

    //Will only be sent on a success.
    public void ProcessCompletedInAppPurchase(IInAppPurchaseResult result)
    {
        result.FinishPurchase();
        GoogleMobileAdsWrapper.OutputMessage = "Purchase Succeeded! Credit user here.";
    }

    //Check SKU against valid SKUs.
    public bool IsValidPurchase(string sku)
    {
        foreach(string validSku in validSkus)
        {
            if (sku == validSku)
            {
                return true;
            }
        }
        return false;
    }

    //Return the app's public key.
    public string AndroidPublicKey
    {
        //In a real app, return public key instead of null.
        get { return "pub-5649429931709993"; }
    }
}

// Example script showing how to invoke the Google Mobile Ads Unity plugin.
public class GoogleMobileAdsWrapper: MonoBehaviour
{

    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardBasedVideoAd rewardBasedVideo;
    private float deltaTime = 0.0f;
    private static string outputMessage = "";

    public static string OutputMessage
    {
        set { outputMessage = value; }
    }

    void Update()
    {
        // Calculate simple moving average for time to render screen. 0.1 factor used as smoothing
        // value.
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    public void RequestBanner()
    {
        #if UNITY_EDITOR
            string adUnitId = "ca-app-pub-5649429931709993/9622743861";
        #elif UNITY_ANDROID
            string adUnitId = "ca-app-pub-5649429931709993/9622743861";
        #elif UNITY_IPHONE
            string adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";
        #else
            string adUnitId = "unexpected_platform";
        #endif

        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
        // Register for ad events.
        bannerView.OnAdLoaded += HandleAdLoaded;
        bannerView.OnAdFailedToLoad += HandleAdFailedToLoad;
        bannerView.OnAdLoaded += HandleAdOpened;
        bannerView.OnAdClosed += HandleAdClosed;
        bannerView.OnAdLeavingApplication += HandleAdLeftApplication;
        // Load a banner ad.
        bannerView.LoadAd(createAdRequest());
    }

    public void KillBanner()
    {
        bannerView.Destroy();
    }

    public void RequestInterstitial()
    {
        #if UNITY_EDITOR
            string adUnitId = "ca-app-pub-5649429931709993/2099477069";
        #elif UNITY_ANDROID
            string adUnitId = "ca-app-pub-5649429931709993/2099477069";
        #elif UNITY_IPHONE
            string adUnitId = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
        #else
            string adUnitId = "unexpected_platform";
        #endif

        // Create an interstitial.
        interstitial = new InterstitialAd(adUnitId);
        // Register for ad events.
        interstitial.OnAdLoaded += HandleInterstitialLoaded;
        interstitial.OnAdFailedToLoad += HandleInterstitialFailedToLoad;
        interstitial.OnAdOpening += HandleInterstitialOpened;
        interstitial.OnAdClosed += HandleInterstitialClosed;
        interstitial.OnAdLeavingApplication += HandleInterstitialLeftApplication;
        // Load an interstitial ad.
        interstitial.LoadAd(createAdRequest());
    }

    // Returns an ad request with custom ad targeting.
    private AdRequest createAdRequest()
    {
        return new AdRequest.Builder()
                .AddTestDevice(AdRequest.TestDeviceSimulator)
                .AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
                .AddKeyword("game")
                .SetGender(Gender.Male)
                .SetBirthday(new DateTime(1985, 1, 1))
                .TagForChildDirectedTreatment(false)
                .AddExtra("color_bg", "9B30FF")
                .Build();
    }

    private void ShowInterstitial()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
        else
        {
            print("Interstitial is not ready yet.");
        }
    }

    private void ShowRewardBasedVideo()
    {
        if (rewardBasedVideo.IsLoaded())
        {
            rewardBasedVideo.Show();
        } else
        {
            print("Reward based video ad is not ready yet.");
        }
    }

    #region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        print("done");
        bannerView.Show();
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleFailedToReceiveAd event received with message: " + args.Message);
    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        print("HandleAdOpened event received");
    }

    void HandleAdClosing(object sender, EventArgs args)
    {
        print("HandleAdClosing event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        print("HandleAdClosed event received");
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        print("HandleAdLeftApplication event received");
    }

    #endregion

    #region Interstitial callback handlers

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        ShowInterstitial();
    }

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleInterstitialFailedToLoad event received with message: " + args.Message);
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        print("HandleInterstitialOpened event received");
    }

    void HandleInterstitialClosing(object sender, EventArgs args)
    {
        print("HandleInterstitialClosing event received");
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        print("HandleInterstitialClosed event received");
    }

    public void HandleInterstitialLeftApplication(object sender, EventArgs args)
    {
        print("HandleInterstitialLeftApplication event received");
    }

    #endregion

}
