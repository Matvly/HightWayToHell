using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemScript : MonoBehaviour
{
    public Transform cameraHolder;
    public HandsController handsController;
    public float pickupRange = 2f;

    public Material GologramMaterial;
    

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

    private GameObject PartGologram;

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

        if (Input.GetMouseButtonDown(1))
        {
            if (heldItem != null)
            {
                if (heldItem.CompareTag("screw"))
                {
                    Aply();
                }
            }

        }
        if (PartGologram != null)
        {
            PartGologram.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            PartGologram.transform.position = heldItem.transform.position;

        }
            if (heldItem != null)
        {
            if (heldItem.CompareTag("Part"))
            {
                Ray ray = new Ray(cameraHolder.position, cameraHolder.forward);
                if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
                {
                    if (PartGologram == null)
                    {
                        //, hit.transform.position, heldItem.transform.rotation
                        PartGologram = Instantiate(heldItem);


                        
                        foreach (Renderer renderer in PartGologram.transform.Find("Meshes").GetComponentsInChildren<Renderer>())
                        {
                            if (renderer != null)
                            {
                                renderer.material = GologramMaterial;
                            }
                            
                        }
                        foreach (Collider collider in PartGologram.GetComponents<Collider>())
                        {
                            if (collider != null)
                            {
                                collider.enabled = false;
                            }

                        }
                        foreach (Collider collider in PartGologram.transform.Find("Meshes").GetComponentsInChildren<Collider>())
                        {
                            if (collider != null)
                            {
                                collider.enabled = false;
                            }

                        }
                        foreach (Collider collider in PartGologram.transform.GetComponentsInChildren<Collider>())
                        {
                            if (collider != null)
                            {
                                collider.enabled = false;
                            }

                        }



                    }
                    else
                    {
                        
                        //PartGologram.transform.position = Vector3.forward * 1f - new Vector3(0f, 0.3f, 0f);

                    }
                }
            }
        }

        void Pickup()
        {
            Ray ray = new Ray(cameraHolder.position, cameraHolder.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
            {
                if (hit.collider.CompareTag("Item") || hit.collider.CompareTag("Pistol") || hit.collider.CompareTag("screw") || hit.collider.CompareTag("Part"))
                {

                    heldItem = hit.collider.gameObject;

                    Rigidbody rb = heldItem.GetComponent<Rigidbody>();
                    if (rb == null && hit.collider.CompareTag("screw"))

                    {

                        hit.collider.AddComponent<Rigidbody>();
                        rb = heldItem.GetComponent<Rigidbody>();
                    }

                    if (rb != null)
                    {
                        rb.isKinematic = true;
                        rb.detectCollisions = false;
                    }

                    heldItem.transform.SetParent(handsController.transform);

                    if (hit.collider.CompareTag("Pistol"))
                    {

                        HandsController controller = handsController.GetComponent<HandsController>();
                        if (controller != null)
                        {
                            controller.Anim.SetTrigger("Start");
                            Debug.Log("Loaded");

                        }

                        heldItem.transform.localPosition = pistolLocalPosition;
                        heldItem.transform.localEulerAngles = pistolLocalRotation;



                        // Настройка рук
                        handsController.MainLeftHand.localPosition = leftHandLocalPosition;
                        handsController.MainRightHand.localPosition = defRightHandPos;
                        handsController.MainLeftHand.localEulerAngles = leftHandLocalRotation;
                        handsController.MainRightHand.localEulerAngles = defRightHandRotation;

                    }
                    else if (hit.collider.CompareTag("Part"))
                    {
                        heldItem.transform.localPosition = Vector3.forward * 1f - new Vector3(0f, 0.3f, 0f);

                        heldItem.transform.localRotation = Quaternion.identity;


                        // Настройка рук

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
            Destroy(PartGologram);
            PartGologram = null;
            heldItem.transform.SetParent(null);

            Rigidbody rb = heldItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.detectCollisions = true;
                rb.AddForce(cameraHolder.forward * 6f, ForceMode.Impulse);
            }

            heldItem = null;

            HandsController controller = handsController.GetComponent<HandsController>();
            if (controller != null)
            {
                controller.Anim.SetTrigger("End");
                Debug.Log("Ended");

            }


        }

        void Aply()
        {
            Ray ray = new Ray(cameraHolder.position, cameraHolder.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
            {
                if (hit.collider.CompareTag("hole"))
                {

                    heldItem.transform.SetParent(hit.collider.gameObject.transform);
                    heldItem.transform.position = hit.collider.transform.position;
                    heldItem.transform.rotation = hit.collider.transform.rotation;
                    Rigidbody rb = heldItem.GetComponent<Rigidbody>();
                    Collider rbHit = hit.collider.gameObject.GetComponent<Collider>();

                    if (rb != null)
                    {

                        //rb.isKinematic = false;
                        Destroy(rb);
                    }
                    //if (rbHit != null)
                    //{
                    //    rbHit.enabled = false;
                    //}


                    heldItem = null;

                    HandsController controller = handsController.GetComponent<HandsController>();
                    if (controller != null)
                    {
                        controller.Anim.SetTrigger("End");


                    }






                }
            }
        }
    }
}
