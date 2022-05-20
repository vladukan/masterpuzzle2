using UnityEngine;
using System.Linq;
public class PlayerController : MonoBehaviour
{
    public float direction = 1f;
    public bool UiStore = false;
    public GameObject Cap, leftArm, RightArm, LeftGun, RightGun, LLL, RRR;
    public bool leftArmWork, RightArmWork;
    public bool active = false;
    private float angleZ;
    private float mousex;
    private bool isEmpty = false;
    public GameObject workingarm;
    public bool isKill = false;
    public delegate void ChangeBullets(int bullets);
    public event ChangeBullets PlayerBullets;
    public Levels levels;
    private UiManager ui;
    public string idBullet;
    public int countBullets = 3;
    public float LazerMaxLength = 10;
    public int LazerReflections = 10;
    private Camera cameraMain;
    private float beginTime;
    public bool isUpdate = false;
    public bool isDown = false;
    public bool isCollision = false;
    private void Awake()
    {
        levels = FindObjectOfType<Levels>();
        ui = FindObjectOfType<UiManager>();
        if (transform.tag == "Player")
        {
            if (leftArmWork) levels.playerAngleZBegin = leftArm.transform.localEulerAngles.y;
            if (RightArmWork) levels.playerAngleZBegin = RightArm.transform.localEulerAngles.y;
            levels.LazerMaxLength = LazerMaxLength;
            levels.LazerReflections = LazerReflections;
        }
    }
    void Start()
    {
        idBullet = GameManager.Instance.GenerateString();
        cameraMain = Camera.main;
        active = false;
        if (leftArmWork)
        {
            LeftGun.SetActive(true);
            RightGun.SetActive(false);
            workingarm = leftArm;
            XandYofPresent(workingarm);
        }
        else if (RightArmWork)
        {
            RightGun.SetActive(true);
            LeftGun.SetActive(false);
            workingarm = RightArm;
            XandYofPresent(workingarm);
        }
        else
        {
            refbotpostions();
            RightGun.SetActive(false);
            LeftGun.SetActive(false);
            GetComponent<Animator>().enabled = true;
        }
    }
    void OnEnable()
    {
        Lean.Touch.LeanTouch.OnFingerDown += HandleFingerDown;
        Lean.Touch.LeanTouch.OnFingerUp += HandleFingerUp;
        Lean.Touch.LeanTouch.OnFingerUpdate += HandleFingerUpdate;
    }
    void OnDisable()
    {
        Lean.Touch.LeanTouch.OnFingerDown -= HandleFingerDown;
        Lean.Touch.LeanTouch.OnFingerUp -= HandleFingerUp;
        Lean.Touch.LeanTouch.OnFingerUpdate -= HandleFingerUpdate;
    }
    void HandleFingerDown(Lean.Touch.LeanFinger finger)
    {
        if (UiStore || ui.gameMenu.activeSelf || isKill) return;
        if (isEmpty)
        {
            active = false;
            return;
        }
        isUpdate = true;
        isDown = true;
        active = true;
    }
    void HandleFingerUp(Lean.Touch.LeanFinger finger)
    {
        if (!isDown) return;
        isUpdate = false;
        if (isEmpty || isKill) return;
        Shoot();
    }
    void HandleFingerUpdate(Lean.Touch.LeanFinger finger)
    {
        if (isUpdate) MoveArm();
    }
    private void MoveArm()
    {
        if (isCollision) active = false; else active = true;
        if (workingarm == null) return;
        float angle;
        if (transform.tag == "Player")
        {
            Vector3 dir = cameraMain.WorldToScreenPoint(workingarm.transform.position) - Input.mousePosition;
            angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            levels.playerAngleZRunTime = angle;
        }
        else
        {
            angle = levels.playerAngleZRunTime;
            angle -= angleZ;
        }
        workingarm.transform.localRotation = Quaternion.Euler(new Vector3(-90f, 0, angle * direction));
    }
    public void Shoot()
    {
        //if (time - beginTime < 0.03f) return;
        if (ui.storeScreen.activeSelf ||
        ui.gameMenu.activeSelf ||
        ui.levelFailPanel.activeSelf ||
        ui.levelNotLoad.activeSelf ||
        ui.outOfAmmoMenu.activeSelf ||
        ui.giftMenu.activeSelf ||
        ui.levelCompletedPanel.activeSelf || isEmpty) return;
        PlayerBullets += OnChangeBullets;
        OnChangeBullets(countBullets);
    }
    public void sendbullet(GameObject gun)
    {
        active = false;
        LineRenderer temp;
        if (transform.tag == "Player") temp = gun.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<LineRenderer>();
        else temp = gun.GetComponent<LineRenderer>();
        Vector3[] temparray = new Vector3[temp.positionCount];
        temp.GetPositions(temparray);
        Vector3[] arr = temparray.ToArray<Vector3>();
        GameObject bullet;
        if (transform.tag == "Player")
            bullet = Lean.Pool.LeanPool.Spawn(GameManager.Instance.Bullet, arr[0], Quaternion.identity);
        else
            bullet = Lean.Pool.LeanPool.Spawn(GameManager.Instance.BulletEnemy, arr[0], Quaternion.identity);
        bullet.GetComponent<BulletData>().idBullet = idBullet;
        bullet.GetComponent<BulletData>().ListOfPoints = arr;
        bullet.transform.SetParent(transform);
        if (AudioManager.instance) AudioManager.instance.Play("Bullet");
    }
    public void DanceForWin()
    {
        refbotpostions();
        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().SetTrigger("Dance");
        LeftGun.SetActive(false);
        RightGun.SetActive(false);
        active = false;
    }
    public void XandYofPresent(GameObject workingarm)
    {
        if (transform.tag == "Enemy") angleZ = levels.playerAngleZBegin - workingarm.transform.localEulerAngles.y;
        //active = true;
    }
    public void refbotpostions()
    {
        GetComponent<Animator>().enabled = false;
        if (GameManager.Instance)
        {
            LLL = GameManager.Instance.LLL;
            RRR = GameManager.Instance.RRR;
        }
        leftArm.name = LLL.transform.GetChild(0).name;
        RightArm.name = RRR.transform.GetChild(0).name;
        GameObject lll = leftArm.transform.parent.gameObject;
        GameObject rrr = RightArm.transform.parent.gameObject;
        lll.transform.localPosition = LLL.transform.localPosition;
        lll.transform.localEulerAngles = LLL.transform.localEulerAngles;
        lll.transform.GetChild(0).transform.localPosition = LLL.transform.GetChild(0).transform.localPosition;
        lll.transform.GetChild(0).transform.localEulerAngles = LLL.transform.GetChild(0).transform.localEulerAngles;
        lll.transform.GetChild(0).GetChild(0).transform.localPosition =
            LLL.transform.GetChild(0).GetChild(0).transform.localPosition;
        lll.transform.GetChild(0).GetChild(0).transform.localEulerAngles =
            LLL.transform.GetChild(0).GetChild(0).transform.localEulerAngles;
        rrr.transform.localPosition = RRR.transform.localPosition;
        rrr.transform.localEulerAngles = RRR.transform.localEulerAngles;
        rrr.transform.GetChild(0).transform.localPosition = RRR.transform.GetChild(0).transform.localPosition;
        rrr.transform.GetChild(0).transform.localEulerAngles = RRR.transform.GetChild(0).transform.localEulerAngles;
        rrr.transform.GetChild(0).GetChild(0).transform.localPosition =
            RRR.transform.GetChild(0).GetChild(0).transform.localPosition;
        rrr.transform.GetChild(0).GetChild(0).transform.localEulerAngles =
            RRR.transform.GetChild(0).GetChild(0).transform.localEulerAngles;
    }
    private void CheckEnemies()
    {
        if (ui.activeBonusLevel && !ui.pause)
        {
            ui.LevelBonusCompleted();
            return;
        }
        if (GameObject.FindGameObjectsWithTag("Enemy").Length > 0 && isEmpty)
            UiManager.Instance.OutOfAmmo();
    }
    private void OnChangeBullets(int bullets)
    {
        if (isCollision) return;
        if (transform.tag == "Enemy") countBullets++;
        if (bullets <= 1)
        {
            countBullets = countBullets - 1;
            isEmpty = true;
            ui.pauseGame.SetActive(false);
            if (transform.tag == "Player") levels.SetBullets(countBullets, false);
            if (transform.tag == "Player") Invoke("CheckEnemies", 1.5f);
        }
        else
        {
            countBullets = countBullets - 1;
            if (transform.tag == "Player") levels.SetBullets(countBullets, false);
        }
        if (RightArmWork) sendbullet(RightGun);
        if (leftArmWork) sendbullet(LeftGun);
    }
}
