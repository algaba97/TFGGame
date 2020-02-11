using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject bulletPrefab;

    public void Shoot(float speed, Vector3 target,int damage = 1,System.Action<GunController> OnShootAction = null)
    {
        Bullet bullet = Instantiate(bulletPrefab).GetComponent<Bullet>();
        bullet.transform.position = transform.position;
        bullet.Init(speed, target,damage);
        if (OnShootAction != null)
            OnShootAction(this);
    }
}
