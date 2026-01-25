using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    public string StartFluidKey;
    public float StartFluidValue;

    public float rayLength = 10f;
    public Transform RayPos;

    public int maxFluids;
    public Dictionary<string, float> Fluids = new Dictionary<string, float>();
    // Напрямок променя (за замовчуванням вперед від об'єкта)
    public Vector3 rayDirection = Vector3.forward;
    private void Start()
    {
        Fluids.Add(StartFluidKey, StartFluidValue);
        Debug.Log(Fluids.First().Key);
        Debug.Log(Fluids.First().Value);
    }
    void Update()
    {
        if(Fluids.Count > maxFluids) { Fluids.Remove(Fluids.Last().Key); }

        KeyValuePair<string, float> DeletableObj = new KeyValuePair<string, float>("empty", 0f);
        foreach (KeyValuePair<string, float> fluid in Fluids)
        {
            if (fluid.Value <= 0) { DeletableObj = fluid; }
        }
        if (Fluids.ContainsKey(DeletableObj.Key))
        {
            Fluids.Remove(DeletableObj.Key);
        }
        // Створюємо рейкаст від позиції цього об'єкта
        Ray ray = new Ray(RayPos.position, transform.TransformDirection(rayDirection));
        RaycastHit hit;

        // Якщо промінь влучив у якийсь об'єкт
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            
            if (hit.transform.gameObject.GetComponent<Engine>())
            {
                Engine ContactObj = hit.transform.gameObject.GetComponent<Engine>();
                //Debug.Log(Fluids.First().Value);
                if (Fluids.Any())
                {
                    if (Fluids.First().Value > 0)
                    {
                        if (ContactObj.Fluids.ContainsKey(Fluids.First().Key))
                        {
                            
                            ContactObj.Fluids[Fluids.First().Key] += 1f;
                            Fluids[Fluids.First().Key] -= 1f;

                        }
                        else if (ContactObj.Fluids.Count < ContactObj.maxFluids)
                        {
                            
                            ContactObj.Fluids.Add(Fluids.First().Key, 1);
                            Fluids[Fluids.First().Key] -= 1f;
                        }
                        float CurrentTank = 0;
                        foreach (KeyValuePair<string, float> fluid in Fluids)
                        {
                            CurrentTank += fluid.Value;
                        }
                        if (ContactObj.maxTank > CurrentTank)
                        {


                        }
                    }
                }

               
                



            }

        }
    }

}
