using UnityEngine;
public class BulletData : MonoBehaviour
{
    public string idBullet;
    private int i;
    public Vector3[] ListOfPoints;
    private void Start()
    {
        i = 0;
    }
    private void FixedUpdate()
    {
        if (transform.position.x < -11f) Lean.Pool.LeanGameObjectPool.Destroy(gameObject);
        if (transform.position.x > 11f) Lean.Pool.LeanGameObjectPool.Destroy(gameObject);
        if (transform.position.y < -3f) Lean.Pool.LeanGameObjectPool.Destroy(gameObject);
        if (transform.position.y > 8) Lean.Pool.LeanGameObjectPool.Destroy(gameObject);
        if (i < ListOfPoints.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, ListOfPoints[i], 15 * Time.deltaTime);
            if (transform.position == ListOfPoints[i]) i++;
        }
        else Lean.Pool.LeanGameObjectPool.Destroy(gameObject);

    }
}
