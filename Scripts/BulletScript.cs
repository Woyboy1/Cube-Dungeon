using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [Header("Particles")]
    public ParticleSystem explosionEffect;

    private void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Environment" || collision.gameObject.tag == "Spike")
        {
            Destroy(this.gameObject);
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(this.gameObject);
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        if  (collision.gameObject.tag == "EnemyBullet")
        {
            AudioManager.instance.PlaySFX("BulletsCollide");
            Destroy(this.gameObject);
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
    }
}
