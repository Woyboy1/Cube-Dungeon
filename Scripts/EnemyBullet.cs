using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("Bullet Necessities:")]
    public ParticleSystem explosionEffect; 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Environment")
        {
            Destroy(this.gameObject);
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            Enemy.isHit = true;
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(this.gameObject);
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

    }
}
