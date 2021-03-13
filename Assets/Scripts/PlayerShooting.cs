using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameDevChef.DirtyCode
{
    public class PlayerShooting : MonoBehaviour
    {
        [SerializeField] private Weapon[] avaliableWeapons;      
        [SerializeField] private Text ammoText;

        private InputReceiver inputReciever;
        private Weapon holdWeapon;
        private int currentWeaponIndex;
        private float currentShotWaitTime;

        private void Awake()
        {
            inputReciever = GetComponent<InputReceiver>();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            ChangeHoldWeapon();
        }

        public void SetAmmoText()
        {
            ammoText.text = holdWeapon.GetCurrentAmmo().ToString();
        }    

        private void Update()
        {
            HandleShootingWeapon();
            HandleSwitchingWeapon();
            currentShotWaitTime += Time.deltaTime;

            if (inputReciever.IsReloading)
            {
                Reload();
            }
        }  

        private void HandleShootingWeapon()
        {
            if (CheckIfCanShoot())
            {
                currentShotWaitTime = 0;
                holdWeapon.Shot();
                SetAmmoText();
            }
        }

        private bool CheckIfCanShoot()
        {
            return inputReciever.IsShooting && currentShotWaitTime > holdWeapon.GetShootingInterval() && holdWeapon.HasEnoughAmmo();
        }
     
        private void HandleSwitchingWeapon()
        {
            if (Input.mouseScrollDelta.y < 0)
            {
                SwitchToPreviousWeapon();
            }

            if (Input.mouseScrollDelta.y > 0)
            {
                SwitchToNextWeapon();
            }
        }

        private void SwitchToNextWeapon()
        {
            currentWeaponIndex++;
            if (currentWeaponIndex > avaliableWeapons.Length - 1)
                currentWeaponIndex = 0;
            ChangeHoldWeapon();
        }

        private void SwitchToPreviousWeapon()
        {
            currentWeaponIndex--;
            if (currentWeaponIndex < 0)
                currentWeaponIndex = avaliableWeapons.Length - 1;
            ChangeHoldWeapon();
        }

        private void ChangeHoldWeapon()
        {
            holdWeapon = avaliableWeapons[currentWeaponIndex];
            foreach (var weapon in avaliableWeapons)
            {
                weapon.gameObject.SetActive(false);
            }
            holdWeapon.gameObject.SetActive(true);
            SetAmmoText();
        }

        private void Reload()
        {
            currentShotWaitTime = 0f;            
            holdWeapon.Reload();
            SetAmmoText();
        }
    }
}