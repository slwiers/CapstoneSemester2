using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KlaySpriteChange : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public Sprite KlayV2;
    public Sprite KlayV3;
    public Sprite KlayV4;
    public Sprite KlayV5;
    public Sprite KlayV6;
    public Sprite KlayV7;
    public Sprite KlayV8;
    public Sprite KlayV9;

    public RoomStateManager roomStateManager;

    private void Awake()
    {
        spriteRenderer = GameObject.Find("Klay").GetComponent<SpriteRenderer>();

        CheckV2();
        CheckV3();
        CheckV4();
        CheckV5();
        CheckV6();
        CheckV7();
        CheckV8();
        CheckV9();
    }

    public void CheckV2()
    {
        RoomStateManager instance = FindAnyObjectByType<RoomStateManager>();
        if (instance.changeKlayV2)
        {
            spriteRenderer.sprite = KlayV2;
            Debug.Log("V2 Swapped");
        }
    }

    public void CheckV3()
    {
        RoomStateManager instance = FindAnyObjectByType<RoomStateManager>();
        if (instance.changeKlayV3)
        {
            spriteRenderer.sprite = KlayV3;
            Debug.Log("V3 Swapped");
        }
    }

    public void CheckV4()
    {
        RoomStateManager instance = FindAnyObjectByType<RoomStateManager>();
        if (instance.changeKlayV4)
        {
            spriteRenderer.sprite = KlayV4;
            Debug.Log("V4 Swapped");
        }
    }

    public void CheckV5()
    {
        RoomStateManager instance = FindAnyObjectByType<RoomStateManager>();
        if (instance.changeKlayV5)
        {
            spriteRenderer.sprite = KlayV5;
            Debug.Log("V5 Swapped");
        }
    }

    public void CheckV6()
    {
        RoomStateManager instance = FindAnyObjectByType<RoomStateManager>();
        if (instance.changeKlayV6)
        {
            spriteRenderer.sprite = KlayV6;
            Debug.Log("V6 Swapped");
        }
    }

    public void CheckV7()
    {
        RoomStateManager instance = FindAnyObjectByType<RoomStateManager>();
        if (instance.changeKlayV7)
        {
            spriteRenderer.sprite = KlayV7;
            Debug.Log("V7 Swapped");
        }
    }

    public void CheckV8()
    {
        RoomStateManager instance = FindAnyObjectByType<RoomStateManager>();
        if (instance.changeKlayV8)
        {
            spriteRenderer.sprite = KlayV8;
            Debug.Log("V8 Swapped");
        }
    }

    public void CheckV9()
    {
        RoomStateManager instance = FindAnyObjectByType<RoomStateManager>();
        if (instance.changeKlayV9)
        {
            spriteRenderer.sprite = KlayV9;
            Debug.Log("V9 Swapped");
        }
    }

}
