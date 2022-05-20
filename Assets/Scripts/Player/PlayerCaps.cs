using UnityEngine;
public class PlayerCaps : MonoBehaviour
{
    private Levels levels;
    public PlayerController player;
    private GameObject capPlayer;
    public int cap = 0;
    void Start()
    {
        player = GetComponent<PlayerController>();
        if (!PlayerPrefs.HasKey("capPlayer")) PlayerPrefs.SetInt("capPlayer", 0);
        if (!player.UiStore)SetCap();
    }
    public void SetCap()
    {
        //cap = 85;
        cap = PlayerPrefs.GetInt("capPlayer");
        if (cap > 0)
        {
            GameObject prefab = Resources.Load<GameObject>($"Caps/Cap_{cap}") as GameObject;
            if (prefab == null)
            {
                Debug.Log("Load Gun fail");
                return;
            }
            //print("Set Cap " + cap);
            if (!player) player = GetComponent<PlayerController>();
            capPlayer = Lean.Pool.LeanPool.Spawn(prefab, player.Cap.transform);
            capPlayer.transform.SetParent(player.Cap.transform);
            if (player.UiStore)
            {
                capPlayer.layer = 5;
                capPlayer.transform.GetChild(0).gameObject.layer = 5;
            }
            if (capPlayer.GetComponent<CapsController>()) Destroy(capPlayer.GetComponent<CapsController>());
        }
        else ClearCap();
    }
    public void ClearCap()
    {
        Lean.Pool.LeanGameObjectPool.Destroy(capPlayer);
    }
}
