using UnityEngine;
using UnityEngine.Events;

public class Test : MonoBehaviour
{
    [SerializeField] private UnityEvent OnAction;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            OnAction.Invoke();
        }
    }
}