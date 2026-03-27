using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClayPieceGet : MonoBehaviour
{

    private void Update()
    {
        //GameObject clayTrigger = GameObject.Find("ClayTrigger");

        ClayPieceManagement instance = FindAnyObjectByType<ClayPieceManagement>();

        if (instance.triggerClayPiece1 == true)
        {
            GameObject clayTrigger = transform.GetChild(0).gameObject;
            clayTrigger.SetActive(true);

        }

        if (instance.triggerClayPiece2 == true)
        {
            GameObject clayTrigger = transform.GetChild(0).gameObject;
            clayTrigger.SetActive(true);

        }

        if (instance.triggerClayPiece3 == true)
        {
            GameObject clayTrigger = GameObject.FindGameObjectWithTag("Clay");
            clayTrigger.transform.GetChild(0).gameObject.SetActive(true);

        }

        if (instance.triggerClayPiece4 == true)
        {
            GameObject clayTrigger = GameObject.FindGameObjectWithTag("Clay");
            clayTrigger.transform.GetChild(0).gameObject.SetActive(true);

        }

        if (instance.triggerClayPiece5 == true)
        {
            GameObject clayTrigger = GameObject.FindGameObjectWithTag("Clay");
            clayTrigger.transform.GetChild(0).gameObject.SetActive(true);

        }

        if (instance.triggerClayPiece6 == true)
        {
            GameObject clayTrigger = GameObject.FindGameObjectWithTag("Clay");
            clayTrigger.transform.GetChild(0).gameObject.SetActive(true);

        }

        if (instance.triggerClayPiece7 == true)
        {
            GameObject clayTrigger = GameObject.FindGameObjectWithTag("Clay");
            clayTrigger.transform.GetChild(0).gameObject.SetActive(true);

        }
        if (instance.triggerClayPiece8 == true)
        {
            GameObject clayTrigger = GameObject.FindGameObjectWithTag("Clay");
            clayTrigger.transform.GetChild(0).gameObject.SetActive(true);

        }

        if (instance.triggerClayPiece9 == true)
        {
            GameObject clayTrigger = GameObject.FindGameObjectWithTag("Clay");
            clayTrigger.transform.GetChild(0).gameObject.SetActive(true);

        }

    }    

}

