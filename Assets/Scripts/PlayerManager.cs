using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameDevChef.DirtyCode
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private Weapon[] avaliableWeapons;
        [SerializeField] private Transform rifleTransformParent;
        [SerializeField] private float walkSpeed;
        [SerializeField] private float runSpeed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float mouseSensitivity;
        [SerializeField] private AudioSource mainAudioSource;
        [SerializeField] private Text ammoText;
        [SerializeField] private int maxAmmoNumber;

        private Rigidbody rigidbody;
        private Weapon holdWeapon;
        private int currentWeaponIndex;
        private float currentAmmoNumber;
        private float currentShotWaitTime;
        private float currentRotationY;
        private float currentRotationX;
        private bool isRunningEnabled;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
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
            currentRotationY = transform.eulerAngles.y;
            currentRotationX = transform.eulerAngles.x;
            Cursor.lockState = CursorLockMode.Locked;
            SetAmmoText();
        }

        private void FixedUpdate()
        {
            float moveSpeed = isRunningEnabled ? runSpeed : walkSpeed;
            var moveVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            var worldMoveVector = transform.TransformDirection(moveVector);
            worldMoveVector = new Vector3(worldMoveVector.x, 0f, worldMoveVector.z);
            rigidbody.velocity = worldMoveVector.normalized * moveSpeed;
        }

        private void Update()
        {
            RotatePlayerObject();
            HandleShootingWeapon();
            HandleSwitchingWeapon();
            currentShotWaitTime += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                isRunningEnabled = true;
            }
            else
            {
                isRunningEnabled = false;
            }
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
            return Input.GetMouseButton(0) && currentShotWaitTime > holdWeapon.GetShootingInterval() && HasEnoughAmmo();
        }

        private void ShootRPG()
        {
            
            mainAudioSource.clip = holdWeapon.GetShotSound();
            mainAudioSource.Stop();
            mainAudioSource.Play();
            Projectile projectile = Instantiate(holdWeapon.GetProjectilePrefab(),
                holdWeapon.GetBulletSpawnTransform().position,
                holdWeapon.GetBulletSpawnTransform().rotation);
            projectile.Init(holdWeapon.GetWeaponDamage(), holdWeapon.GetProjectileSpeed(), holdWeapon.GetImpactClip());
        }

        private void ShootRifle()
        {
            mainAudioSource.clip = holdWeapon.GetShotSound();
            mainAudioSource.Stop();
            mainAudioSource.Play();
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

        private void ShootPistol()
        {
            mainAudioSource.clip = holdWeapon.GetShotSound();
            mainAudioSource.Stop();
            mainAudioSource.Play();
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out RaycastHit hit1, holdWeapon.GetWeaponRange()))
            {
                EnemyManager controller = hit1.collider.GetComponentInParent<EnemyManager>();
                Vector3 direction = hit1.point - holdWeapon.GetBulletSpawnTransform().position;
                if (controller)
                {
                    controller.OnEnemyShot(direction, hit1.collider.GetComponent<Rigidbody>(), holdWeapon.GetWeaponDamage());
                }
            }
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

        private void SwitchToNextWeapon()
        {
            currentWeaponIndex++;
            if (currentWeaponIndex > avaliableWeapons.Length - 1)
                currentWeaponIndex = 0;
            holdWeapon = avaliableWeapons[currentWeaponIndex];
            foreach (var weapon in avaliableWeapons)
            {
                weapon.gameObject.SetActive(false);
            }
            holdWeapon.gameObject.SetActive(true);
        }

        private void SwitchToPreviousWeapon()
        {
            currentWeaponIndex--;
            if (currentWeaponIndex < 0)
                currentWeaponIndex = avaliableWeapons.Length - 1;
            holdWeapon = avaliableWeapons[currentWeaponIndex];
            foreach (var weapon in avaliableWeapons)
            {
                weapon.gameObject.SetActive(false);
            }
            holdWeapon.gameObject.SetActive(true);
        }

        private bool HasEnoughAmmo()
        {
            return currentAmmoNumber > 0;
        }

        private void RotatePlayerObject()
        {
            float yaw = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed * mouseSensitivity;
            float pitch = Input.GetAxis("Mouse Y") * Time.deltaTime * rotationSpeed * mouseSensitivity;
            currentRotationY += yaw;
            currentRotationX -= pitch;
            currentRotationX = Mathf.Clamp(currentRotationX, -90, 90);
            rifleTransformParent.localRotation = Quaternion.Euler(currentRotationX, 0, 0);
            transform.localRotation = Quaternion.Euler(0, currentRotationY, 0);
        }

        public void SetAmmoText()
        {
            ammoText.text = currentAmmoNumber.ToString();
        }
    }
}