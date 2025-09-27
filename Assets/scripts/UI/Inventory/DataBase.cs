using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataBAse : MonoBehaviour
{
    public List<Item> items = new List<Item>();
}

[System.Serializable]

public class Item
{
    public int id;
    public string name;
    public Sprite img;
}

