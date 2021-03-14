using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Weapon[] avaliableWeapons;
    [SerializeField] private Text ammoText;
    
    private InputReciever inputReciever;
    private Weapon holdWeapon;
    private int currentWeaponIndex;
    private float currentShootWaitTime;
   
    private void Awake()
    {
        inputReciever = GetComponent<InputReciever>();
    }

    private void Start()
    {
        SetActiveWeapon();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void SetActiveWeapon()
    {
        holdWeapon = avaliableWeapons[currentWeaponIndex];
        foreach (var weapon in avaliableWeapons)
        {
            weapon.gameObject.SetActive(false);
        }
        holdWeapon.gameObject.SetActive(true);
        SetAmmoText();
    }

    public void SetAmmoText()
    {
        ammoText.text = holdWeapon.GetCurrentAmmo().ToString();
    }

    private void Update()
    {     
        HandleSwitchingWeapons();
        HandleShoting();
        if (inputReciever.IsReloading)
        {
            Reload();
        }
    }
  

    private void HandleSwitchingWeapons()
    {
        if (inputReciever.IsScrollingDown)
        {
            SwitchToPreviousWeapon();
        }

        if (inputReciever.IsScrollingUp)
        {
            SwitchToNextWeapon();
        }
    }

    private void SwitchToPreviousWeapon()
    {
        currentWeaponIndex--;
        if (currentWeaponIndex < 0)
            currentWeaponIndex = avaliableWeapons.Length - 1;
        SetActiveWeapon();
    }

    private void SwitchToNextWeapon()
    {
        currentWeaponIndex++;
        if (currentWeaponIndex > avaliableWeapons.Length - 1)
            currentWeaponIndex = 0;
        SetActiveWeapon();
    }

    private void HandleShoting()
    {
        currentShootWaitTime += Time.deltaTime;
        if (CheckIfCanShoot())
        {
            currentShootWaitTime = 0;
            holdWeapon.Shoot();
            SetAmmoText();          
        }
    }

    private bool CheckIfCanShoot()
    {
        return inputReciever.IsShooting && currentShootWaitTime > holdWeapon.GetShootingInterval() && holdWeapon.HasEnoughAmmo();
    }

    private void Reload()
    {
        holdWeapon.Reload();
        SetAmmoText();
    }
}
