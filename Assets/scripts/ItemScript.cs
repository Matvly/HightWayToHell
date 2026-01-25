using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEditor.Build;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class ItemScript : MonoBehaviour
{
    public static ItemScript Instance;


    private Vector3 WirePointA = Vector3.zero;
    private Vector3 WirePointB = Vector3.zero;

    private GameObject helderA;
    private GameObject helderB;

    public GameObject pickupHintUI;
    private CanvasGroup pickupCG;
    private Tween fadeTween;
    private GameObject nearbyItem;

    public Camera playerCamera;
    public Transform cameraHolder;
    public HandsController handsController;
    public float pickupRange = 2f;

    public TextMeshProUGUI ItemStateText;
    [Header("����������")]
    public Material GologramMaterial;
    public Material GologramRedMaterial;
    public GameObject GologramHolder;


    [Header("������� � ������� ��� ��������� � �����")]
    public Vector3 pistolLocalPosition;
    public Vector3 pistolLocalRotation;

    [Header("������� � ������� ��� ��� ���������")]
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

    private GameObject currentTarget;


    private int CurrentScrew = 0;

    public void Awake()
    {
        Instance = this;

       
    }

    private void Start()
    {
        pickupCG = pickupHintUI.GetComponent<CanvasGroup>();

        pickupCG.alpha = 0f;
    }
    private void Update()
    {






        Ray OutlineRay = new Ray(cameraHolder.position, cameraHolder.forward);
        RaycastHit OutlineHit;

        if (Physics.Raycast(OutlineRay, out OutlineHit, pickupRange-0.5f))
        {
            GameObject target = OutlineHit.collider.gameObject;
            bool Doing = true;

            if (target.transform.parent != null)
            {
                if (target.transform.parent.parent != null)
                {
                    if (target.transform.parent.parent.CompareTag("Unlined"))
                    {
                        Doing = false;
                    }

                }
            } 
            // ���� ����� �ᒺ��
            if (currentTarget != target && !target.CompareTag("Unlined") && Doing && !target.CompareTag("Part"))
            {
                
                

                if (currentTarget != null)
                {
                    Outline oldOutline = currentTarget.GetComponent<Outline>();
                    if (oldOutline != null) Destroy(oldOutline);
                }

                // ������ ������� ������
                Outline outline = target.AddComponent<Outline>();
                outline.OutlineColor = Color.black;
                outline.OutlineWidth = 5;
                outline.OutlineMode = Outline.Mode.OutlineAll;


                currentTarget = target;
                // ��������� ������� � ������������

            }
        }
        else
        {
            // ���� ������ �� ������� � ��������� �������
            if (currentTarget != null)
            {
                Outline oldOutline = currentTarget.GetComponent<Outline>();
                if (oldOutline != null) Destroy(oldOutline);
                currentTarget = null;
            }
        }



















        ItemStateText.text = "";
        Ray InfoRay = new Ray(cameraHolder.position, cameraHolder.forward);
        if (Physics.Raycast(InfoRay, out RaycastHit InfoHit, pickupRange))
        {
            if (InfoHit.transform.GetComponent<Engine>())
            {
                string TextToDisplay = "";
                foreach (KeyValuePair<string, float> fluid in InfoHit.transform.GetComponent<Engine>().Fluids)
                {
                    TextToDisplay = TextToDisplay + fluid.Key.ToString() + ": " + fluid.Value.ToString() + "\n";
                }
                ItemStateText.text = TextToDisplay;
            }
            else if(InfoHit.transform.GetComponent<Bucket>())
            {
                string TextToDisplay = "";
                foreach (KeyValuePair<string, float> fluid in InfoHit.transform.GetComponent<Bucket>().Fluids)
                {
                    TextToDisplay = TextToDisplay + fluid.Key.ToString() + ": " + fluid.Value.ToString() + "\n";
                }
                ItemStateText.text = TextToDisplay;
            }
            
        }
        


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
            if (heldItem == null && nearbyItem != null)
            {
                PickupFromTrigger(nearbyItem);
            }
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

        void LateUpdate()
        {
            if (heldItem != null && heldItem.CompareTag("Pistol"))
            {
                handsController.MainLeftHand.localPosition = leftHandLocalPosition;
                handsController.MainRightHand.localPosition = rightHandLocalPosition;

                handsController.MainLeftHand.localEulerAngles = leftHandLocalRotation;
                handsController.MainRightHand.localEulerAngles = rightHandLocalRotation;
            }
        }

        if (PartGologram == null && heldItem != null && heldItem.CompareTag("Part"))
        {
            //, hit.transform.position, heldItem.transform.rotation
            PartGologram = Instantiate(heldItem);

            foreach (Collider collider in PartGologram.GetComponents<Collider>())
            {
                if (collider != null)
                {
                    collider.isTrigger = true;
                }

            }





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
        if (PartGologram != null)
        {
            PartGologram.transform.tag = "Unlined";
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
            else if (heldItem.CompareTag("Wire"))
            {
                Ray ray = new Ray(cameraHolder.position, cameraHolder.forward);
                if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
                {
                    if (Input.GetMouseButtonDown(1))
                    { 
                        if (WirePointA == Vector3.zero)
                        {
                            if (hit.transform.CompareTag("Connector"))
                            {

                                WirePointA = hit.collider.bounds.center;
                                helderA = hit.transform.gameObject;
                            }
                            else
                            {
                                WirePointA = hit.point + new Vector3(0f, 0.01f , 0f);
                            }
                            
                        }
                        else
                        {
                            GameObject LineObj = new GameObject("Wire");
                            if (hit.transform.CompareTag("Connector"))
                            {
                                WirePointB = hit.transform.position;
                                helderB = hit.transform.gameObject;
                            }
                            else
                            {
                                WirePointB = hit.point + new Vector3(0f, 0.01f, 0f);
                            }


                            

                            LineObj.transform.position = (WirePointA + WirePointB) / 2f;
                            LineObj.tag = "Connector";

                            LineRenderer line;
                            WireScript wire;

                            wire = LineObj.AddComponent<WireScript>();
                            line = LineObj.AddComponent<LineRenderer>();

                            line.positionCount = 2;
                            line.startWidth = 0.05f;
                            line.endWidth = 0.05f;
                            line.material = new Material(Shader.Find("Sprites/Default"));
                            line.startColor = Color.black;
                            line.endColor = Color.black;
                            line.SetPosition(0, WirePointA);
                            line.SetPosition(1, WirePointB);

                            wire.PosA = WirePointA;
                            wire.PosB = WirePointB;
                            wire.boxA = helderA;
                            wire.boxB = helderB;

                            helderA = null;
                            helderB = null;

                            WirePointA = Vector3.zero;
                            WirePointB = Vector3.zero;
                            line = null;
                            LineObj = null;
                        }
                    }
                }
            }
        }

        void Pickup()
        {
            Ray ray = new Ray(cameraHolder.position, cameraHolder.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
            {
                Debug.Log(hit.transform);
                if (hit.collider.CompareTag("Item") || hit.collider.CompareTag("Pistol") || hit.collider.CompareTag("screw") || hit.collider.CompareTag("Part") || hit.collider.CompareTag("Wire"))
                {

                    heldItem = hit.collider.gameObject;

                    Rigidbody rb = heldItem.GetComponent<Rigidbody>();
                    if (rb == null && hit.collider.CompareTag("Part"))
                    {
                        hit.collider.AddComponent<Rigidbody>();
                        rb = heldItem.GetComponent<Rigidbody>();
                    }
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



                        // ��������� ���
                        handsController.MainLeftHand.localPosition = leftHandLocalPosition;
                        handsController.MainRightHand.localPosition = rightHandLocalPosition;
                        handsController.MainLeftHand.localEulerAngles = leftHandLocalRotation;
                        handsController.MainRightHand.localEulerAngles = rightHandLocalRotation;

                    }
                    else if (hit.collider.CompareTag("Part"))
                    {
                        heldItem.transform.localPosition = Vector3.forward * 1f - new Vector3(0f, 0.3f, 0f);

                        heldItem.transform.localRotation = Quaternion.identity;

                        heldItem.AddComponent<GologramScript>();

                        // ��������� ���

                    }
                    else
                    {
                        // ������� �������
                        heldItem.transform.localPosition = Vector3.forward * 1f;
                        heldItem.transform.localRotation = Quaternion.identity;
                    }
                }
            }
        }

   void CheckForPickupAfterDrop()
{
    Ray ray = new Ray(cameraHolder.position, cameraHolder.forward);

    if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
    {
        if (hit.collider.CompareTag("Item") ||
            hit.collider.CompareTag("Pistol") ||
            hit.collider.CompareTag("screw") ||
            hit.collider.CompareTag("Part"))
        {
            SetNearbyItem(hit.collider.gameObject);
            return;
        }
    }

    HidePickupHint();
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

            handsController.MainLeftHand.localPosition = defLeftHandPos;
            handsController.MainRightHand.localPosition = defRightHandPos;

            handsController.MainLeftHand.localEulerAngles = defLeftHandRotation;
            handsController.MainRightHand.localEulerAngles = defRightHandRotation;

            heldItem = null;

            HandsController controller = handsController.GetComponent<HandsController>();
            if (controller != null)
            {
                controller.Anim.SetTrigger("End");
                Debug.Log("Ended");

            }
            CheckForPickupAfterDrop();

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
                else
                {

                    Destroy(rb);
                    //Destroy(heldItem.GetComponent<Partscript>());
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

    void ShowPickupHint()
    {
        pickupHintUI.SetActive(true);

        fadeTween?.Kill();
        pickupCG.alpha = 0f;

        fadeTween = pickupCG.DOFade(1f, 0.75f);
    }

    void HidePickupHint()
    {
        fadeTween?.Kill();

        fadeTween = pickupCG
            .DOFade(0f, 0.75f)
            .OnComplete(() =>
            {
                pickupHintUI.SetActive(false);
            });
    }

    public void SetNearbyItem(GameObject item)
    {
        nearbyItem = item;

        ShowPickupHint();
    }

    public void ClearNearbyItem(GameObject item)
    {
        if (nearbyItem == item)
        {
            nearbyItem = null;

            HidePickupHint();
        }
    }

    void PickupFromTrigger(GameObject item)
    {
        heldItem = item;

        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        heldItem.transform.SetParent(handsController.transform);

        if (heldItem.CompareTag("Pistol"))
        {
            HandsController controller = handsController;

            Weapon weapon = heldItem.GetComponent<Weapon>();
            weapon.Hands = controller;
            weapon.playerCamera = playerCamera;

            controller.Anim.SetTrigger("Start");

            // ������� ������
            heldItem.transform.localPosition = pistolLocalPosition;
            heldItem.transform.localEulerAngles = pistolLocalRotation;

            // ������� ��� (�����������)
            handsController.MainLeftHand.localPosition = leftHandLocalPosition;
            handsController.MainRightHand.localPosition = rightHandLocalPosition;

            handsController.MainLeftHand.localEulerAngles = leftHandLocalRotation;
            handsController.MainRightHand.localEulerAngles = rightHandLocalRotation;
        }


        pickupHintUI.SetActive(false);
        nearbyItem = null;
    }

}
