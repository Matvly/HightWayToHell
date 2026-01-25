using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Partscript : MonoBehaviour
{
    [SerializeField] private GameObject[] holes;
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject collisionReflex;
    [SerializeField] private float linearDamageLimitter = 1;
    public GameObject[] Holes => holes;

    private float linearDamageLimitter2X;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        linearDamageLimitter2X = linearDamageLimitter * 2;
    }

    private void Update()
    {
        foreach (Renderer renderer in gameObject.transform.Find("Meshes").GetComponentsInChildren<Renderer>())
        {
            if (renderer != null)
            {
                renderer.material.SetFloat("_BumpScale", maxHealth / health - 1);
            }

        }
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Rigidbody>() != null && rb != null)
        {
            if (rb.linearVelocity.magnitude + collision.transform.GetComponent<Rigidbody>().linearVelocity.magnitude >= linearDamageLimitter2X)
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
            if (rb.linearVelocity.magnitude >= linearDamageLimitter)
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
