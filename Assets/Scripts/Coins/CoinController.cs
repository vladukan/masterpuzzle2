using UnityEngine;
public class CoinController : MonoBehaviour
{
    [SerializeField] GameObject coinNumPrefab;
    private CoinsManager coinsManager;
    private UiManager ui;
    private AudioSource cameraAudio;
    void Start()
    {
        LeanTween.rotateAround(gameObject, Vector3.up, 360, 2f).setLoopClamp();
        coinsManager = FindObjectOfType<CoinsManager>();
        ui = FindObjectOfType<UiManager>();
        cameraAudio = Camera.main.transform.GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet") DestroyCoins(other.transform.position);
    }
    private void DestroyCoins(Vector3 pos)
    {
        if (!ui.activeBonusLevel) coinsManager.AddCoins(transform.position, 10);
        else
        {
            cameraAudio.PlayOneShot(coinsManager.soundCoin);
            coinsManager.levelCoins += 10;
        }
        Lean.Pool.LeanGameObjectPool.Destroy(Instantiate(coinNumPrefab, pos, Quaternion.identity), 1f);
        Lean.Pool.LeanGameObjectPool.Destroy(gameObject);
    }
}
