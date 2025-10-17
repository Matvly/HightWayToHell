using UnityEngine;

public class Holescript : MonoBehaviour
{
    public GameObject Screw;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Transform Tr = GetComponentInParent<Transform>();
        Collider cl = gameObject.GetComponent<Collider>();
        if (Tr.Find("screw") != null)
        {

            Tr.Find("screw").position = transform.position;
            Tr.Find("screw").rotation = transform.rotation;


            if (cl != null)
            {
                cl.enabled = false;
            }
            
        }
        else
        {
            cl.enabled = true;
        }
    }

   

}
