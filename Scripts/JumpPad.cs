using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Header("Bouncepad Statistics:")]
    public float bounceStrength = 20f;
    public ParticleSystem jumpEffects;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            Instantiate(jumpEffects, transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounceStrength, ForceMode2D.Impulse);
            AudioManager.instance.PlaySFX("PlayerJump");
        }
    }
}
