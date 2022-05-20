using System.Collections;
using System;
using UnityEngine;
using TMPro;
public class PresentsManager : MonoBehaviour
{
    public GameObject timerPreview;
    public GameObject btnPreview;
    private DateTime date;
    private CoinsManager coins;
    private string typeBtn = "EveryDaySurprice";
    private void Start()
    {
        coins = FindObjectOfType<CoinsManager>();
        date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
        StartCoroutine(TimerPrice());
        CheckBtnTimer(timerPreview, btnPreview);
    }

    private IEnumerator TimerPrice()
    {
        while (date > DateTime.Now)
        {
            TimeSpan t = date - DateTime.Now;
            timerPreview.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            String.Format("{0:00}:{1:00}:{2:00}",
            t.TotalHours,
            (int)t.TotalMinutes - (int)t.TotalHours * 60,
            (int)t.TotalSeconds - (int)t.TotalMinutes * 60);
            yield return new WaitForSeconds(1f);
        }
    }

    public void GetPrice()
    {
        PlayerPrefs.SetInt(typeBtn, 1);
        DateTime t = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        DateTime t2 = t.AddDays(1);
        PlayerPrefs.SetString(typeBtn + "_Date", t2.ToString());
        btnPreview.SetActive(false);
        timerPreview.SetActive(true);
        coins.AddCoins(new Vector3(0,0,0), 100, true);
    }
    public void CheckBtnTimer(GameObject timer = null, GameObject text = null)
    {
        if (PlayerPrefs.HasKey(typeBtn) == false) PlayerPrefs.SetInt(typeBtn, 0);
        if (PlayerPrefs.HasKey(typeBtn + "_Date") == false)
        {
            DateTime t = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            PlayerPrefs.SetString(typeBtn + "_Date", t.ToString());
        }
        if (DateTime.Parse(PlayerPrefs.GetString(typeBtn + "_Date")) < DateTime.Now)
        {
            PlayerPrefs.SetInt(typeBtn, 0);
            text.SetActive(true);
            timer.SetActive(false);
        }
        else
        {
            text.SetActive(false);
            timer.SetActive(true);
        }
    }
}
