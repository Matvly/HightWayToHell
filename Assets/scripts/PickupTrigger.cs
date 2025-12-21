using UnityEngine;

public class PickupTrigger : MonoBehaviour
{
    public GameObject item; // само оружие

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ItemScript.Instance.SetNearbyItem(item);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ItemScript.Instance.ClearNearbyItem(item);
        }
    }
}
