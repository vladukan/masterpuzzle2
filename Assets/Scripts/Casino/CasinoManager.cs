using UnityEngine;
using System.Collections;
public class CasinoManager : MonoBehaviour
{
    public GameObject CasinoScreen;
    public GameObject BackCasino;
    public GameObject Ruletka;
    public GameObject BtnSpin;
    public int price;
    private CoinsManager coins;
    private SurpriceManager surprice;
    void Start()
    {
        coins = FindObjectOfType<CoinsManager>();
        surprice = FindObjectOfType<SurpriceManager>();
    }
    public void GetRuletkaNumber()
    {
        StartCoroutine(RotateRuletka());
    }
    IEnumerator RotateRuletka()
    {
        BtnSpin.SetActive(false);
        BackCasino.SetActive(false);
        RectTransform rect = Ruletka.GetComponent<RectTransform>();
        yield return new WaitForSeconds(0.5f);
        int i = Random.Range(-25, 25);
        int j = Random.Range(1, 6);
        switch (j)
        {
            case 1: i += 30; price = 600; break;
            case 2: i += 90; price = 400; break;
            case 3: i += 150; price = 1000; break;
            case 4: i += 210; price = 0; break;
            case 5: i += 270; price = 100; break;
            case 6: i += 330; price = 200; break;
        }
        j = Random.Range(3, 5);
        onCompleteRotate(rect, j * 360f + i);
    }
    private void onCompleteRotate(RectTransform rect, float rotate, float time = 5f)
    {
        LeanTween.rotate(rect, rotate, time).setEase(LeanTweenType.easeOutExpo)
        .setOnComplete(() =>
        {
            surprice.screenSurprice.SetActive(true);
            if (price == 0 && surprice.bonusHats.Count != 0)
            {
                surprice.LoadBonusHat();
                surprice.CreateHatMenu(true);
                surprice.animatorSurprice.Play("OnHatsSurprice");
            }
            else
            {
                if (surprice.bonusHats.Count == 0) price = 500;
                surprice.OpenSurpriceAmount(price);
            }
            CasinoScreen.SetActive(false);
            coins.BtnCasino.SetActive(false);
        });
    }
    public void SetRotateBegin()
    {
        Ruletka.transform.localEulerAngles = new Vector3(0, 0, 0);
        BtnSpin.SetActive(true);
        BackCasino.SetActive(true);
    }
}
