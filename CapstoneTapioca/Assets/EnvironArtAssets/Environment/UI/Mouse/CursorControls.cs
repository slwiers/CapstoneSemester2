using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorControls : MonoBehaviour
{
    public Texture2D cursorArrow;
    public Texture2D cursorArrowUpdate;
    public Texture2D cursorArrowHover;
    private Camera _camera;

    private void Start()
    {
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
        _camera = Camera.main;

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(cursorArrowUpdate, Vector2.zero, CursorMode.ForceSoftware);
        }
        if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
        }
    }

    public void OnMouseEnter()
    {
        Cursor.SetCursor(cursorArrowHover, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void OnMouseExit()
    {
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
    }
}
