using System;
using UnityEngine;

public class Bank : MonoBehaviour
{
    public static Action<int> OnAddGold;
    public static Action<int> OnSubGold;

    private int _gold;

    private void Start()
    {
        _gold = 0;

        GoldInterface.OnGoldChange.Invoke(_gold);

        OnAddGold += AddGold;
        OnSubGold += SubGold;
    }

    private void AddGold(int value)
    {
        _gold += value;
        GoldInterface.OnGoldChange.Invoke(_gold);
    }

    private void SubGold(int value)
    {
        _gold -= value;
        GoldInterface.OnGoldChange.Invoke(_gold);
    }

    private void OnDestroy()
    {
        OnAddGold -= AddGold;
        OnSubGold -= SubGold;
    }
}