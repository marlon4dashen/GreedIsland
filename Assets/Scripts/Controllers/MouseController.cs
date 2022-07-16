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
    private OverlayTile selectedTile;
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
        events.OnDeselect += clearSelected;
        events.OnSwitchTeam += refreshMove;
        mode = Mode.Move;
    }


    public void onTurn(Team team){
        if (currentTeam == null || currentTeam != team){
            //just switched team
            currentTeam = team;
            events.SwitchTeam(team);
            Debug.Log("It's " + currentTeam + " turn");
        }
        startListen();
        // var currentTile = cursorListener();
        // if (currentTile && !charaController.isMoving) {
        //    if (!currMinion) {
        //        select(currentTile);
        //    } else {
               
        //    }
        // } 
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

        if (Input.GetMouseButtonDown(0)) {
            var minion = charaController.getCharacterFromTile(tile);

            if (minion) {
                if (minion.team == currentTeam) {
                    //selected
                    currMinion = minion;
                    events.SelectCharacter(selectedTile);
                } else {
                    //display selected enemy info
                }
            } 
        } else {
            // display selected grid info
        }
    }

    public void startListen()
    {
        var focusedTileHit = GetFocusedOnTile();
        if (focusedTileHit.HasValue){
            OverlayTile currentTile = focusedTileHit.Value.collider.gameObject.GetComponent<OverlayTile>();
            events.CursorEnter(currentTile.transform.position);
            if (Input.GetMouseButtonDown(0) && !charaController.isMoving){
                if (selectedTile == null) {
                    var minion = charaController.getCharacterFromTile(currentTile);
                    if (minion != null && minion.team == currentTeam){
                        selectedTile = currentTile;
                        events.SelectCharacter(selectedTile);
                    }
                } else {
                    // a minion is pre-selected
                    var prevCharacter = charaController.getCharacterFromTile(selectedTile);

                    var currCharacter = charaController.getCharacterFromTile(currentTile);
                    if (currCharacter != null) {
                        
                        // for now only select another minion
                        if (charaController.checkSameTeam(prevCharacter, currCharacter)) {
                            // on the same team
                            events.Deselect();
                            selectedTile = currentTile;
                            events.SelectCharacter(selectedTile);
                        } else {
                            // on different team
                            // attack
                            // check if atkee in atker atk range
                            if (charaController.checkInAttackRange(prevCharacter, currCharacter)) {
                                //if in, attack
                                events.CharacterAttack(prevCharacter, currCharacter);
                                events.Deselect();
                            // } else if (moveRange.Contains(atkee)) {
                            //     // not in attack range but can move to nearby then attack
                            } else {
                                Debug.Log("Not in attack range");
                            }
                            // TODO: ability
                        }
                    } else {
                        // move to a location
                        events.CharacterMove(currentTile);
                        events.Deselect();
                    }
                }
            }
            //test switch round
            if (Input.GetMouseButtonDown(1)) {
                events.Deselect();
                events.StateChange(currentTeam == Team.Blue ? GameState.ENEMYTURN : GameState.PLAYERTURN);
            }

        }else{
            events.CursorExit();
        }
    }

    public void refreshMove(Team team) {
        charaController.refreshSteps(team);
    }

    public void clearSelected(){
        selectedTile = null;
        mode = Mode.Move;
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
        Debug.Log("In attack");
        mode = Mode.Attack;
    }

    public void onMoveButton(){
        Debug.Log("In move");
        mode = Mode.Move;
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
