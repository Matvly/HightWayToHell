using System;
using TMPro;
using UnityEngine;

public class GoldInterface : MonoBehaviour
{
    public static Action<int> OnGoldChange;

    [SerializeField] private TextMeshProUGUI _goldText;

    private void Awake()
    {
        OnGoldChange += ChangeGold;
    }

    private void ChangeGold(int value)
    {
        _goldText.text = value.ToString();
    }

    private void OnDestroy()
    {
        OnGoldChange -= ChangeGold;
    }
}