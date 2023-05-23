using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public void TutorialButton()
    {
        SceneManager.LoadScene("Tutorial");
    }

    #region PLAY BUTTON!
    public void PlayButton()
    {
        EndLevelFlag.stagesPassed = 0;
        SceneManager.LoadScene("Map4");
    }
    #endregion

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }

}
