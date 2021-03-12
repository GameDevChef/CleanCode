using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameDevChef.DirtyCode
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private Weapon[] weapons;
        [SerializeField] private Transform rifleTransParent;
        [SerializeField] private float walkSpeed;
        [SerializeField] private float runSpeed;
        [SerializeField] private float rotSpeed;
        [SerializeField] private float mouseSens;
        [SerializeField] private AudioSource source;

        private Rigidbody rb;
        private Weapon holdWeapon;
        private int currentWeaponIndex;
        private float curAmmo;
        private float curWaitTime;
        private float curRotationY;
        private float curRotationX;
        private bool running;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            SetActiveWeaponAndInitializeVariables();
        }

        private void SetActiveWeaponAndInitializeVariables()
        {
            holdWeapon = weapons[currentWeaponIndex];
            foreach (var weapon in weapons)
            {
                weapon.gameObject.SetActive(false);
            }
            holdWeapon.gameObject.SetActive(true);
            curAmmo = 20;
            curRotationY = transform.eulerAngles.y;
            curRotationX = transform.eulerAngles.x;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void FixedUpdate()
        {
            float moveSpeed;
            var moveVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            var worldMoveVector = transform.TransformDirection(moveVector);
            worldMoveVector = new Vector3(worldMoveVector.x, 0f, worldMoveVector.z);
            if (running)
            {
                moveSpeed = runSpeed;
            }
            else
            {
                moveSpeed = walkSpeed;
            }

            rb.velocity = worldMoveVector.normalized * moveSpeed;
        }

        private void Update()
        {
            if(Input.mouseScrollDelta.y < 0)
            {
                currentWeaponIndex--;
                if (currentWeaponIndex < 0)
                    currentWeaponIndex = weapons.Length - 1;
                holdWeapon = weapons[currentWeaponIndex];
                foreach (var weapon in weapons)
                {
                    weapon.gameObject.SetActive(false);
                }
                holdWeapon.gameObject.SetActive(true);
            }

            if (Input.mouseScrollDelta.y > 0)
            {
                currentWeaponIndex++;
                if (currentWeaponIndex > weapons.Length - 1)
                    currentWeaponIndex = 0;
                holdWeapon = weapons[currentWeaponIndex];
                foreach (var weapon in weapons)
                {
                    weapon.gameObject.SetActive(false);
                }
                holdWeapon.gameObject.SetActive(true);
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                running = true;
            }
            else
            {
                running = false;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                curWaitTime = 0f;
                curAmmo = 20;
                source.clip = holdWeapon.GetReloadSound();
                source.Stop();
                source.Play();

            }

            Rotate();

            curWaitTime += Time.deltaTime;
            if (Input.GetMouseButton(0) && curWaitTime > holdWeapon.GetShootingInterval() && !HasNotEnoughAmmo())
            {
                curWaitTime = 0;
                curAmmo--;
                switch (holdWeapon.GetWeaponType())
                {
                    case WeaponType.Pistol:
                        source.clip = holdWeapon.GetShotSound();
                        source.Stop();
                        source.Play();
                        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out RaycastHit hit1, holdWeapon.GetWeaponRange()))
                        {
                            EnemyManager controller = hit1.collider.GetComponentInParent<EnemyManager>();
                            Vector3 direction = hit1.point - holdWeapon.GetBulletSpawnTransform().position;
                            if (controller)
                            {
                                controller.OnEnemyShot(direction, hit1.collider.GetComponent<Rigidbody>(), holdWeapon.GetWeaponDamage());
                            }
                        }
                        break;
                    case WeaponType.Rilfe:
                        source.clip = holdWeapon.GetShotSound();
                        source.Stop();
                        source.Play();
                        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out RaycastHit hit2, holdWeapon.GetWeaponRange()))
                        {
                            EnemyManager controller = hit2.collider.GetComponentInParent<EnemyManager>();
                            Vector3 direction = hit2.point - holdWeapon.GetBulletSpawnTransform().position;
                            if (controller)
                            {
                                controller.OnEnemyShot(direction, hit2.collider.GetComponent<Rigidbody>(), holdWeapon.GetWeaponDamage());
                            }
                        }
                        break;
                    case WeaponType.RPG:
                        source.clip = holdWeapon.GetShotSound();
                        source.Stop();
                        source.Play();
                        Projectile projectile = Instantiate(holdWeapon.GetProjectilePrefab(), 
                            holdWeapon.GetBulletSpawnTransform().position, 
                            holdWeapon.GetBulletSpawnTransform().rotation);
                        projectile.Init(holdWeapon.GetWeaponDamage(), holdWeapon.GetProjectileSpeed(), holdWeapon.GetImpactClip());                       
                        break;
                    default:
                        break;
                }
               
            }
        }

        private bool HasNotEnoughAmmo()
        {
            return curAmmo <= 0;
        }

        private void Rotate()
        {
            float yaw = Input.GetAxis("Mouse X") * Time.deltaTime * rotSpeed * mouseSens;
            float pitch = Input.GetAxis("Mouse Y") * Time.deltaTime * rotSpeed * mouseSens;
            curRotationY += yaw;
            curRotationX -= pitch;
            curRotationX = Mathf.Clamp(curRotationX, -90, 90);
            rifleTransParent.localRotation = Quaternion.Euler(curRotationX, 0, 0);
            transform.localRotation = Quaternion.Euler(0, curRotationY, 0);
        }
    }
}