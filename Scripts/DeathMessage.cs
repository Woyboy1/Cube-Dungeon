using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathMessage : MonoBehaviour
{
    public TextMeshProUGUI deathText;
    

    private void Start()
    {
        EndLevelFlag.stagesPassed -= 1;

        deathText.GetComponent<TextMeshProUGUI>().text = "Congratulations, you passed " + EndLevelFlag.stagesPassed + " stages. Wanna" +
            "give it another go?";
    }
}
