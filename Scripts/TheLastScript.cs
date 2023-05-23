using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TheLastScript : MonoBehaviour
{
    public static bool bossIsDead = false;

    private void Update()
    {
        if (bossIsDead)
        {
            PlayerController.playerInvincibility = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            if (bossIsDead)
            {
                SceneManager.LoadScene("EndScreen");
            }
        }
    }
}
