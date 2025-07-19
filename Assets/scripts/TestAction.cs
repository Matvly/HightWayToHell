using UnityEngine;
using System;
public class TestAction : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public bool Dances = false;
    
    public static Action Test;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Test += Dance;
        

    }

    // Update is called once per frame
    void Dance()
    {
        Dances = true;   
    }
    void Update()
    {
        if (Dances == true)
        {
            Debug.Log("dfdsf");
            if (transform.parent != null)
            {
                
                transform.RotateAround(transform.parent.position, Vector3.up, rotationSpeed * Time.deltaTime);
            }

        }

    }
    private void OnDestroy()
    {
        Test -= Dance;
    }
}
