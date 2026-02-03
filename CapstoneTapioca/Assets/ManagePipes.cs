using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagePipes : MonoBehaviour
{
    public GameObject PipesHolder;
    public GameObject[] Pipes;

    [SerializeField]
    int totalPipes = 0;

    int correctedPipes = 0;

    public float delayTime = 5f; //amount of time the timer is going to wait (change this in engine if you need to change it)
    public GameObject YouWin; //the text to appear upon winning

    // Start is called before the first frame update
    void Start()
    {
        totalPipes = PipesHolder.transform.childCount;

        Pipes = new GameObject[totalPipes];

        for (int i = 0; i < Pipes.Length; i++)
        {
            Pipes[i] = PipesHolder.transform.GetChild(i).gameObject;
        }
    }

  public void correctMove()
    {
        correctedPipes += 1;
        Debug.Log("Correct Move");

        if(correctedPipes == totalPipes)
        {
            Debug.Log("You Win!");
            YouWin.SetActive(true); //sets the text active
            StartCoroutine(waitForSceneChange()); //calls the timer function and waits to change the scene for a few seconds
        }
    }

    public void wrongMove()
    {
        correctedPipes -= 1;
    }

    private IEnumerator waitForSceneChange() //function for the timer to be called as
    {
        yield return new WaitForSeconds(delayTime); //starts the timer
        {
            SceneManager.LoadScene("WaterRoom7"); //loads the next scene
        }
    }
}
