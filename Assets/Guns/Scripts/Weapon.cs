using UnityEngine;

public class Weapon : MonoBehaviour
{
    public HandsController Hands;
    
    public WeaponData data;
    public Transform shootPoint;

    public Camera playerCamera;

    private float nextFireTime = 0f;
    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >=  nextFireTime && gameObject.transform.GetComponentInParent<HandsController>() != null)
        {
            Shoot();
            nextFireTime = Time.time + data.fireRate;
        }
    }




    void Shoot()
    {
        if (data.bulletPrefab != null && shootPoint != null)
        {
            Instantiate(data.bulletPrefab, shootPoint.position, shootPoint.rotation);

            
            Hands.HandsAnim.SetTrigger("Shoot");
        }
    }
    //void Shoot()
    //{

    //    Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    //    Vector3 targetPoint;

    //    if (Physics.Raycast(ray, out RaycastHit hit))
    //    {
    //        targetPoint = hit.point;
    //    }
    //    else
    //    {
    //        targetPoint = ray.GetPoint(1000f);
    //    }

    //    Vector3 dir = (targetPoint - shootPoint.position).normalized;

        
    //    bullet.GetComponent<Rigidbody>().linearVelocity = dir * data.bulletSpeed;


    //}
}
