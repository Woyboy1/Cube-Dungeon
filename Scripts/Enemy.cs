using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Instance:
    public static Enemy enemyInstance;

    public static bool bossIsActive;


    // ===========================
    // Movements:
    [Header("Movements & Others")]
    public GameObject playerTarget;
    public float movementSpeed;
    public float jumpMultiplier;
    public int numberOfHits = 3;
    public float fallingForce = 0f;

    public bool makeEnemyStationary = false;

    // ===========================
    // Neccessary Objects:
    [Header("Necessary Objects:")]
    public LayerMask enemyLayer;
    public LayerMask groundLayer;
    public Transform groundCheck;

    private float checkRadius = 0.24f;
    private bool isOnEnemy;
    private bool isGrounded; 
    private Rigidbody2D rb;
    private float distance;
    private SpriteRenderer sr;

    // ==========================
    // Enemy Health:
    [Header("Enemy Health and Stats:")]
    public int currentHealth = 5;
    public int killingHealth = 0;
    public Color enemyColor;
    
    // ==========================

    [Header("Particle Effects")]
    public ParticleSystem jumpingParticles;
    public ParticleSystem deathParticles;

    [Header("BOSS ONLY")]
    public bool isBoss = false;
    private float bossCurrentTimer = 0f;

    // ==========================
    // Timer:
    [Header("Random Jumping Interval:")]
    [TextArea]
    public string note = "rangeNumOne must be greater than 0 and must " +
        "be less than rangeNumTwo.";
    public float rangeNumOne;
    public float rangeNumTwo;
    private float endTime = 0f;
    private float startTime = 0f;

    // ===========================

    public static bool isHit = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        CheckEnemySpeed();
        if (isBoss)
        {
            bossIsActive = true;
            movementSpeed = 0;
        }


        endTime = Random.Range(rangeNumOne, rangeNumTwo);
    }

    void Update()
    {
        // Jump Timer =============
        startTime += 1 * Time.deltaTime;

        // Boss Timer ==========
        bossCurrentTimer += 1 * Time.deltaTime;

        if (bossCurrentTimer >= 2.5)
        {
            movementSpeed = 3.5f;
        }

        EnemyDies();
    }

    private void FixedUpdate()
    {
        Jumping();
        MoveTowardsPlayer();
    }

    // ------------------------------------------------
    // Getters:

    public int getCurrentHealth()
    {
        return currentHealth;
    }

    // ------------------------------------------------
    // Setters:

    public int setCurrentHealth(int newCurrentHealth)
    {
        return currentHealth = newCurrentHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            AudioManager.instance.PlaySFX("EnemyGetsHit");
            sr.color = Color.white;
            currentHealth -= 1;
            StartCoroutine(ChangeColorBack());
        }
    }

    IEnumerator ChangeColorBack()
    {
        yield return new WaitForSeconds(0.1f);
        sr.color = enemyColor;
    }

    public void MoveTowardsPlayer()
    {
        distance = Vector2.Distance(transform.position, playerTarget.transform.position); // checking distances between 2 points
        Vector2 direction = playerTarget.transform.position - transform.position;
        direction.Normalize();

        transform.position = Vector2.MoveTowards(this.transform.position, playerTarget.transform.position, movementSpeed * Time.deltaTime);
    }
    public void Jumping()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        
        if (isGrounded)
        {
            if (startTime > endTime)
            {
                AudioManager.instance.PlaySFX("EnemyJumps");
                Instantiate(jumpingParticles, groundCheck.transform.position, Quaternion.identity);
                rb.velocity = Vector2.up * jumpMultiplier;
                startTime = 0f;
            }
        } else if (!isGrounded)
        {
            rb.velocity += Vector2.down * fallingForce * Time.deltaTime;
        }
    }

    public void EnemyDies()
    {
        if (currentHealth <= killingHealth)
        {
            // Debug.Log("Enemy died");
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            EndLevelFlag.globalEnemyCount--;
            if(isBoss) 
            {
                TheLastScript.bossIsDead = true;
            }
        }
    }

    public void CheckEnemySpeed()
    {
        if (!makeEnemyStationary)
        {
            movementSpeed = Random.Range(2.5f, 6.5f);
        }
        if (makeEnemyStationary)
        {
            movementSpeed = 0;
        } 
    }
}
