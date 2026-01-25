using DG.Tweening;
using TMPro;
using UnityEngine;

public class HelicopterScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool locking = true;
    public Transform targetPosition1;
    public Transform targetPosition2;
    public Transform player;
    public Transform playerPos;
    void Start()
    {

        

        Sequence seq = DOTween.Sequence();

        // Перша анімація (рух 10 секунд)
        seq.Append(transform.DOMove(targetPosition1.position, 10f)
                            .SetEase(Ease.InOutQuad));
        seq.AppendCallback(() => locking = false);
        seq.AppendCallback(() => player.position = targetPosition1.position + new Vector3(1f, 10.3f, -2.1f));
        // Пауза 5 секунд
        seq.AppendInterval(2f);
       
        // Друга анімація (наприклад, рух вгору за 3 секунди)
        seq.Append(transform.DOMove(targetPosition2.position, 30f)
                            .SetEase(Ease.InOutQuad));
        seq.Join(transform.DORotate(new Vector3(0f, -90f, 0f), 5f, RotateMode.FastBeyond360)
                 .SetEase(Ease.InOutQuad));

        seq.AppendCallback(() => Destroy(gameObject));

        // Запускаємо послідовність
        seq.Play();
    


}


// Update is called once per frame
void Update()
    {
        if (locking) {
            player.position = playerPos.position + new Vector3(1f, 1.3f, -1.9f);
        }
        
    }
}
