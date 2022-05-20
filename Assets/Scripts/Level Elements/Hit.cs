using UnityEngine;
public class Hit : MonoBehaviour
{
    private int directX = 1;
    private int directY = 1;
    public Vector3 thisdirection;
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "Wood")HitForce(collision.transform.position);
    }
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "Wood")HitForce(collision.transform.position);
    }
    public void HitForce(Vector3 obj){
        if(transform.position.x>obj.x) directX = 1; else directX = -1;
            if(transform.position.y>obj.y) directY = 1; else directY = -1;
            Vector3 pos = new Vector3(thisdirection.x*directX,thisdirection.y*directY,thisdirection.z);
            GetComponent<Rigidbody>().AddForce(pos * 5000 * Time.deltaTime, ForceMode.Force);
    }
}
