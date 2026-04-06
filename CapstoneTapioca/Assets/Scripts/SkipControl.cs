using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipControl : MonoBehaviour
{
    public GameObject skipButton;

    public float turnOn = 60f;

    private void Start()
    {
        skipButton.SetActive(false);
        StartCoroutine(StartTimer());
    }
    IEnumerator StartTimer()
    {
         yield return new WaitForSeconds(turnOn);
         skipButton.SetActive(true);

    }
}
