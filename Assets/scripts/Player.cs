using UnityEngine;

public class Player : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Bank.OnAddGold.Invoke(100);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Bank.OnSubGold.Invoke(100);
        }
    }
}