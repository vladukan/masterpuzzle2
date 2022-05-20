using UnityEngine;
public class RopeMetalCollision : MonoBehaviour
{

    private int directX = 1;
    private int directY = 1;
    public Vector3 thisdirection = new Vector3(1, 0, 0);
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Bullet") {
            Force(other.transform.position);
            Lean.Pool.LeanGameObjectPool.Destroy(other.gameObject);
        }
    }

    public void Force(Vector3 obj)
    {
        if (transform.position.x > obj.x) directX = 1; else directX = -1;
        if (transform.position.y > obj.y) directY = 1; else directY = -1;
        Vector3 pos = new Vector3(thisdirection.x * directX, thisdirection.y * directY, thisdirection.z);
        GetComponent<Rigidbody>().AddForce(pos * 5000 * Time.deltaTime, ForceMode.Force);
    }

}
