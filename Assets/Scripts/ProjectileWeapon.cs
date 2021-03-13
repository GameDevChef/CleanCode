using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{

    [SerializeField] private float projectileSpeed;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private AudioClip impactClip;
    public override void Shot()
    {
        base.Shot();
        Projectile projectile = Instantiate(projectilePrefab, bulletSpawnTransform.position, bulletSpawnTransform.rotation);
        projectile.Init(this);
    }

    public float GetProjectileSpeed()
    {
        return projectileSpeed;
    }

    public AudioClip GetImpactClip()
    {
        return impactClip;
    }
}
