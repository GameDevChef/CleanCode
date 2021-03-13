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
       
        [SerializeField] private AudioSource mainAudioSource;
        [SerializeField] private Text ammoText;
        [SerializeField] private int maxAmmoNumber;

        private InputReceiver inputReciever;

        private Weapon holdWeapon;
        private int currentWeaponIndex;
        private float currentAmmoNumber;
        private float currentShotWaitTime;


        private void Awake()
        {
            inputReciever = GetComponent<InputReceiver>();
        }

        private void Start()
        {
            SetActiveWeaponAndInitializeVariables();
        }

        private void SetActiveWeaponAndInitializeVariables()
        {
            holdWeapon = avaliableWeapons[currentWeaponIndex];
            foreach (var weapon in avaliableWeapons)
            {
                weapon.gameObject.SetActive(false);
            }
            holdWeapon.gameObject.SetActive(true);
            currentAmmoNumber = maxAmmoNumber;
          
            Cursor.lockState = CursorLockMode.Locked;
            SetAmmoText();
        }

        public void SetAmmoText()
        {
            ammoText.text = currentAmmoNumber.ToString();
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
                currentAmmoNumber--;
                SetAmmoText();
                switch (holdWeapon.GetWeaponType())
                {
                    case WeaponType.Pistol:
                        ShootPistol();
                        break;
                    case WeaponType.Rilfe:
                        ShootRifle();
                        break;
                    case WeaponType.RPG:
                        ShootRPG();
                        break;
                    default:
                        break;
                }
            }
        }

        private bool CheckIfCanShoot()
        {
            return inputReciever.IsShooting && currentShotWaitTime > holdWeapon.GetShootingInterval() && HasEnoughAmmo();
        }

        private bool HasEnoughAmmo()
        {
            return currentAmmoNumber > 0;
        }

        private void ShootPistol()
        {
            PlayWeaponShotSound();
            ShootWithRaycast();
        }

        private void PlayWeaponShotSound()
        {
            mainAudioSource.clip = holdWeapon.GetShotSound();
            mainAudioSource.Stop();
            mainAudioSource.Play();
        }

        private void ShootWithRaycast()
        {
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out RaycastHit hit2, holdWeapon.GetWeaponRange()))
            {
                EnemyManager controller = hit2.collider.GetComponentInParent<EnemyManager>();
                Vector3 direction = hit2.point - holdWeapon.GetBulletSpawnTransform().position;
                if (controller)
                {
                    controller.OnEnemyShot(direction, hit2.collider.GetComponent<Rigidbody>(), holdWeapon.GetWeaponDamage());
                }
            }
        }
      
        private void ShootRifle()
        {
            PlayWeaponShotSound();
            ShootWithRaycast();
        }

        private void ShootRPG()
        {
            PlayWeaponShotSound();
            ShootWithProjectile();
        }

        private void ShootWithProjectile()
        {
            Projectile projectile = Instantiate(holdWeapon.GetProjectilePrefab(),
                holdWeapon.GetBulletSpawnTransform().position,
                holdWeapon.GetBulletSpawnTransform().rotation);
            projectile.Init(holdWeapon);
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
        }

        private void Reload()
        {
            currentShotWaitTime = 0f;
            currentAmmoNumber = maxAmmoNumber;
            mainAudioSource.clip = holdWeapon.GetReloadSound();
            mainAudioSource.Stop();
            mainAudioSource.Play();
            SetAmmoText();
        }
    }
}