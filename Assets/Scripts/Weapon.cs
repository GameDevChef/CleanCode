using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected float range;
    [SerializeField] private int damage;
    [SerializeField] private AudioClip shotClip;
    [SerializeField] private AudioClip reloadClip;
    [SerializeField] private int maxAmmo;
    [SerializeField] protected Transform bulletSpawnTransform;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float shootingInterval;

    private int currentAmmo;

    private void Awake()
    {
        currentAmmo = maxAmmo;
    }

    public virtual void Shoot()
    {
        PlayAudioClip(shotClip);
        currentAmmo--;
    }

    public void Reload()
    {
        PlayAudioClip(reloadClip);
        currentAmmo = maxAmmo;
    }

    private void PlayAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Stop();
        audioSource.Play();
    }

    public int GetWeaponDamage()
    {
        return damage;
    }

    internal int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public float GetShootingInterval()
    {
        return shootingInterval;
    }

    public bool HasEnoughAmmo()
    {
        return currentAmmo > 0;
    } 
}