using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    public static WinCondition Instance;

    public bool savedNPC1 = false;
    public bool savedNPC2 = false;
    public bool savedNPC3 = false;
    public bool savedNPC4 = false;
    public bool savedNPC5 = false;

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
        if (savedNPC1 && savedNPC2 && savedNPC3 && savedNPC4 && savedNPC5)
        {
            LevelLoader instance = FindAnyObjectByType<LevelLoader>();
            instance.LoadLevel(17);
            savedNPC1 = false;
            savedNPC2 = false;
            savedNPC3 = false;
            savedNPC4 = false;
            savedNPC5 = false;
        }
    }
}
