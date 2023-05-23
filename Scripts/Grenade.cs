using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class Grenade : MonoBehaviour
{
    Collider2D[] inExplosionRadius = null; // empty for now because there is nothing inside the collider.
    [Header("Core Items:")]
    [SerializeField] private float explosionForceMultiplier = 5f;
    [SerializeField] private float explosionRadius = 5f;

    [Header("Grenade Statistics:")]
    public ParticleSystem explosionEffect;

    // =============================
    // custom timer:
    private float currentTime = 1.5f;
    private float endTime = 0f;

    private void Start()
    {
        
    }

    private void Update()
    {
        currentTime -= 1 * Time.deltaTime;

        if (currentTime <= endTime)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Explosion();
            AudioManager.instance.PlaySFX("GrenadeExplosion");
            Destroy(gameObject);
        }
    }
    private void Explosion()
    {
        inExplosionRadius = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D o in inExplosionRadius)
        {
            Rigidbody2D o_rb = o.GetComponent<Rigidbody2D>();

            if (o_rb != null)
            {
                Vector2 distance = o.transform.position - transform.position;
                if (distance.magnitude > 0) // so you will not get NaN error 
                {
                    float explosionForce = explosionForceMultiplier / distance.magnitude;
                    o_rb.AddForce(distance.normalized * explosionForce);
                }
            }
        }
    }

    private void OnDrawGizmos() // draw gizmos
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
