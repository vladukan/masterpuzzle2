using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{
    public List<GameObject> botsTokill;
    public GameObject blasteffect;
    void Start()
    {
        blasteffect.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Mirror") return;
        if (collision.gameObject.tag == "Bullet")
        {
            Lean.Pool.LeanGameObjectPool.Destroy(collision.gameObject);
            KillThisBot();
        }
        else
        {
            KillThisBot();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Mirror") return;
        if (other.gameObject.tag == "Bullet")
        {
            Lean.Pool.LeanGameObjectPool.Destroy(other.gameObject);
            KillThisBot();
        }
        else
        {
            KillThisBot();
        }
    }
    public void KillThisBot()
    {
        blasteffect.transform.parent = null;
        blasteffect.SetActive(true);

        for (int i = 0; i < botsTokill.Count; i++)
        {
            if (botsTokill[i] == null) break;
            if (botsTokill[i].tag == "Wood") botsTokill[i].GetComponent<wood>().DestroyWoodBomb();
            else botsTokill[i].GetComponent<PlayerCollision>().KillThisBot();
        }
        Destroy(gameObject);
    }
}
