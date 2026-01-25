using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Engine : MonoBehaviour
{
    
    public float maxTank = 2500;

    public int maxFluids;
    public Dictionary<string, float> Fluids = new Dictionary<string, float>();

   


    

    public bool started;
    public GameObject[] rotationParts;
    public Vector3 rotationSpeed = new Vector3(0, 100, 0);

    void Update()
    {
        
        if (Fluids.Count > maxFluids) { Fluids.Remove(Fluids.Last().Key); }

        KeyValuePair<string, float> DeletableObj = new KeyValuePair<string, float>("empty", 0f);
        foreach (KeyValuePair<string, float> fluid in Fluids)
        {
            if (fluid.Value <= 0) { DeletableObj = fluid; }
        }
        if (Fluids.ContainsKey(DeletableObj.Key))
        {
            Fluids.Remove(DeletableObj.Key);
        }
        
        if (started && (!Fluids.ContainsKey("gasoline") || !Fluids.ContainsKey("oil")))
        {
            started = false;
        }
        if (started) 
        {
            foreach (GameObject prt in rotationParts)
            {
                prt.transform.Rotate(rotationSpeed * Time.deltaTime);
            }
            
            Fluids["gasoline"] -= 0.1f;
            Fluids["oil"] -= 0.001f;

        }
        // обертання об’єкта кожен кадр
        
    }



}

