using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WeaponType
{
    Pistol, Rilfe, RPG
}

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] private int projectileSpeed;
    [SerializeField] private AudioClip shotClip;
    [SerializeField] private AudioClip reloadClip;
    [SerializeField] private AudioClip impactClip;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform bulletSpawnTransform;

    [SerializeField] private float shootingInterval;

    public WeaponType GetWeaponType()
    {
        return weaponType;
    }

    public float GetWeaponRange()
    {
        return range;
    }

    public int GetWeaponDamage()
    {
        return damage;
    }

    public AudioClip GetShotSound()
    {
        return shotClip;
    }

    public AudioClip GetReloadSound()
    {
        return reloadClip;
    }

    public Projectile GetProjectilePrefab()
    {
        return projectilePrefab;
    }

    public int GetProjectileSpeed()
    {
        return projectileSpeed;
    }

    public AudioClip GetImpactClip()
    {
        return impactClip;
    }

    public Transform GetBulletSpawnTransform()
    {
        return bulletSpawnTransform;
    }

    public float GetShootingInterval()
    {
        return shootingInterval;
    }
}