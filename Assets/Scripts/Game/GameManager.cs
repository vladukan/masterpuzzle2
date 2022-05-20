using UnityEngine;
using UnityEngine.EventSystems;
public class GameManager : MonoBehaviour
{
    public GameObject Bullet;
    public GameObject BulletEnemy;
    public static GameManager Instance;
    public float timecount;
    public GameObject LLL, RRR;
    private Touch touch;
    private UiManager ui;
    private float beginTime;
    private void Awake()
    {
        if (!Instance) Instance = this;
        ui = FindObjectOfType<UiManager>();
    }
    private void FixedUpdate()
    {
        //MouseClick();
        //SensorTouch();
    }
    public void killcount()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            UiManager.Instance.levelCompleted();
            GameObject pc = GameObject.FindGameObjectWithTag("Player");
            pc.GetComponent<PlayerController>().DanceForWin();
            pc.GetComponent<PlayerController>().enabled = false;
        }
    }
    private void SensorTouch()
    {

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0 && !EventSystem.current.currentSelectedGameObject)
            {
                touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began: beginTime = Time.time; break;
                    case TouchPhase.Ended: shoot(Time.time); break;
                    case TouchPhase.Canceled: shoot(Time.time); break;
                }
            }
        }
    }
    private void MouseClick()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor ||
            Application.platform == RuntimePlatform.WindowsPlayer)
        {
            if (Input.GetMouseButtonDown(0)) beginTime = Time.time;
            if (Input.GetMouseButtonUp(0) && !EventSystem.current.currentSelectedGameObject)
                shoot(Time.time);
        }
    }
    public void shoot(float time)
    {
        //print(time - beginTime);
        if (time - beginTime < 0.05f) return;
        if (ui.storeScreen.activeSelf ||
        ui.gameMenu.activeSelf ||
        ui.levelFailPanel.activeSelf ||
        ui.levelNotLoad.activeSelf ||
        ui.outOfAmmoMenu.activeSelf ||
        ui.giftMenu.activeSelf ||
        ui.levelCompletedPanel.activeSelf) return;
        else
        {
            //PlayerController[] pc = FindObjectsOfType<PlayerController>();
            //foreach (PlayerController element in pc) element.Shoot();
        }
    }
    const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";
    public string GenerateString(int num = 8)
    {
        string str = "";
        for (int i = 0; i < num; i++) str += glyphs[Random.Range(0, glyphs.Length)];
        return str;
    }
}
