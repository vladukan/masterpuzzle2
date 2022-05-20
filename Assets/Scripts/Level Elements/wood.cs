using UnityEngine;
public class wood : MonoBehaviour
{
    private int directX = 1;
    private int directY = 1;
    private Vector3 pos;
    public Vector3 thisdirection;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Mirror" || other.gameObject.tag == "Wood") return;
        if (other.gameObject.tag == "Bullet" ||
            other.gameObject.tag == "Hit" ||
            other.gameObject.tag == "Enemy")
        {
            if (transform.position.x > other.transform.position.x) directX = 1; else directX = -1;
            if (transform.position.y > other.transform.position.y) directY = 1; else directY = -1;
            pos = new Vector3(thisdirection.x * directX, thisdirection.y * directY, thisdirection.z);
            DestroyWood(pos);
        }
    }
    public void DestroyWood(Vector3 pos)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).transform.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            transform.GetChild(i).transform.gameObject.GetComponent<Rigidbody>().AddForce(pos * 5000 * Time.deltaTime);
        }
        transform.GetComponent<BoxCollider>().enabled = false;
    }
    public void DestroyWoodBomb()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).transform.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            transform.GetChild(i).transform.gameObject.GetComponent<Rigidbody>().
            AddForce(thisdirection * 5000 * Time.deltaTime);
        }
        transform.GetComponent<BoxCollider>().enabled = false;
    }
}
