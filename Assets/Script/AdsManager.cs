using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
    public static AdsManager instance;

    string AndroidAppId = "6004376";
    string bannerID = "Banner_Android";
    string interstitialId = "Interstitial_Android";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Advertisement.Initialize(AndroidAppId);
        LoadBanner();
        LoadInterstitial();
    }

    // ---------------- BANNER ----------------
    void LoadBanner()
    {
        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
        Advertisement.Banner.Load(bannerID);
        Advertisement.Banner.Show(bannerID);
    }

    public void HideBanner()
    {
        Advertisement.Banner.Hide();
    }

    // ---------------- INTERSTITIAL ----------------
    void LoadInterstitial()
    {
        Advertisement.Load(interstitialId);
    }

    public void ShowInterstitialAd()
    {
        Advertisement.Show(interstitialId);
        LoadInterstitial(); // preload next
    }
}
