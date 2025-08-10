using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public Transform cameraHolder;
    public Transform hands;
    public float pickupRange = 2f;
    private GameObject heldItem;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldItem == null)
            {
                Pickup();
            }

        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (heldItem != null)
            {
                Drop();
            }
        }
    }

    void Pickup()
    {
        Ray ray = new Ray(cameraHolder.position, cameraHolder.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            if (hit.collider.CompareTag("Item"))
            {
                heldItem = hit.collider.gameObject;

                Rigidbody rb = heldItem.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.detectCollisions = false;
                }
                heldItem.transform.SetParent(hands);
                Vector3 offset = hands.position + hands.forward * 1f;
                heldItem.transform.position = offset;
                heldItem.transform.rotation = hands.rotation;
                heldItem.transform.SetParent(hands);

            }
        } 
    }

    void Drop()
    {
        heldItem.transform.SetParent(null);

        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;

            
            rb.AddForce(cameraHolder.forward * 6f, ForceMode.Impulse);
        }

        heldItem = null;

    }
}
