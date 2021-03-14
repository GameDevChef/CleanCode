using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private Animator animator;
    private int curHealth;
    private Rigidbody[] rigidbodies;

    private void Awake()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        curHealth = maxHealth;
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = true;
            rb.interpolation = RigidbodyInterpolation.None;

        }
    }

    public void OnEnemyShot(Vector3 shootDirection, Rigidbody shotRB, int damage)
    {
        curHealth -= damage;
        animator.enabled = false;
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
        if (shotRB)
        {
            shotRB.AddForce(shootDirection.normalized * 100f, ForceMode.Impulse);
        }
    }
}


