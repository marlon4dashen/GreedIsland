using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MouseController : MonoBehaviour
{

    private static MouseController _instance;
    public static MouseController Instance {
        get => _instance;
    }
    private CharacterController charaController;
    private OverlayTile selectedTile;
    private GameEvents events;

    private void Awake(){
        _instance = this;
    }


    public void init(CharacterController characterController, GameEvents currentEvent){
        charaController = characterController;
        events = currentEvent;
        events.OnDeselect += clearSelected;
    }

    // Update is called once per frame
    public void startListen()
    {
        var focusedTileHit = GetFocusedOnTile();
        if (focusedTileHit.HasValue){
            OverlayTile currentTile = focusedTileHit.Value.collider.gameObject.GetComponent<OverlayTile>();
            events.CursorEnter(currentTile.transform.position);
            if (Input.GetMouseButtonDown(0) && !charaController.isMoving){
                if (selectedTile == null && charaController.checkCharacterOnTile(currentTile)) {
                    selectedTile = currentTile;
                    events.SelectCharacter(selectedTile);
                } else if (selectedTile != null){

                    if (charaController.checkCharacterOnTile(currentTile)) {
                        // attack or ability or select another minion on your team
                        // for now only select another minion
                        events.Deselect();
                        selectedTile = currentTile;
                        events.SelectCharacter(selectedTile);
                    } else {
                        // move to a location
                        events.CharacterMove(currentTile);
                        events.Deselect();
                    }
                }
            }

        }else{
            GameEvents.current.CursorExit();
        }

        if (charaController.isMoving) {
            charaController.continuePath();
        }
    }

    public void clearSelected(){
        selectedTile = null;
    }
    

    public RaycastHit2D? GetFocusedOnTile(){

        Vector3 mousePos3d = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos = new Vector2(mousePos3d.x, mousePos3d.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero);

        if (hits.Length > 0){
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }

        return null;

    }
}
