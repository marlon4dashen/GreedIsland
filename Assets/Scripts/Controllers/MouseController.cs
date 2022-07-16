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
    private Character currMinion;
    private GameEvents events;

    private Team currentTeam;
    public Mode mode;

    private void Awake(){
        _instance = this;
    }


    public void init(CharacterController characterController, GameEvents currentEvent){
        charaController = characterController;
        events = currentEvent;
        events.OnSwitchTeam += resetStates;
        mode = Mode.Move;
    }

    private void resetStates(Team team){
        currMinion = null;
        currentTeam = team;
        charaController.refreshSteps(team);
    }

    public void onTurn(Team team){
        if (currentTeam == null || currentTeam != team){
            //just switched team
            events.SwitchTeam(team);
            Debug.Log("It's " + currentTeam + " turn");
        }
        // startListen();
        var currentTile = cursorListener();
        if (!charaController.isMoving) {
            select(currentTile);
            if (currMinion)
                performAction(currentTile);
            
        } 
    }

    public OverlayTile cursorListener(){
        var focusedTileHit = GetFocusedOnTile();
        if (focusedTileHit.HasValue){
            OverlayTile currentTile = focusedTileHit.Value.collider.gameObject.GetComponent<OverlayTile>();
            events.CursorEnter(currentTile.transform.position);
            return currentTile;
        }
        events.CursorExit();
        return null;
    }

    public void select(OverlayTile tile){

        if (Input.GetMouseButtonDown(0) && tile != null) {
            var minion = charaController.getCharacterFromTile(tile);

            if (minion) {
                if (minion.team == currentTeam) {
                    //whenever selected a new character, refresh move
                    currMinion = minion;
                    mode = Mode.Move;
                } else {
                    //display selected enemy info
                }
            } 
        } else {
            // display selected grid info
        }

        if (currMinion){
            events.Deselect();
            events.SelectCharacter(currMinion, mode);
        }
    }

    public void performAction(OverlayTile tile) {
        if (Input.GetMouseButtonDown(0) && tile != null) {
            switch(mode){
                case Mode.Move:
                    minionMove(tile);
                    break;
                case Mode.Attack:
                    minionAttack(tile);
                    break;
                default:
                    break;
            }
        }
    }

    private void minionMove(OverlayTile tile) {
        var minion = charaController.getCharacterFromTile(tile);
        if (minion == null) {
            //good to move, since no other character standing on it
            events.CharacterMove(tile);
            events.Deselect();
        }else {
            Debug.Log("someone is already there.");
        }
    }

    private void minionAttack(OverlayTile tile) {
        var target = charaController.getCharacterFromTile(tile);
        if (target != null){
            if (!charaController.checkSameTeam(target, currMinion)) {
                events.CharacterAttack(target);
                events.Deselect();
            } else {
                Debug.Log("That's your ally, dumb!");
            }
        } else {
            Debug.Log("No minion is there to attack.");
        }
        return;
    }

    public void checkBattleStatus() {
        var won = charaController.checkStatus();

        if (won.HasValue){
            switch (won)
            {
                case Team.Red:
                    events.StateChange(GameState.REDWON);
                    break;
                case Team.Blue:
                    events.StateChange(GameState.BLUEWON);
                    break;
                default:
                    Debug.Log("Something is broken");
                    break;
            }
        }
    }

    public void onAttackButton(){
        mode = Mode.Attack;
    }

    public void onMoveButton(){
        mode = Mode.Move;
    }

    public void onEndTurnButton() {
        if (!charaController.isMoving) {
            //end current player's turn, can't end while still there's minion moving.
            events.Deselect();
            events.StateChange(currentTeam == Team.Blue ? GameState.ENEMYTURN : GameState.PLAYERTURN);
        }
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
