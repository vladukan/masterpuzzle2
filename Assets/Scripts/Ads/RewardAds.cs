using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
public class RewardAds : MonoBehaviour
{
    public RewardedAd rewardedAd;
    private string IdRewardAds = "ca-app-pub-3940256099942544/5224354917";
    private string type;
    public Button TripleCoins;
    public Button CasinoSpin;
    public Button SkipLevel;
    public Button SkipAmmo;
    public Button EquipBtn;
    public GameObject Canvas;
    public CasinoManager casino;
    private CoinsManager coins;
    private SurpriceManager surprice;
    private void Start()
    {
        coins = FindObjectOfType<CoinsManager>();
        surprice = FindObjectOfType<SurpriceManager>();
        casino = FindObjectOfType<CasinoManager>();
    }
    private void OnEnable()
    {
        CreateRewardAd();
    }
    public void ShowRewarded(string name)
    {
        type = name;
        if (rewardedAd.IsLoaded()) rewardedAd.Show();
        else
        {
            AdRequest adRequest = new AdRequest.Builder().Build();
            rewardedAd.LoadAd(adRequest);
        }
    }
    public void CheckReward()
    {
        if (rewardedAd.IsLoaded())
        {
            TripleCoins.interactable = true;
            CasinoSpin.interactable = true;
            SkipAmmo.interactable = true;
            SkipLevel.interactable = true;
            EquipBtn.interactable = true;
        }
        else
        {
            TripleCoins.interactable = false;
            CasinoSpin.interactable = false;
            SkipAmmo.interactable = false;
            SkipLevel.interactable = false;
            EquipBtn.interactable = false;
        }
    }
    public void CreateRewardAd()
    {
        rewardedAd = new RewardedAd(IdRewardAds);
        rewardedAd.OnAdClosed += HandleClosedReward;
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        AdRequest adRequest = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(adRequest);
    }
    public void HandleUserEarnedReward(object sender, Reward args)
    {
        StartCoroutine(AwaitForCloseAds());
    }
    public void HandleClosedReward(object sender, EventArgs args)
    {
        AdRequest adRequest = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(adRequest);
    }
    IEnumerator AwaitForCloseAds()
    {
        yield return new WaitForEndOfFrame();
        switch (type)
        {
            case "triple":
                coins = FindObjectOfType<CoinsManager>();
                coins.AddLevelCoins(true);
                UiManager.Instance.NextLevel();
                break;
            case "casino": casino.GetRuletkaNumber(); break;
            case "hat":
                surprice = FindObjectOfType<SurpriceManager>();
                UiManager.Instance.giftMenu.SetActive(false);
                surprice.SaveHatLevel(true);
                break;
            case "skip":
                UiManager.Instance.NextLevel(true);
                break;
        }
    }
}
