using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyNameGenerator : MonoBehaviour
{
    public GameObject enemyNameObject;
    private string nameInput = null;

    public string[] enemyNames; 

    // Start is called before the first frame update
    void Start()
    {
        CreateRandomName();
    }

    private void CreateRandomName()
    {
        int index = 0;
        index = Random.Range(0, enemyNames.Length);

        enemyNameObject.GetComponent<TextMeshPro>().text = enemyNames[index];
    }
}
