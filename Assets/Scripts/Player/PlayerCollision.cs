using UnityEngine;
public class PlayerCollision : MonoBehaviour
{
    private Collider[] incolliders;
    private Rigidbody[] rigidbodies;
    private BoxCollider[] boxColliders;
    private PlayerController player;
    private CoinsManager coins;
    void Start()
    {
        player = GetComponent<PlayerController>();
        coins = FindObjectOfType<CoinsManager>();
        removerigandcol();
    }
    private void OnCollisionEnter(Collision other)
    {
        //print(other.gameObject.tag);
        if (UiManager.Instance.pause) return;
        if (other.gameObject.tag == "Bullet")
            if (other.gameObject.GetComponent<BulletData>().idBullet != player.idBullet) KillThisBot();
        if (other.gameObject.tag == "Hit" ||
        other.gameObject.tag == "Wood" ||
        other.gameObject.tag == "Glass"
        ) KillThisBot();
    }
    private void OnTriggerEnter(Collider other)
    {
        //print(other.gameObject.tag);
        if (UiManager.Instance.pause) return;
        if (other.gameObject.tag == "Bullet")
            if (other.gameObject.GetComponent<BulletData>().idBullet != player.idBullet) KillThisBot();
        if (other.gameObject.tag == "Hit" ||
        other.gameObject.tag == "Wood" ||
        other.gameObject.tag == "Glass"
        ) KillThisBot();

    }
    private void Collisions(Collision collision)
    {

    }
    public void KillThisBot()
    {
        for (int i = 0; i < incolliders.Length; i++)
        {
            incolliders[i].enabled = true;
        }
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            if(rigidbodies[i])
            rigidbodies[i].isKinematic = false;
        }
        transform.GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Animator>().enabled = false;
        player.isKill = true;
        if (gameObject.tag == "Enemy")
        {
            tag = "Untagged";
            coins.levelCoins += 10;
            GameManager.Instance.killcount();
        }
        else if (gameObject.tag == "Player")
        {
            UiManager.Instance.LevelFail();
        }
        if (player.LeftGun != null)
        {
            //player.LeftGun.GetComponent<LineRenderer>().enabled = false;
            player.LeftGun.SetActive(false);
        }
        if (player.RightGun != null)
        {
            //player.RightGun.GetComponent<LineRenderer>().enabled = false;
            player.RightGun.SetActive(false);
        }
        //Lean.Pool.LeanGameObjectPool.Destroy(GetComponent<PlayerController>());
        if (AudioManager.instance) AudioManager.instance.Play("Hit");
    }
    public void removerigandcol()
    {
        GetComponent<Animator>().enabled = false;
        incolliders = GetComponentsInChildren<Collider>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        boxColliders = GetComponentsInChildren<BoxCollider>();
        for (int i = 0; i < boxColliders.Length; i++)
        {
            boxColliders[i].transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
        }
        for (int i = 0; i < incolliders.Length; i++)
        {
            incolliders[i].enabled = false;
        }
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = true;
        }
        if (player.UiStore) return;

        CapsuleCollider cc = GetComponent<CapsuleCollider>();
        cc.enabled = true;
        cc.height = 1.85f;
        cc.center = new Vector3(0, 0.85f, 0);
    }
}
