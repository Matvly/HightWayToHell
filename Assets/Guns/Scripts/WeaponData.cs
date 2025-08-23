using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Scriptable Objects/Gun")]
public class WeaponData : ScriptableObject
{
    [Header("��������������")]
    public float fireRate;
    public int damage;
    public float bulletSpeed;

    [Header("������")]
    public GameObject bulletPrefab;
}
