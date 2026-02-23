using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public void SceneChange()
    {
        SceneManager.LoadScene("CafeteriaRoom6");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
