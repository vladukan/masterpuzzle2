using UnityEngine;
using UnityEngine.Purchasing;
public class IAPmanager : MonoBehaviour
{
    private AdMob ads;
    public GameObject btnAds;
    private void Start()
    {
        ads = FindObjectOfType<AdMob>();
        if (!PlayerPrefs.HasKey("ads") || PlayerPrefs.GetInt("ads") == 1) btnAds.SetActive(true);
    }
    public void OnPurchaseCompleted(Product product)
    {
        switch (product.definition.id)
        {
            case "noads":
                PlayerPrefs.SetInt("ads", 0);
                ads.banner.Hide();
                btnAds.SetActive(false);
                break;
        }
    }
    public void OnPurchaseFailed()
    {
        print("Fail purchase");
    }
}
