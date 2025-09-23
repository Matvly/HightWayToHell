using UnityEngine;
using DG.Tweening;
public class BillboardWithOffset : MonoBehaviour
{
    public float waitTime = 5f;     // Час очікування перед зникненням
    public float fadeDuration = 1f; // Час, за який об’єкт зникне

    public GameObject WavePref;
    public Vector3 offset = new Vector3(0, 0.5f, 0); // Зсув відносно позиції об'єкта
    public bool onlyRotateY = false; // Якщо true — крутиться тільки по горизонталі

    void LateUpdate()
    {
        if (Camera.main == null) return;

        // Позиція з урахуванням зсуву
        Vector3 targetPos = Camera.main.transform.position + offset;

        if (onlyRotateY)
        {
            // Ігноруємо нахил камери по вертикалі
            targetPos.y = transform.position.y;
        }

        transform.LookAt(targetPos);
    }


    
    void Start()
    {
        InvokeRepeating("Wave", 0f, 0.2f);
        // Чекаємо, а потім запускаємо анімацію
        DOVirtual.DelayedCall(waitTime, () =>
        {
            // Якщо це SpriteRenderer

          
            // Якщо це MeshRenderer (3D)
            var rend = GetComponentInChildren<Renderer>();
            if (rend != null && rend.material.HasProperty("_Color"))
            {
                Color startColor = rend.material.color;
                rend.material.DOFade(0f, fadeDuration).OnComplete(() => Destroy(gameObject));
                return;
            }

            

        });
    }


    void Wave()
    {
        Instantiate(WavePref, transform.position , Quaternion.identity);
    }
}

