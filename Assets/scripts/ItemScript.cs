using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEditor.Build;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemScript : MonoBehaviour
{
    public Camera playerCamera;
    public Transform cameraHolder;
    public HandsController handsController;
    public float pickupRange = 2f;
    [Header("Голоограми")]
    public Material GologramMaterial;
    public Material GologramRedMaterial;
    public GameObject GologramHolder;


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

    private Transform HoveredScrew;

    private Collider HoveredScrewCollider;

    private int Yrot = 0;
    private int Xrot = 0;
    private int Zrot = 0;

    private bool Placeble = true;



    private int CurrentScrew = 0;
    private void Update()
    {
        

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Yrot += 45;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Xrot += 45;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Zrot += 45;
        }




        if (scroll > 0f)
        {
            CurrentScrew++;
        }
        else if (scroll < 0f)
        {
            CurrentScrew--;
        }

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
                else if (heldItem.CompareTag("Part") && Placeble)
                {
                    AplyPart();
                }
            }

        }



        if (PartGologram == null && heldItem != null && heldItem.CompareTag("Part"))
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
            var rb = PartGologram.GetComponent<Rigidbody>();
            rb.detectCollisions = false;

        }
        if (PartGologram != null) 
        {

            
            if (!PartGologram.GetComponent<GologramScript>().GetPermissonToPlace())
            {
                foreach (Renderer renderer in PartGologram.transform.Find("Meshes").GetComponentsInChildren<Renderer>())
                {
                    if (renderer != null)
                    {
                        renderer.material = GologramRedMaterial;
                    }

                }
                Placeble = false;
            }
            else
            {
                
                foreach (Renderer renderer in PartGologram.transform.Find("Meshes").GetComponentsInChildren<Renderer>())
                {
                    if (renderer != null)
                    {
                        renderer.material = GologramMaterial;
                    }

                }
                Placeble = true;
            }


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
                        ////, hit.transform.position, heldItem.transform.rotation
                        //PartGologram = Instantiate(heldItem);



                        //foreach (Renderer renderer in PartGologram.transform.Find("Meshes").GetComponentsInChildren<Renderer>())
                        //{
                        //    if (renderer != null)
                        //    {
                        //        renderer.material = GologramMaterial;
                        //    }

                        //}
                        //var rb = PartGologram.GetComponent<Rigidbody>();
                        //rb.detectCollisions = false;






                    }
                    else
                    {
                        if (hit.collider.CompareTag("screw"))
                        {

                            if (CurrentScrew >= PartGologram.GetComponent<Partscript>().Holes.Length) { CurrentScrew = 0; }

                            if (CurrentScrew < 0) { CurrentScrew = PartGologram.GetComponent<Partscript>().Holes.Length - 1; }

                            Vector3 Offset = PartGologram.transform.position - PartGologram.GetComponent<Partscript>().Holes[CurrentScrew].transform.position;
                            PartGologram.transform.position = hit.collider.GetComponent<Transform>().position + Offset;
                            PartGologram.transform.rotation = hit.collider.GetComponent<Transform>().rotation * Quaternion.Euler(Xrot, Yrot, Zrot);
                            //PartGologram.GetComponent<Partscript>().Holes[CurrentScrew].transform.position;
                            HoveredScrew = hit.collider.transform;
                            
                        }
                       
                        else
                        {
                            PartGologram.transform.position = hit.point;
                            PartGologram.transform.rotation = Quaternion.Euler(Xrot, Yrot, Zrot);
                        }
                        //PartGologram.transform.position = Vector3.forward * 1f - new Vector3(0f, 0.3f, 0f);

                    }
                }
                else if (PartGologram != null)
                {
                    PartGologram.transform.position = GologramHolder.transform.position;
                    PartGologram.transform.rotation = Quaternion.Euler(Xrot, Yrot, Zrot);
                }
            }
        }

        void Pickup()
        {
            Ray ray = new Ray(cameraHolder.position, cameraHolder.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
            {
                Debug.Log(hit.transform);
                if (hit.collider.CompareTag("Item") || hit.collider.CompareTag("Pistol") || hit.collider.CompareTag("screw") || hit.collider.CompareTag("Part"))
                {

                    heldItem = hit.collider.gameObject;

                    Rigidbody rb = heldItem.GetComponent<Rigidbody>();
                    if (rb == null && hit.collider.CompareTag("screw"))

                    {
                        Debug.Log("Yeaah");
                        hit.collider.AddComponent<Rigidbody>();
                        rb = heldItem.GetComponent<Rigidbody>();
                    }

                    if (rb != null)
                    {
                        rb.isKinematic = true;
                        rb.detectCollisions = false;
                    }

                    heldItem.transform.SetParent(handsController.transform);

                    if (hit.collider.CompareTag("Pistol") || hit.transform.CompareTag("Pistol"))
                    {
                        
                        HandsController controller = handsController.GetComponent<HandsController>();
                        hit.collider.GetComponent<Weapon>().Hands = controller;
                        hit.collider.GetComponent<Weapon>().playerCamera = playerCamera;
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

                        heldItem.AddComponent<GologramScript>();

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
            if (heldItem.GetComponent<GologramScript>() != null)
            {
                Destroy(heldItem.GetComponent<GologramScript>());
            }
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

                    heldItem.transform.SetParent(hit.collider.gameObject.GetComponentInParent<Transform>());
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

        void AplyPart()
        {
            
            heldItem.transform.SetParent(null);

            Rigidbody rb = heldItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                if (HoveredScrew == null)
                {
                    rb.isKinematic = false;

                }
               
               

                //HoveredScrew.GetComponent<Collider>().isTrigger = true;

                rb.detectCollisions = true;
                heldItem.transform.position = PartGologram.transform.position;
                heldItem.transform.rotation = PartGologram.transform.rotation;
                heldItem.transform.SetParent(HoveredScrew);
                
                HoveredScrew = null;

            }
            if (heldItem.GetComponent<GologramScript>() != null)
            {
                Destroy(heldItem.GetComponent<GologramScript>());
            }
            Destroy(PartGologram);
            PartGologram = null;
            heldItem = null;

            HandsController controller = handsController.GetComponent<HandsController>();
            if (controller != null)
            {
                controller.Anim.SetTrigger("End");
                Debug.Log("Ended");

            }
            
            





        }
    }
}
