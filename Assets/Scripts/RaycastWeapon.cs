using GameDevChef.DirtyCode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : Weapon
{
    public override void Shot()
    {
        base.Shot();
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out RaycastHit hit, range))
        {
            EnemyManager enemy = hit.collider.GetComponentInParent<EnemyManager>();
            Vector3 direction = hit.point - bulletSpawnTransform.position;
            if (enemy)
            {
                enemy.ImpactRigidbody(direction, hit.collider.GetComponent<Rigidbody>());
                enemy.TakeDamage(GetWeaponDamage());
            }
        }
    }
}