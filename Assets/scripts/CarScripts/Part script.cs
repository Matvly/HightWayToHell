using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Partscript : MonoBehaviour
{
    [SerializeField] private GameObject[] holes;
    [SerializeField] private float health;
    [SerializeField] private GameObject collisionReflex;

    public GameObject[] Holes => holes;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Rigidbody>() != null)
        {
            if (rb.linearVelocity.magnitude + collision.transform.GetComponent<Rigidbody>().linearVelocity.magnitude >= 2)
            {
                foreach (ContactPoint contact in collision.contacts)
                {
                    Vector3 point = contact.point;
                    Quaternion rotation = Quaternion.LookRotation(contact.normal);
                    Instantiate(collisionReflex, point, rotation);


                }

                health -= collision.transform.GetComponent<Rigidbody>().linearVelocity.magnitude / 2 * collision.transform.GetComponent<Rigidbody>().mass;
               
                health -= rb.linearVelocity.magnitude / 2;
                Debug.Log(health);

                if (health <= 0) { Destroy(gameObject); }
            }

        }
        else
        {
            if (rb.linearVelocity.magnitude >= 1)
            {
                foreach (ContactPoint contact in collision.contacts)
                {
                    Vector3 point = contact.point;
                    Quaternion rotation = Quaternion.LookRotation(contact.normal);
                    Instantiate(collisionReflex, point, rotation);


                }


                
                
                health -= rb.linearVelocity.magnitude / 2;
                Debug.Log(health);

                if (health <= 0) { Destroy(gameObject); }
            }
        }
        
        

    }


}
