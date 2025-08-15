using UnityEditor.Animations;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public Transform cameraHolder;
    public Transform rightHand;
    public Transform leftHand;
    public Transform hands;
    public float pickupRange = 2f;


    [Header("Позиция и поворот для пистолета в руках")]
    public Vector3 pistolLocalPosition;
    public Vector3 pistolLocalRotation;

    [Header("Позиция и поворот рук при пистолете")]
    public Vector3 rightHandLocalPosition;
    public Vector3 leftHandLocalPosition;
    public Vector3 rightHandLocalRotation;
    public Vector3 leftHandLocalRotation;

    private Vector3 defLeftHandPos = new Vector3(-0.330000013f, -0.156000018f, 0.674000025f);
    private Vector3 defLeftHandRotation = new Vector3(356.253998f, 0f, 0f);

    private Vector3 defRightHandPos = new Vector3(0.492000014f, -0.156000018f, 0.674000025f);
    private Vector3 defRightHandRotation = new Vector3(356.253998f, 0f, 0f);

    private GameObject heldItem;

   

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldItem == null)
                Pickup();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (heldItem != null)
                Drop();
        }
    }

    void Pickup()
    {
        Ray ray = new Ray(cameraHolder.position, cameraHolder.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            if (hit.collider.CompareTag("Item") || hit.collider.CompareTag("Pistol"))
            {
                heldItem = hit.collider.gameObject;

                Rigidbody rb = heldItem.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.detectCollisions = false;
                }

                heldItem.transform.SetParent(hands);

                if (hit.collider.CompareTag("Pistol"))
                {


                    
                    heldItem.transform.localPosition = pistolLocalPosition;
                    heldItem.transform.localEulerAngles = pistolLocalRotation;

                    

                    // Настройка рук
                    leftHand.localPosition = leftHandLocalPosition;
                    rightHand.localPosition = defRightHandPos;
                    leftHand.localEulerAngles = leftHandLocalRotation;
                    rightHand.localEulerAngles = defRightHandRotation;

                }
                else
                {
                    // Обычный предмет
                    heldItem.transform.localPosition = Vector3.forward * 1f;
                    heldItem.transform.localRotation = Quaternion.identity;
                }
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

        // Сброс позиции рук (если нужно)
        leftHand.localPosition = defLeftHandPos;
        leftHand.localEulerAngles = defLeftHandRotation;
        rightHand.localPosition = defRightHandPos;
        rightHand.localEulerAngles = defRightHandRotation;
    }
}
