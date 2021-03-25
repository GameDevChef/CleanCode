using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : Weapon
{
    public override void Shoot()
    {
        base.Shoot();
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out RaycastHit hit2, range))
        {
            EnemyManager controller = hit2.collider.GetComponentInParent<EnemyManager>();
            Vector3 direction = hit2.point - bulletSpawnTransform.position;
            if (controller)
            {
                controller.OnEnemyShot(direction, hit2.collider.GetComponent<Rigidbody>(), GetWeaponDamage());
            }
        }
    }
}
