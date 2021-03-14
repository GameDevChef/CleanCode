using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    [SerializeField] private AudioClip impactClip;
    [SerializeField] private int projectileSpeed;
    [SerializeField] private Projectile projectilePrefab;

    public override void Shoot()
    {
        base.Shoot();
        Projectile projectile = Instantiate(projectilePrefab, bulletSpawnTransform.position, bulletSpawnTransform.rotation);
        projectile.Init(this);
    }

    public int GetProjectileSpeed()
    {
        return projectileSpeed;
    }

    public AudioClip GetImpactClip()
    {
        return impactClip;
    }
}
