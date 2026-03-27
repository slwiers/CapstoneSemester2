using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClayPieceManagement : MonoBehaviour
{
    public static ClayPieceManagement Instance;

    public bool triggerClayPiece1 = false;
    public bool killClayPiece1 = false;

    public bool triggerClayPiece2 = false;
    public bool killClayPiece2 = false;

    public bool triggerClayPiece3 = false;
    public bool killClayPiece3 = false;

    public bool triggerClayPiece4 = false;
    public bool killClayPiece4 = false;

    public bool triggerClayPiece5 = false;
    public bool killClayPiece5 = false;

    public bool triggerClayPiece6 = false;
    public bool killClayPiece6 = false;

    public bool triggerClayPiece7 = false;
    public bool killClayPiece7 = false;

    public bool triggerClayPiece8 = false;
    public bool killClayPiece8 = false;

    public bool triggerClayPiece9 = false;
    public bool killClayPiece9 = false;


    private DialogueManager dialogueManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }

    }

    private void Update()
    {
        DialogueManager instance = FindAnyObjectByType<DialogueManager>();

        if (killClayPiece1 == true)
        {
            if (!instance.dialogueIsPlaying)
            {
                GameObject clayTrigger = GameObject.FindGameObjectWithTag("Clay");
                clayTrigger.SetActive(false);
                triggerClayPiece1 = false;
                killClayPiece1 = false;
            }

        }
        

        if (killClayPiece2 == true)
            {

           if (!instance.dialogueIsPlaying)
           {
              GameObject clayTrigger = GameObject.FindGameObjectWithTag("Clay");
              clayTrigger.SetActive(false);
              triggerClayPiece2 = false;
              killClayPiece2 = false;

           }

        }
        

        if (killClayPiece3 == true)
        {
            if (!instance.dialogueIsPlaying)
            {
                GameObject clayTrigger = GameObject.FindGameObjectWithTag("Clay");
                clayTrigger.SetActive(false);
                triggerClayPiece3 = false;
                killClayPiece3 = false;
            }

        }


        if (killClayPiece4 == true)
        {
            if (!instance.dialogueIsPlaying)
            {
                GameObject clayTrigger = GameObject.FindGameObjectWithTag("Clay");
                clayTrigger.SetActive(false);
                triggerClayPiece4 = false;
                killClayPiece4 = false;
            }

        }


        if (killClayPiece5 == true)
        {
            if (!instance.dialogueIsPlaying)
            {
                GameObject clayTrigger = GameObject.FindGameObjectWithTag("Clay");
                clayTrigger.SetActive(false);
                triggerClayPiece5 = false;
                killClayPiece5 = false;
            }

        }


        if (killClayPiece6 == true)
        {
            if (!instance.dialogueIsPlaying)
            {
                GameObject clayTrigger = GameObject.FindGameObjectWithTag("Clay");
                clayTrigger.SetActive(false);
                triggerClayPiece6 = false;
                killClayPiece6 = false;
            }

        }


        if (killClayPiece7 == true)
        {
            if (!instance.dialogueIsPlaying)
            {
                GameObject clayTrigger = GameObject.FindGameObjectWithTag("Clay");
                clayTrigger.SetActive(false);
                triggerClayPiece7 = false;
                killClayPiece7 = false;
            }

        }


        if (killClayPiece8 == true)
        {
            if (!instance.dialogueIsPlaying)
            {
                GameObject clayTrigger = GameObject.FindGameObjectWithTag("Clay");
                clayTrigger.SetActive(false);
                triggerClayPiece8 = false;
                killClayPiece8 = false;
            }

        }


        if (killClayPiece9 == true)
        {
            if (!instance.dialogueIsPlaying)
            {
                GameObject clayTrigger = GameObject.FindGameObjectWithTag("Clay");
                clayTrigger.SetActive(false);
                triggerClayPiece9 = false;
                killClayPiece9 = false;
            }

        }
    }


}
