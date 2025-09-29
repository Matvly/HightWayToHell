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
        Collider cl = gameObject.GetComponent<Collider>();
        if (transform.Find("screw") != null)
        {

            transform.Find("screw").position = transform.position;
            transform.Find("screw").rotation = transform.rotation;


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
