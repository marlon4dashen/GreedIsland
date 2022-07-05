using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{

    public static GameEvents current;

    private void Awake(){
        current = this;
    }

    public event Action<Vector3> OnCursorEnter;

    public void CursorEnter(Vector3 pos) {
        if (OnCursorEnter != null) {
            OnCursorEnter(pos);
        }
    }

    public event Action OnCursorExit;
    public void CursorExit() {
        if (OnCursorExit != null) {
            OnCursorExit();
        }
    }

    public event Action<OverlayTile> OnSelectCharacter;

    public void SelectCharacter(OverlayTile tile) {
        if (OnSelectCharacter != null) {
            OnSelectCharacter(tile);
        }
    }

    public event Action<OverlayTile, OverlayTile> OnCharacterMove;

    public void CharacterMove(OverlayTile start, OverlayTile des) {
        if (OnCharacterMove != null) {
            OnCharacterMove(start, des);
        }
    }

}
