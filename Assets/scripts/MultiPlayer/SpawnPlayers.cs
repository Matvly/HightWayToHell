using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject player;

    public BoxCollider mapCollider;
    
    private void Start()
    {
        Bounds b = mapCollider.bounds;

        Vector3 randomPos = new Vector3(
            Random.Range(b.min.x, b.max.x),
            0f,
            Random.Range(b.min.z, b.max.z)
            );
    }

    
}
