using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Scriptable Objects/Gun")]
public class WeaponData : ScriptableObject
{
    [Header("Характеристики")]
    public float fireRate;
    public int damage;
    public float bulletSpeed;

    [Header("Снаряд")]
    public GameObject bulletPrefab;
}
