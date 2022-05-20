using System.Collections;
using UnityEngine;
using TMPro;
public class UiManager : MonoBehaviour
{
    public GameObject outOfAmmoMenu;
    public GameObject giftMenu;
    public GameObject levelCompletedPanel;
    public GameObject levelFailPanel;
    public GameObject levelNotLoad;
    public GameObject gamePanel;
    public GameObject gameMenu;
    public GameObject storeScreen;
    public GameObject bulletsNumber;
    public GameObject pauseGame;
    public TextMeshProUGUI LevelNumber;
    public static UiManager Instance;
    public int level;
    public int levelBonus;
    public bool activeBonusLevel;
    public bool pause;
    private Levels levels;
    private CoinsManager coins;
    private SurpriceManager surprice;
    private StoreManager store;
    private RewardAds ads;
    private ReviewController review;
    private void Awake()
    {
        if (!Instance) Instance = this;
    }
    void Start()
    {
        SetPrefs();
        levels = FindObjectOfType<Levels>();
        coins = GetComponent<CoinsManager>();
        surprice = GetComponent<SurpriceManager>();
        store = GetComponent<StoreManager>();
        ads = FindObjectOfType<RewardAds>();
        review = FindObjectOfType<ReviewController>();
        ClearMenuElements();
        gameMenu.SetActive(true);
        gamePanel.SetActive(true);
    }
    public void levelCompleted()
    {
        StartCoroutine(win());
    }
    public void LevelBonusCompleted()
    {
        StartCoroutine(winBonus());
    }
    public void LevelFail()
    {
        StartCoroutine(loss());
    }
    public void OutOfAmmo()
    {
        StartCoroutine(noAmmo());
    }
    IEnumerator noAmmo()
    {
        //pause = true;
        ads.CheckReward();
        yield return new WaitForSeconds(0.5f);
        print("noAmmo");
        surprice.bonusHat = false;
        pauseGame.SetActive(false);
        bulletsNumber.SetActive(false);
        outOfAmmoMenu.SetActive(true);
        if (AudioManager.instance) AudioManager.instance.Play("loss");
    }
    public void AddGiftMenu()
    {
        StartCoroutine(gift());
    }
    IEnumerator gift()
    {
        //pause = true;
        yield return new WaitForSeconds(0.001f);
        print("gift");
        pauseGame.SetActive(false);
        giftMenu.SetActive(true);
    }
    IEnumerator loss()
    {
        ads.CheckReward();
        yield return new WaitForSeconds(2f);
        if (!GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isKill) yield break;
        pause = true;
        yield return new WaitForSeconds(0.5f);
        print("lose");
        surprice.bonusHat = false;
        pauseGame.SetActive(false);
        bulletsNumber.SetActive(false);
        levelFailPanel.SetActive(true);
        if (AudioManager.instance) AudioManager.instance.Play("loss");
    }
    IEnumerator win()
    {
        pauseGame.SetActive(false);
        bulletsNumber.SetActive(false);
        ads.CheckReward();
        CapsController hat = FindObjectOfType<CapsController>();
        if (FindObjectOfType<CapsController>()) hat.SetHat();

        yield return new WaitForSeconds(2f);
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isKill) yield break;
        pause = true;
        levels.ClearBullets();
        levelFailPanel.SetActive(false);
        outOfAmmoMenu.SetActive(false);
        if (surprice.bonusHat)
        {
            giftMenu.SetActive(true);
            surprice.CreateHatMenu();
            surprice.bonusHat = false;
        }

