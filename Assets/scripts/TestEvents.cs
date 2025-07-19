using UnityEngine;
using System;

public class TestEvents : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TestAction.Test.Invoke();
        }
        
    }

}
