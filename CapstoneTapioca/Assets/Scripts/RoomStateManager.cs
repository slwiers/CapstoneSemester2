using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomStateManager : MonoBehaviour
{
    public static RoomStateManager Instance;

    public bool valeDamDown = false;

    public bool matrixUp = false;

    public bool changeKlayV2;
    public bool changeKlayV3;
    public bool changeKlayV4;
    public bool changeKlayV5;
    public bool changeKlayV6;
    public bool changeKlayV7;
    public bool changeKlayV8;
    public bool changeKlayV9;
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
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (valeDamDown && currentSceneIndex == 3)
        {
            GameObject valeDam = GameObject.Find("Dam");
            Transform childTransform = valeDam.transform.Find("DamComponents");
            childTransform.gameObject.SetActive(false);
        }

        if (matrixUp && currentSceneIndex == 2)
        {
            GameObject matrixBackrgound = GameObject.Find("Matrix");
            Transform childTransform = matrixBackrgound.transform.Find("MatrixBackground");
            childTransform.gameObject.SetActive(true);
        }
        if (!matrixUp && currentSceneIndex == 2)
        {
            GameObject martixBackrgound = GameObject.Find("Matrix");
            Transform childTransform = martixBackrgound.transform.Find("MatrixBackground");
            childTransform.gameObject.SetActive(false);
        }
    }

}
