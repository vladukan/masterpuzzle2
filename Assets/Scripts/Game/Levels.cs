using UnityEngine;
using System.Collections;
public class Levels : MonoBehaviour
{
    private UiManager ui;
    private GameManager game;
    private int maxLevel = 201;
    private int maxLevelBonus = 8;
    public float playerAngleZBegin;
    public float playerAngleZRunTime;
    public GameObject[] imageBullets;
    public float LazerMaxLength = 10;
    public int LazerReflections = 10;
    private GameObject prefab;
    void Start()
    {
        game = FindObjectOfType<GameManager>();
        ui = FindObjectOfType<UiManager>();
    }
    public void LoadLevel(int level, bool pause = false)
    {
        string file;
        if (ui.activeBonusLevel) file = $"Bonuses/Level_{level}"; else file = $"Levels/Level_{level}";
        prefab = Resources.Load<GameObject>(file) as GameObject;
        if (prefab == null)
        {
            Debug.Log("Load level fail");
            LoadRandomLevel();
            //ui.ErrorLoadLevel();
            //return;
        }
        ui.bulletsNumber.SetActive(true);
        if (!ui.gameMenu.activeSelf) ui.pauseGame.SetActive(true);
        ui.pause = pause;
        SetBullets(0, true);
        Debug.Log("Load level success");
        GameObject obj = Lean.Pool.LeanPool.Spawn(prefab, transform.position, Quaternion.identity);
        obj.transform.SetParent(transform);
    }
    public void ClearLevel(bool reload, int level)
    {
        StartCoroutine(ClearLevelCoroutine(reload, level));
    }
    IEnumerator ClearLevelCoroutine(bool reload, int level)
    {
        foreach (Transform child in gameObject.transform) Lean.Pool.LeanGameObjectPool.Destroy(child.gameObject);
        ClearBullets();
        yield return new WaitForEndOfFrame();
        if (reload) LoadLevel(level);
    }
    public void ClearBullets()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in objects) Lean.Pool.LeanGameObjectPool.Destroy(bullet);
        objects = GameObject.FindGameObjectsWithTag("SuperBullet");
        foreach (GameObject bullet in objects) Lean.Pool.LeanGameObjectPool.Destroy(bullet);
    }
    public void SetBullets(int value, bool type)
    {
        if (value > imageBullets.Length) return;
        for (int i = 0; i < imageBullets.Length - value; i++) imageBullets[i].SetActive(type);
    }
    private void LoadRandomLevel()
    {
        int randomLevel = 1;
        string file;
        if (PlayerPrefs.HasKey("randomLevel")) randomLevel = PlayerPrefs.GetInt("randomLevel");
        else
        {
            if (ui.activeBonusLevel) randomLevel = Random.Range(1, maxLevelBonus);
            else randomLevel = Random.Range(10, maxLevel);
            PlayerPrefs.SetInt("randomLevel", randomLevel);
        }
        if (ui.activeBonusLevel) file = $"Bonuses/Level_{randomLevel}";
        else file = $"Levels/Level_{randomLevel}";
        prefab = Resources.Load<GameObject>(file) as GameObject;
    }
}
