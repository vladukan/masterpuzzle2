using UnityEngine;
public class SpinnerController : MonoBehaviour
{
    public int force = 1000;
    private int directX = 1;
    private int directY = 1;
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet") HitForce(collision.transform.position);
    }
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            HitForce(collision.transform.position);
            Lean.Pool.LeanGameObjectPool.Destroy(collision.gameObject);
        }
    }
    public void HitForce(Vector3 obj)
    {
        print("triger");

        if (transform.position.x > obj.x) directX = 1; else directX = -1;
        if (transform.position.y > obj.y) directY = 1; else directY = -1;

        GetComponent<Rigidbody>().AddTorque(Vector3.forward * force * directX * directY);
    }
}
