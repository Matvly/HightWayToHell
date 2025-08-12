using UnityEngine;

public class HandsController : MonoBehaviour
{
    public Transform player;
    public Transform hands;
    public Vector3 offset;

    private void Update()
    {
        hands.position = player.position + offset;

    }

     


}
