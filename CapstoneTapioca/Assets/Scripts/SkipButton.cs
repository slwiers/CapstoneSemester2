using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipButton : MonoBehaviour
{
    public GameObject HourHand;
    public GameObject MinuteHand;

    public float delayTime = 5f; //amount of time the timer is going to wait (change this in engine if you need to change it)
    public GameObject YouWin; //the text to appear upon winning
    public void ClockWin()
    {
        HourHand.SetActive(false);
        MinuteHand.SetActive(false);
        YouWin.SetActive(true); //sets the text active
        StartCoroutine(waitForSceneChangeClock()); //calls the timer function and waits to change the scene for a few seconds
    }

    public void MinesWin()
    {
        YouWin.SetActive(true); //sets the text active
        StartCoroutine(waitForSceneChangeMines()); //calls the timer function and waits to change the scene for a few seconds
    }

    public void PipesWin()
    {
        YouWin.SetActive(true); //sets the text active
        StartCoroutine(waitForSceneChangePipes()); //calls the timer function and waits to change the scene for a few seconds
    }

    private IEnumerator waitForSceneChangeClock() //function for the timer to be called as
    {
        yield return new WaitForSeconds(delayTime); //starts the timer
        {
            SceneManager.LoadScene("SecurityRoom5"); //loads the next scene
        }
    }

    private IEnumerator waitForSceneChangeMines() //function for the timer to be called as
    {
        yield return new WaitForSeconds(delayTime); //starts the timer
        {
            SceneManager.LoadScene("ServerRoom2"); //loads the next scene
        }
    }

    private IEnumerator waitForSceneChangePipes() //function for the timer to be called as
    {
        yield return new WaitForSeconds(delayTime); //starts the timer
        {
            SceneManager.LoadScene("DamBeachRoom3"); //loads the next scene
        }
    }
}