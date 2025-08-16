using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float lifeTime = 3f;
    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Shooted in" + collision.gameObject.name);
        Destroy(gameObject);
    }
}
