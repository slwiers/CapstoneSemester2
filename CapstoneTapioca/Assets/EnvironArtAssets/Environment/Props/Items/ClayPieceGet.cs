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
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        ClayPieceManagement instance = FindAnyObjectByType<ClayPieceManagement>();

        if (instance.triggerClayPiece1 == true)
        {
            if (GameObject.Find("ClayTrigger1"))
            {
                GameObject clayTrigger = transform.GetChild(0).gameObject;
                clayTrigger.SetActive(true);
            }

        }

        if (instance.triggerClayPiece2 == true)
        {
            if (GameObject.Find("ClayTrigger2"))
            {
                GameObject clayTrigger = transform.GetChild(0).gameObject;
                clayTrigger.SetActive(true);
            }
        }

        if (instance.triggerClayPiece3 == true)
        {
            
            if (GameObject.Find("ClayTrigger3"))
            {
                GameObject clayTrigger = transform.GetChild(0).gameObject;
                clayTrigger.SetActive(true);

                if (instance.triggerClayPiece3 == true && GameObject.Find("ClayTrigger9"))
                {
                    gameObject.SetActive(false);
                    Debug.Log("3");
                }
            }

        }

        if (instance.triggerClayPiece4 == true)
        {
            if (GameObject.Find("ClayTrigger4"))
            {
                GameObject clayTrigger = transform.GetChild(0).gameObject;
                clayTrigger.SetActive(true);
            }
        }

        if (instance.triggerClayPiece5 == true)
        {
            if (GameObject.Find("ClayTrigger5"))
            {
                GameObject clayTrigger = transform.GetChild(0).gameObject;
                clayTrigger.SetActive(true);
            }

        }

        if (instance.triggerClayPiece6 == true)
        {
            if (GameObject.Find("ClayTrigger6"))
            {
                GameObject clayTrigger = transform.GetChild(0).gameObject;
                clayTrigger.SetActive(true);
            }
        }

        if (instance.triggerClayPiece7 == true)
        {
            if (GameObject.Find("ClayTrigger7"))
            {
                GameObject clayTrigger = transform.GetChild(0).gameObject;
                clayTrigger.SetActive(true);
            }
        }
        if (instance.triggerClayPiece8 == true)
        {
            if (GameObject.Find("ClayTrigger8"))
            {
                GameObject clayTrigger = transform.GetChild(0).gameObject;
                clayTrigger.SetActive(true);
            }
        }


        if (instance.triggerClayPiece9 == true && currentSceneIndex == 8)
        {
            GameObject parentObject = GameObject.Find("ClayTrigger9");
            Transform childTransform = parentObject.transform.Find("ClayPiece9");

            if (childTransform)
            {
                childTransform.gameObject.SetActive(true);

                //if (instance.triggerClayPiece3 == true && GameObject.Find("ClayTrigger3"))
                //{
                //    gameObject.SetActive(false);
                //}
            }

        }

    }    

}

