using UnityEngine;
public class BombReal : MonoBehaviour
{
    public GameObject blasteffect;
    public float radiusBomb = 1f;
    public float forceBomb = 10f;
    void Start()
    {
        blasteffect.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Mirror") return;
        Lean.Pool.LeanGameObjectPool.Destroy(collision.gameObject);
        Kill();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Mirror") return;
        Lean.Pool.LeanGameObjectPool.Destroy(collision.gameObject);
        Kill();
    }
    public void Kill()
    {
        blasteffect.transform.parent = null;
        blasteffect.SetActive(true);
        Collider[] objects = Physics.OverlapSphere(transform.position, radiusBomb);
        foreach (Collider el in objects)
        {
            Rigidbody rb = el.attachedRigidbody;
            if (rb) rb.AddExplosionForce(forceBomb, transform.position, radiusBomb);
            Vector3 force = transform.position + el.transform.position;
            if (el.tag == "Enemy" || el.tag == "Player")
            {
                if (el.transform.GetComponent<PlayerCollision>())
                    el.transform.GetComponent<PlayerCollision>().KillThisBot();
                if (el.transform.GetComponent<Rigidbody>())
                {
                    el.transform.GetComponent<Rigidbody>().isKinematic = false;
                    el.transform.GetComponent<Rigidbody>().AddForce(force * 1000 * Time.deltaTime, ForceMode.Impulse);
                }
            }
            if (el.tag == "Wood")
                if (el.transform.GetComponent<wood>()) el.transform.GetComponent<wood>().DestroyWood(force / 4);
            if (el.tag == "Bomb")
                if (el.transform.GetComponent<BombReal>()) el.transform.GetComponent<BombReal>().Kill();
            if (el.tag == "Glass")
                if (el.transform.GetComponent<BreakableWindow>()) 
                el.transform.GetComponent<BreakableWindow>().breakWindow();
        }
        Destroy(gameObject);
    }
}
