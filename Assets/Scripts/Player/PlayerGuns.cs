using UnityEngine;
public class PlayerGuns : MonoBehaviour
{
    private PlayerController player;
    public int gun = 0;
    private GameObject gunPlayer;
    void Start()
    {
        player = GetComponent<PlayerController>();
        if (!PlayerPrefs.HasKey("gunPlayer")) PlayerPrefs.SetInt("gunPlayer", 0);
        SetGun();
    }
    public void SetGun()
    {
        gun = PlayerPrefs.GetInt("gunPlayer");
        GameObject prefab = Resources.Load<GameObject>($"Guns/Gun_{gun}") as GameObject;
        if (prefab == null)
        {
            Debug.Log("Load Gun fail");
            return;
        }
        //print("Set Gun " + gun);
        if (player.RightArmWork)
        {
            gunPlayer = Lean.Pool.LeanPool.Spawn(prefab, player.RightGun.transform.GetChild(0).transform);
            gunPlayer.transform.SetParent(player.RightGun.transform.GetChild(0).transform);
        }
        if (player.leftArmWork)
        {
            gunPlayer = Lean.Pool.LeanPool.Spawn(prefab, player.LeftGun.transform.GetChild(0).transform);
            gunPlayer.transform.SetParent(player.LeftGun.transform.GetChild(0).transform);
        }
        if (gunPlayer != null && gunPlayer.transform.GetChild(0).GetComponent<GunsCollisions>())
            gunPlayer.transform.GetChild(0).GetComponent<GunsCollisions>().player = player;
        if (player.UiStore)
        {
            gunPlayer.layer = 5;
            gunPlayer.transform.GetChild(0).gameObject.layer = 5;
            if (gun == 0) foreach (Transform child in gunPlayer.transform.GetChild(0).gameObject.transform) child.gameObject.layer = 5;
        }
    }
    public void ClearGun()
    {
        Lean.Pool.LeanGameObjectPool.Destroy(gunPlayer);
    }
}
