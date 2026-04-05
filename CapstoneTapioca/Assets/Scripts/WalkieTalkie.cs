using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkieTalkie : MonoBehaviour
{
    public GameObject dialBox;
    public List<GameObject> dialBoxText;
    private GameObject activeText;

    public float turnOn = 5f;
    public float turnOff = 10f;

    private void Start()
    {
        if (dialBox == null)
        {
            dialBox = this.gameObject;
        }

        StartCoroutine(StartTimer());
    }
    IEnumerator StartTimer()
    {

        while (true)
        {
            int randomIndex = Random.Range(0, dialBoxText.Count);
            GameObject activeText = dialBoxText[randomIndex];

            dialBox.SetActive(false);
            yield return new WaitForSeconds(turnOff);

            dialBox.SetActive(true);
            activeText.SetActive(true);
            yield return new WaitForSeconds(turnOn);


            activeText.SetActive(false);

        }

    }

}
