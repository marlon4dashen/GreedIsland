using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{

    void Start()
    {
        GameEvents.current.OnCursorEnter += DrawCursor;
        GameEvents.current.OnCursorExit += HideCursor;
    }

    private void DrawCursor(Vector3 pos){
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        transform.position = pos;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = (int) SortingOrders.Cursor;
    }

    private void HideCursor() {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }
}
