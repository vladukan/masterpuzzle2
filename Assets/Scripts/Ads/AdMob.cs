using UnityEngine;
using System;
using System.Collections;
using GoogleMobileAds.Api;
using UnityEngine.Networking;
public class AdMob : MonoBehaviour
{
    public InterstitialAd interstitialAd;
    public BannerView banner;
    private string IdPageAds = "ca-app-pub-3940256099942544/1033173712";
    private string IdBanner = "ca-app-pub-3940256099942544/6300978111";
    private float timer = 30f;
    private float interval = 30f;
    private bool isConnect = true;
    private UiManager ui;
    private CoinsManager coins;
    private SurpriceManager surprice;
    private void Awake()
    {
        if (PlayerPrefs.HasKey("ads") && PlayerPrefs.GetInt("ads") == 0) PlayerPrefs.SetInt("ads", 0);
        else PlayerPrefs.SetInt("ads", 1);
        MobileAds.Initialize(initStatus => { });
    }
    private void Start()
    {
        ui = FindObjectOfType<UiManager>();
        timer = 30f;
        StartCoroutine(Timer());
        StartCoroutine(TestConnection());
    }
    private void OnEnable()
    {
        CreateInterAd();
        CreateBanner();
    }
    IEnumerator Timer()
    {
        while (true)
        {
            timer++;
            yield return new WaitForSeconds(1f);
        }
    }
    public void ShowAds()
    {
        if (PlayerPrefs.GetInt("ads") == 0 || ui.level <= 3) return;
        if (timer >= interval)
        {
            if (interstitialAd.IsLoaded())
            {
                interstitialAd.Show();
                timer = 1f;
            }
            else
            {
                AdRequest adRequest = new AdRequest.Builder().Build();
                interstitialAd.LoadAd(adRequest);
            }
        }
    }


    IEnumerator ShowBanner()
    {
        yield return new WaitForSeconds(3f);
        banner.Show();
    }
    private void CreateInterAd()
    {
        interstitialAd = new InterstitialAd(IdPageAds);
        interstitialAd.OnAdClosed += HandleOnAdClosed;
        AdRequest adRequest = new AdRequest.Builder().Build();
        interstitialAd.LoadAd(adRequest);
    }
    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        interstitialAd.Destroy();
        CreateInterAd();
    }

    public void CreateBanner()
    {
        if (PlayerPrefs.GetInt("ads") == 0) return;
        AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        banner = new BannerView(IdBanner, adaptiveSize, AdPosition.Bottom);
        AdRequest adRequest = new AdRequest.Builder().Build();
        banner.LoadAd(adRequest);
        StartCoroutine(ShowBanner());
    }
    IEnumerator TestConnection()
    {
        while (true)
        {
            //print("SEND REQUEST");
            using (UnityWebRequest webRequest = UnityWebRequest.Get("http://google.com"))
            {
                yield return webRequest.SendWebRequest();
                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError: isConnect = false; break;
                    case UnityWebRequest.Result.DataProcessingError: isConnect = false; break;
                    case UnityWebRequest.Result.ProtocolError: isConnect = false; break;
                    case UnityWebRequest.Result.Success:
                        if (!isConnect)
                        {
                            CreateBanner();
                            CreateInterAd();
                            //CreateRewardAd();
                            isConnect = true;
                            //print("Internet RECONNECT");
                        }
                        //print("Internet TRUE");
                        break;
                }
            }
            yield return new WaitForSeconds(10f);
        }

    }
    private void OnDisable()
    {
        banner.Hide();
        banner.Destroy();
        interstitialAd.Destroy();
    }
}
