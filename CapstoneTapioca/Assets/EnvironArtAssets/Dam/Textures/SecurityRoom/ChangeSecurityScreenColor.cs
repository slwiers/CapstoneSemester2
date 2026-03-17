using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSecurityScreenColor : MonoBehaviour
{
    public float delayTime = 3f;
    public float repeat = 3f;

    private void Start()
    {
        InvokeRepeating("ChangeColor", delayTime, repeat);

    }

    public void ChangeColor()
    {
        Renderer renderer = GetComponent<Renderer>();

        Color color = new Color(
            Random.Range(0f, 1f), // Red
            Random.Range(0f, 1f), // Green
            Random.Range(0f, 1f), // Blue
            1f                    // Alpha
        );

        renderer.material.color = color;
    }

}
