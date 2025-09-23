using UnityEngine;

public class PointReycast : MonoBehaviour
{
    public GameObject prefabToSpawn; // Префаб, який будемо створювати
    public float rayDistance = 100f; // Дальність рейкаста
    private int PointReload = 5;

    void Update()
    {
        // Перевірка натискання середньої кнопки миші
        if (Input.GetMouseButtonDown(2))
        {
            if (PointReload > 0)
            {
                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, rayDistance))
                {
                    // Створюємо префаб у точці зіткнення
                    Instantiate(prefabToSpawn, hit.point, Quaternion.identity);

                    Debug.Log("Створено префаб у точці: " + hit.point);
                    PointReload--;
                }
            }
            if (PointReload == 0)
            {
                PointReload--;
                Invoke("ReloadPoints", 20f);
            }

        }

    }
    void ReloadPoints()
    {
        PointReload = 3;
    }
}
