using UnityEngine;

public class GologramScript : MonoBehaviour
{
    public bool Colliding = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider other)
    {
        Colliding = true;
    }

    void OnTriggerExit(Collider other)
    {
        Colliding = false;
    }
}
