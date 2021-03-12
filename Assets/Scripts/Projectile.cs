using GameDevChef.DirtyCode;
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
    public void Init(int damage, int speed, AudioClip impactClip)
    {
        this.damage = damage;
        this.impactClip = impactClip;
        rigidody.velocity = transform.forward * speed;

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
