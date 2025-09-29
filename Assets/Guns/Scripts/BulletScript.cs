using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 3f;
    public float lifeTime = 3f;
    private void Start()
    {
        Destroy(gameObject, lifeTime);
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * speed;
        }



    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Shooted in" + collision.gameObject.name);
        Destroy(gameObject);
    }
}
