using UnityEngine;

public class CollisionMetalDespawn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("OnDespawn", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDespawn()
    {
        Destroy(gameObject);
    }
}
