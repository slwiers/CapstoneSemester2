using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ChangeLightColor : MonoBehaviour
{
    public Light targetLight;

    public float delayTime = 3f;
    public float repeat = 3f;

    private void Start()
    {
        targetLight = GetComponent<Light>();

        InvokeRepeating("ChangeColor", delayTime, repeat);
    }

    public void ChangeColor()
    {
        targetLight.color = new UnityEngine.Color(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                1f
            );
    }
}
