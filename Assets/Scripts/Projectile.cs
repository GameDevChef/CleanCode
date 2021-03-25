
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage;
    private Rigidbody rigidody;
    private AudioClip impactClip;

    private void Awake()
    {
        rigidody = GetComponent<Rigidbody>();
    }
    public void Init(ProjectileWeapon weapon)
    {
        this.damage = weapon.GetWeaponDamage();
        this.impactClip = weapon.GetImpactClip();
        rigidody.velocity = transform.forward * weapon.GetProjectileSpeed();

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.TryGetComponent(out EnemyManager enemy))
        {
            enemy.OnEnemyShot(transform.forward, other.GetComponent<Rigidbody>(), damage);
            AudioSource.PlayClipAtPoint(impactClip, transform.position);
            Destroy(gameObject);
        }
    }


}
