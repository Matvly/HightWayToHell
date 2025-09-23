using UnityEngine;

public class PointReycast : MonoBehaviour
{
    public GameObject prefabToSpawn; // ������, ���� ������ ����������
    public float rayDistance = 100f; // �������� ��������
    private int PointReload = 5;

    void Update()
    {
        // �������� ���������� �������� ������ ����
        if (Input.GetMouseButtonDown(2))
        {
            if (PointReload > 0)
            {
                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, rayDistance))
                {
                    // ��������� ������ � ����� ��������
                    Instantiate(prefabToSpawn, hit.point, Quaternion.identity);

                    Debug.Log("�������� ������ � �����: " + hit.point);
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
