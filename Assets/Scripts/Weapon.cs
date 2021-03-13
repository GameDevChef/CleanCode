using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected Transform bulletSpawnTransform;
    [SerializeField] private AudioSource mainAudioSource;
    [SerializeField] private AudioClip shotClip;
    [SerializeField] private AudioClip reloadClip;
    [SerializeField] protected float range;
    [SerializeField] private int damage;
    [SerializeField] private int maxAmmoNumber;
    [SerializeField] private float shootingInterval;

    private int currentAmmoNumber;

    private void Awake()
    {
        currentAmmoNumber = maxAmmoNumber;
    }
    public virtual void Shot()
    {
        currentAmmoNumber--;
        PlayWeaponShotSound();
    }
    public virtual void Reload()
    {
        currentAmmoNumber = maxAmmoNumber;
        mainAudioSource.clip = reloadClip;
        ResetAudioSource();
    }

    public bool HasEnoughAmmo()
    {
        return currentAmmoNumber > 0;
    }

    protected void PlayWeaponShotSound()
    {
        mainAudioSource.clip = shotClip;
        ResetAudioSource();
    }

    private void ResetAudioSource()
    {
        mainAudioSource.Stop();
        mainAudioSource.Play();
    }

    public int GetCurrentAmmo()
    {
        return currentAmmoNumber;
    }

    public int GetWeaponDamage()
    {
        return damage;
    }

   

    public float GetShootingInterval()
    {
        return shootingInterval;
    }
}