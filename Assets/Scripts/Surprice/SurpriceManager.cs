using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SurpriceManager : MonoBehaviour
{
    public Image imageSurprice;
    public GameObject screenSurprice;
    public Animator animatorSurprice;
    public float countFill;
    public bool bonusHat = false;
    public GameObject containerHat;
    public GameObject containerHatBonus;
    public GameObject hatLevel;
    public GameObject hatLevelBonus;
    public int hatLevelID;
    public int hatBonusID;
    private int BonusCoins;
    public TextMeshProUGUI BonusCoinsText;
    private StoreManager store;
    private UiManager ui;
    private CoinsManager Coins;
    public List<int> bonusHats = new List<int>();
    public List<int> rewardHats = new List<int>();
    void Start()
    {
        store = FindObjectOfType<StoreManager>();
        ui = FindObjectOfType<UiManager>();
        Coins = FindObjectOfType<CoinsManager>();
        if (!PlayerPrefs.HasKey("countFillSurprice")) PlayerPrefs.SetFloat("countFillSurprice", 0f);
        else countFill = PlayerPrefs.GetFloat("countFillSurprice");
        if (countFill > 1) countFill = 1;
        imageSurprice.fillAmount = countFill;
        screenSurprice.SetActive(false);
    }
    IEnumerator FillImage(Image image, float time, float endFill)
    {
        float t = 0.0f;
        float startFill = image.fillAmount;

        while ((t += Time.deltaTime) <= time)
        {
            image.fillAmount = Mathf.Lerp(startFill, endFill, t / time);

            yield return null;
        }

        image.fillAmount = endFill;
        if (endFill == 1)
        {
            image.fillAmount = 0;
            countFill = 0;
            PlayerPrefs.SetFloat("countFillSurprice", 0);
            screenSurprice.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetFloat("countFillSurprice", countFill);
        }
    }
    public void StartFill()
    {
        if (ui.level > 6) countFill += 1f / 10f; else countFill += 1f / 5f;
        if (countFill > 1) countFill = 1;
        StartCoroutine(FillImage(imageSurprice, 1f, countFill));
    }
    public void OpenSurprice()
    {
        if (!PlayerPrefs.HasKey("typeBonus")) PlayerPrefs.SetInt("typeBonus", 0);
        if (PlayerPrefs.GetInt("typeBonus") == 0 && bonusHats.Count != 0)
        {
            PlayerPrefs.SetInt("typeBonus", 1);
            LoadBonusHat();
            CreateHatMenu(false);
            // animatorSurprice.Play("OnHatsSurprice");
            ui.giftMenu.SetActive(true);
            screenSurprice.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("typeBonus", 0);
            if (bonusHats.Count == 0)
            {
                BonusCoinsText.text = "+500";
                BonusCoins = 500;
            }
            else
            {
                int coins = Random.Range(0, 100);
                if (coins <= 33)
                {
                    BonusCoinsText.text = "+100";
                    BonusCoins = 50;
                }
                if (coins > 33 && coins <= 66)
                {
                    BonusCoinsText.text = "+200";
                    BonusCoins = 100;
                }
                if (coins > 66 && coins <= 100)
                {
                    BonusCoinsText.text = "+300";
                    BonusCoins = 200;
                }
            }
            animatorSurprice.Play("OnCoinsSurprice");
        }
    }
    public void CreateHatMenu(bool type = false)
    {
        if (!type) AddHatMenu(containerHat, hatLevel); else AddHatMenu(containerHatBonus, hatLevelBonus);
    }
    public void SaveHatLevel(bool type)
    {
        if (type && hatLevel != null) SaveHatEquip(hatLevelID, rewardHats); else SaveHatEquip(hatBonusID, bonusHats);
    }
    public void LoadBonusHat()
    {
        if (bonusHats.Count == 0) return;
        int id = Random.Range(0, bonusHats.Count);
        hatBonusID = bonusHats[id];
        GameObject prefab = Resources.Load<GameObject>($"Caps/Cap_{bonusHats[id]}") as GameObject;
        if (prefab != null)
        {
            hatLevelBonus = Lean.Pool.LeanPool.Spawn(prefab, transform.position, Quaternion.identity);
            hatLevelBonus.transform.SetParent(transform);
            if (hatLevelBonus.transform.GetComponent<CapsController>())
                hatLevelBonus.transform.GetComponent<CapsController>().rotateCap = true;
            //hatLevelBonus = hat;
        }
    }
    private void AddHatMenu(GameObject arr, GameObject prefab)
    {
        if (prefab == null) prefab = hatLevelBonus;
        foreach (Transform child in arr.transform) Lean.Pool.LeanGameObjectPool.Destroy(child.gameObject);
        GameObject hat = Lean.Pool.LeanPool.Spawn(prefab, transform.position, Quaternion.identity);
        hat.layer = 5;
        hat.transform.GetChild(0).gameObject.layer = 5;
        LeanTween.scale(hat, new Vector3(800, 800, 800), 0);
        LeanTween.moveLocal(hat, new Vector3(0, 0, -500), 0);
        hat.transform.SetParent(arr.transform);
    }
    private void SaveHatEquip(int id, List<int> typeHats, bool noEquip = false)
    {
        store.typeStore = "hats";
        PlayerPrefs.SetInt("Cap_" + id, 1);
        
        if (!noEquip) PlayerPrefs.SetInt("capPlayer", id);
        foreach (GameObject el in store.hats)
        {
            ScrollElement hat = el.transform.GetComponent<ScrollElement>();
            if (hat.IdCap == id)
            {
                hat.ClearBtn();
                hat.AddEquip();
                if (!noEquip) hat.ActiveCAp();
            }
        }
        foreach (int el in typeHats)
        {
            if (el == id)
            {
                typeHats.Remove(el);
                return;
            }
        }
    }
    public void SaveHatContinue()
    {
        SaveHatEquip(hatBonusID, bonusHats, true);
    }
    public void AddBonusCoins()
    {
        Coins.AddLevel(BonusCoins);
    }
    public void OpenSurpriceAmount(int amount)
    {
        BonusCoinsText.text = "+" + amount.ToString();
        BonusCoins = amount;
        animatorSurprice.Play("OnCoinsSurprice");
    }
}
