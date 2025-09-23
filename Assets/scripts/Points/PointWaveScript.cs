using DG.Tweening;
using UnityEngine;

public class PointWaveScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.3f)
                 .SetEase(Ease.OutQuad);
        var rend = GetComponent<Renderer>();
        if (rend != null && rend.material.HasProperty("_Color"))
        {
            Color startColor = rend.material.color;
            rend.material.DOFade(0f, 2f).OnComplete(() => Destroy(gameObject));
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
