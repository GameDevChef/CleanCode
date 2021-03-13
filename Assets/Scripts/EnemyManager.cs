using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevChef.DirtyCode
{
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

        public void ImpactRigidbody(Vector3 shootDirection, Rigidbody shotRB)
        {
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

        public void TakeDamage(int damage)
        {
            curHealth -= damage;
        }
    }
}

