using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData data;
    public Transform shootPoint;

    private float nextFireTime = 0f;
    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >=  nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + data.fireRate;
        }
    }

    void Shoot()
    {
        Camera cam = Camera.main;

       
        Vector3 direction = cam.transform.forward;

       
        GameObject bullet = Instantiate(data.bulletPrefab, shootPoint.position, Quaternion.identity);

       
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = direction * data.bulletSpeed;
    }
}
