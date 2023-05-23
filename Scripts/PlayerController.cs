using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{

    public static bool playerInvincibility = false;

    // ====================
    // Movement Input:
    [Header("Movement Input Variables:")]
    public float movementSpeed;
    public float jumpForce;
    public float fallingForce;
    public float checkRadius;
    // ====================

    // ====================
    // Neccessary Objects:
    [Header("Necessary Objects:")]
    public LayerMask groundLayer; 
    public Transform groundCheck;
    // ====================

    // ====================
    // Player Stats and Healtht:
    [Header("Stats and Health:")]
    public int currentHealth = 5;
    public int killingHealth = 0;

    //=====================
    // Shooting:
    [Header("Shooting Variables")]
    public GameObject bulletPrefab;
    public GameObject grenadePrefab;
    public Transform shootingPosition;
    public float fireForce;
    public GameObject weapon;
    public float grenadeForce;
    public int bulletCount = 32;
    public int grenadeCount = 4;

    //=====================

    //=====================
    // Particles:
    [Header("Particles")]
    public ParticleSystem jumpParticleEffects;
    public ParticleSystem deathEffects;

    // =================================
    // private neccessities:
    private float moveInput;
    private bool isGrounded;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    // Sad Timer:
    public float currentTIme = 0;
    public float otherCurrentTime = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                AudioManager.instance.PlaySFX("PlayerJump");
                rb.velocity = Vector2.up * jumpForce;
                Instantiate(jumpParticleEffects, groundCheck.transform.position, Quaternion.identity);
            }
        } else if (!isGrounded)
        {
            rb.velocity += Vector2.down * fallingForce * Time.deltaTime;
            // Debug.Log(rb.velocity);
        }

        ShootWeapon();
        ThrowGrenade();
        RotateWeapon();
        PlayerDies();

        
        currentTIme += 1 * Time.deltaTime;
        otherCurrentTime += 1 * Time.deltaTime;
        GenerateAmmoConstantly();

        if (playerInvincibility)
        {
            currentHealth = 1000;
        }
    }

    private void FixedUpdate()
    {
        // returns true or false made in a circle 
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // returns 1 or 0
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * movementSpeed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
        {
            currentHealth -= 1;
            sr.color = Color.yellow;
            StartCoroutine(ChangeColorBack());
            AudioManager.instance.PlaySFX("PlayerGetsHit");
        }

        if (collision.gameObject.tag == "Spike")
        {
            sr.color = Color.yellow;
            PlayerTakesDamageFromSpike();
            StartCoroutine(ChangeColorBack());
            AudioManager.instance.PlaySFX("PlayerGetsHit");
        }
    }
    IEnumerator ChangeColorBack()
    {
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }

    private void ShootWeapon()
    {
        if (bulletCount > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject bulletProjectile = Instantiate(bulletPrefab, shootingPosition.transform.position, Quaternion.identity);
                bulletProjectile.GetComponent<Rigidbody2D>().AddForce(shootingPosition.up * fireForce, ForceMode2D.Impulse);
                FindObjectOfType<AudioManager>().PlaySFX("PlayerGun");
                bulletCount--;
            }
        } else if (bulletCount <= 0)
        {
            Debug.Log("NO AMMO");
        }
    }

    private void ThrowGrenade()
    {
        if (grenadeCount > 0)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                GameObject grenadeProjectile = Instantiate(grenadePrefab, shootingPosition.transform.position, Quaternion.identity);
                grenadeProjectile.GetComponent<Rigidbody2D>().AddForce(shootingPosition.up * grenadeForce, ForceMode2D.Impulse);
                AudioManager.instance.PlaySFX("ThrowGrenade");
                grenadeCount--;
            }
        }
    }

    private void RotateWeapon()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - weapon.transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        weapon.transform.rotation = Quaternion.Euler(0f, 0f, rotZ +- 90f);
    }

    public void PlayerDies()
    {
        if (currentHealth <= killingHealth)
        {
            // Debug.Log("Player Died");
            Instantiate(deathEffects, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            // RANDOM SPAWN!
            if (Enemy.bossIsActive == true)
            {
                AudioManager.instance.StopSound("BossMusic");
                AudioManager.instance.PlaySFX("Music");
                SceneManager.LoadScene("MainMenu");
            } else
            {
                SceneManager.LoadScene("DeathScreen");
            }
        }
    }

    public void PlayerTakesDamageFromSpike()
    {
        currentHealth -= 2;
    }

    public void GenerateAmmoConstantly()
    {
        if (currentTIme >= 2)
        {
            AudioManager.instance.PlaySFX("AmmoRefill");
            bulletCount += 4; 
            currentTIme = 0;
        }

        if (otherCurrentTime >= 4)
        {
            AudioManager.instance.PlaySFX("GrenadeRefill");
            grenadeCount++;
            otherCurrentTime = 0;
        }
    }
}
