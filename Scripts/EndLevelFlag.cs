using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelFlag : MonoBehaviour
{

    [Header("Enemy Count")]
    public int enemiesInScene;
    private bool bossRequired = false;

    [Header("Debugging:")]
    public bool debugging = false;
    public int sceneNum;

    public static int globalEnemyCount;

    public static int stagesPassed = 0;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
        globalEnemyCount = enemiesInScene;
    }

    private void Update()
    {
        Debug.Log(stagesPassed);

        if (stagesPassed >= 7) // ============================= BOSS TIME WOOOO
        {
            bossRequired = true;
        }

        if (globalEnemyCount <= 0)
        {
            sr.enabled = true;
        }
    }

    public void LoadNextScene()
    {
        int randomNum = Random.Range(1, 9); ;
        int sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;

        if (randomNum == sceneBuildIndex && debugging == false)
        {
            randomNum = Random.Range(1, 9);
        } else if (randomNum != sceneBuildIndex && debugging == false)
        {
            SceneManager.LoadScene(randomNum);
        } else if (debugging)
        {
            SceneManager.LoadScene(sceneNum);
        }

        if (bossRequired)
        {
            AudioManager.instance.StopSound("Music");
            AudioManager.instance.PlaySFX("BossMusic");
            SceneManager.LoadScene("Map9");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (globalEnemyCount <= 0)
            {
                LoadNextScene();
                globalEnemyCount = enemiesInScene;
                stagesPassed++;
            }
            else
            {
                Debug.Log(globalEnemyCount + " enemies remaining");
            }
        }
    }
}
