using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GologramScript : MonoBehaviour
{
    [SerializeField] private GameObject[] holes;

    private List<GameObject> intersections = new();
    public GameObject[] Holes => holes;

    public Action<GameObject> OnEnterCallback { get; set; }
    public Action<GameObject> OnExitCallback { get; set; }

    public void OnTriggerEnter(Collider other)
    {


        if (!intersections.Contains(other.gameObject))
        {
            intersections.Add(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (intersections.Contains(other.gameObject))
        {
            intersections.Remove(other.gameObject);
        }
    }

    public bool GetPermissonToPlace()
    {

        
        foreach (GameObject part in intersections)
        {
            Debug.Log(part);
            if (part.tag != "Hands" && part.tag != "screw" && part.tag != "Meshes" && part.tag != "hole")
            {
                
                return false;
            }
        }
        
        return true;

    }


}
