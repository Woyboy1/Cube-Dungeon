using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyTurret : MonoBehaviour
{
    //=================
    // Shooting:
    [Header("Shooting:")]
    public GameObject enemyBullet;
    public Transform shootingPosition;
    public float fireForce;

    // ================
    // Shooting Time Interval:
    [Header("Random Shooting Interval")]
    [TextArea]
    public string note = "randonNumOne must be greater than 0 and less than" +
        "randomNumTwo";
    public bool useCustomInterval = false;
    public float randomNumOne;
    public float randomNumTwo;
    // ================

    [Header("BOSS ONLY")]
    public bool isBoss = false;
    public GameObject throwEnemyPrefab;

    // ================
    // TIMER 
    private float currentTime;
    private float secondCurrentTime = 5f; 
    // ================

    // =============== 
    // Private:
    private Transform playerTarget;
    private float timeTillShoot = 0f;
    private Rigidbody2D rb;
    // ===============

    void Start()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();

        CheckTimedShots();
    }

    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        secondCurrentTime -= 1 * Time.deltaTime;

        ShootGun();

        if (isBoss)
        {
            ThrowEnemyPrefab();
        }
    }

    private void FixedUpdate()
    {
        HandleRotations();
    }

    private void ShootGun()
    {
        if (currentTime <= timeTillShoot)
        {
            AudioManager.instance.PlaySFX("EnemyShoots");
            GameObject projectile = Instantiate(enemyBullet, shootingPosition.transform.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().AddForce(shootingPosition.up * fireForce, ForceMode2D.Impulse);
            CheckTimedShots();
        }
    }

    private void ThrowEnemyPrefab()
    {
        if (secondCurrentTime <= 0)
        {
            AudioManager.instance.PlaySFX("BossThrow");
            GameObject enemyPrefab = Instantiate(throwEnemyPrefab, shootingPosition.transform.position, Quaternion.identity);
            enemyPrefab.GetComponent<Rigidbody2D>().AddForce(shootingPosition.up * fireForce, ForceMode2D.Impulse);
            secondCurrentTime = 5f;
        }
    }

    private void HandleRotations()
    {
        Vector3 direction = playerTarget.position - this.transform.position; 
        float angle = Mathf.Atan2(direction.y, direction.x); 
        this.transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg +- 90f);
    }

    private void CheckTimedShots()
    {
        if (useCustomInterval)
        {
            currentTime = Random.Range(randomNumOne, randomNumTwo);
        } else if (!useCustomInterval)
        {
            currentTime = Random.Range(2.5f, 5.0f); 
        }
    }
}