        yield return new WaitForSeconds(1f);
        print("win");
        levelCompletedPanel.SetActive(true);
        coins.SetBtnCoins();
        coins.BlockBtnLevel.SetActive(true);
        coins.BlockBtnBonus.SetActive(false);
        coins.BtnCasino.SetActive(true);
        if (AudioManager.instance) AudioManager.instance.Play("WIn");
        if (Particaleffect.instance) StartCoroutine(Particaleffect.instance.playpop());
        surprice.StartFill();
        if (level >= 10 && level % 10 == 0)
        {
            activeBonusLevel = true;
            PlayerPrefs.SetInt("activeBonusLevel", 1);
        }
        level++;
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.DeleteKey("randomLevel");
    }
    IEnumerator winBonus()
    {
        pauseGame.SetActive(false);
        bulletsNumber.SetActive(false);
        ads.CheckReward();
        yield return new WaitForSeconds(2f);
        pause = true;
        levels.ClearBullets();
        print("winBonus");
        activeBonusLevel = false;
        levelCompletedPanel.SetActive(true);
        coins.SetBtnCoins();
        coins.BlockBtnLevel.SetActive(false);
        coins.BlockBtnBonus.SetActive(true);
        if (AudioManager.instance) AudioManager.instance.Play("WIn");
        if (Particaleffect.instance) StartCoroutine(Particaleffect.instance.playpop());
        PlayerPrefs.SetInt("activeBonusLevel", 0);
        levelBonus++;
        PlayerPrefs.SetInt("levelBonus", levelBonus);
        PlayerPrefs.DeleteKey("randomLevel");
    }
    public void NextLevel(bool skipType = false)
    {
        gameMenu.SetActive(false);
        outOfAmmoMenu.SetActive(false);
        levelCompletedPanel.SetActive(false);
        levelFailPanel.SetActive(false);
        giftMenu.SetActive(false);
        surprice.screenSurprice.SetActive(false);
        if (skipType) level++;
        coins.levelCoins = 0;
        if (level == 11) review.RequestReview();
        if (level == 11 && !activeBonusLevel) review.ShowReview();
        if (activeBonusLevel)
        {
            LevelNumber.text = "BONUS";
            levels.ClearLevel(true, levelBonus);
        }
        else
        {
            LevelNumber.text = "LEVEL " + level;
            levels.ClearLevel(true, level);
        }
    }
    public void SkipFromOutOfAmmo()
    {
        pause = false;
        pauseGame.SetActive(true);
        outOfAmmoMenu.SetActive(false);
        bulletsNumber.SetActive(true);
    }
    public void ErrorLoadLevel()
    {
        levelNotLoad.SetActive(true);
        gameMenu.SetActive(false);
    }
    public void BackToMenu()
    {
        levelCompletedPanel.SetActive(false);
        levelFailPanel.SetActive(false);
        levels.ClearLevel(false, level);
        surprice.bonusHat = false;
        ClearMenuElements();
        gameMenu.SetActive(true);
        levels.ClearLevel(true, level);
    }
    private void ClearMenuElements()
    {
        levelCompletedPanel.SetActive(false);
        levelFailPanel.SetActive(false);
        levelNotLoad.SetActive(false);
        bulletsNumber.SetActive(false);
        pauseGame.SetActive(false);
    }
    public void ResetLevels()
    {
        print("reset");
        PlayerPrefs.SetInt("level", 1);
        PlayerPrefs.SetInt("levelBonus", 1);
        PlayerPrefs.SetInt("activeBonusLevel", 0);
        level = 1;
        levelBonus = 1;
        activeBonusLevel = false;
        LevelNumber.text = "LEVEL " + level;
        BackToMenu();
    }
    public void Setlevel(int value)
    {
        activeBonusLevel = false;
        if (level + value < 1) return;
        level += value;
        PlayerPrefs.SetInt("level", level);
        LevelNumber.text = "LEVEL " + level;
        BackToMenu();
    }
    public void Store(bool type)
    {
        storeScreen.SetActive(type);
        store.ContainerElements(type);
        store.player.ClearCap();
        store.player.SetCap();
    }
    private void SetPrefs()
    {
        if (!PlayerPrefs.HasKey("level")) PlayerPrefs.SetInt("level", 1);
        if (!PlayerPrefs.HasKey("levelBonus")) PlayerPrefs.SetInt("levelBonus", 1);
        if (!PlayerPrefs.HasKey("activeBonusLevel")) PlayerPrefs.SetInt("activeBonusLevel", 0);
        level = PlayerPrefs.GetInt("level");
        levelBonus = PlayerPrefs.GetInt("levelBonus");
        if (PlayerPrefs.GetInt("activeBonusLevel") == 0)
        {
            activeBonusLevel = false;
            LevelNumber.text = "LEVEL " + level;
        }
        else
        {
            activeBonusLevel = true;
            LevelNumber.text = "BONUS";
        }
    }
}
