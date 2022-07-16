using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class CharacterController : MonoBehaviour
{

    public Farmer farmerPrefab;
    public Elf ElfPrefab;
    public GraveDigger graveDiggerPrefab;
    private Animator currAnimator;
    public bool isMoving;

    public GameObject minionContainer;
    private Dictionary<string, Character> minionStrToObj;
    private GameEvents events;
    private PathFinder pathFinder;

    //state variables
    public Character currentMinion;
    private List<OverlayTile> path;
    private List<OverlayTile> moveRange;
    private List<OverlayTile> attackRange;

    private List<Character> minionList;
    private Dictionary<OverlayTile, Character> minionLocations;

    private static CharacterController _instance;
    private Dictionary<Character, Animator> animatorList;



    public static CharacterController Instance {
        get { return _instance; }
    }

    private CharacterManager _charaManager;
    private MapManager _mapManager;

    private void Awake(){
        _instance = this;
        minionList = new List<Character>();
        minionLocations = new Dictionary<OverlayTile, Character>();
        minionStrToObj = new Dictionary<string, Character>();
        minionStrToObj.Add("Elf", ElfPrefab);
        minionStrToObj.Add("Farmer", farmerPrefab);
        minionStrToObj.Add("GraveDigger", graveDiggerPrefab);
        pathFinder = new PathFinder();
        path = new List<OverlayTile>();
        moveRange = new List<OverlayTile>();
        isMoving = false;
        animatorList = new Dictionary<Character, Animator>();
    }


    public void init(GameEvents currentEvents, MapManager mapManager){
        _charaManager = CharacterManager.Instance;
        _mapManager = mapManager;
        events = currentEvents;
        events.OnCharacterMove += moveMinion;
        events.OnSelectCharacter += selectMinion;
        events.OnCharacterAttack += minionAttack;
        events.OnSwitchTeam += clearAllStates;
    }


    public void positionMinions(List<Dictionary<string, string>> minionsInfo, Dictionary<Vector2Int, OverlayTile> mapDict) {

        foreach (Dictionary<string, string> minionInfo in minionsInfo) {
            string name = minionInfo["name"];
            int x = CoordToMapLocation.GetXOnMap(Int32.Parse(minionInfo["xPos"]));
            int y = CoordToMapLocation.GetYOnMap(Int32.Parse(minionInfo["yPos"]));
            var loc =  new Vector2Int(x, y);

            var minionPrefab = minionStrToObj[name];
            var init_tile = mapDict[loc];
            var minion = Instantiate(minionPrefab, minionContainer.transform);

            minion.currentTile = init_tile;
            // minionLocations.Add(init_tile, minion);
            updateMinionLocation(minion, init_tile);
            minionList.Add(minion);
            currAnimator = minion.GetComponent<Animator>();
            currAnimator.enabled = false;
            animatorList.Add(minion, currAnimator);
        }
        _charaManager.init(minionList);

    }

    public void selectMinion(Character selected, Mode mode){

        if (!minionList.Contains(selected)) {
            return;
        }
        currentMinion = selected;

        // if the selected minion has steps left in current round, paint range tiles
        switch(mode) {
            case Mode.Attack:
                displayAttackRange(selected);
                break;
            case Mode.Move:
                displayMoveRange(selected);
                break;
            default:
                break;
        }
    }

    private void displayMoveRange(Character currentMinion){
        if (currentMinion.moveLeft > 0) {
            moveRange = RangeFinder.GetTilesInMoveRange(currentMinion.currentTile, currentMinion.moveRange);
            foreach (OverlayTile tile in moveRange) {
                if (minionLocations.ContainsKey(tile)) {
                    if (!checkSameTeam(currentMinion, minionLocations[tile]))
                        _mapManager.PaintCharacterTile(tile);
                } else {
                    _mapManager.PaintRangeTile(tile);
                }
            }
        }
    }

    private void displayAttackRange(Character currentMinion) {
        if (currentMinion.attackLeft > 0){
            attackRange = RangeFinder.GetTilesInAttackRange(currentMinion.currentTile, currentMinion.atkRange);
            foreach (OverlayTile tile in attackRange) {
                _mapManager.PaintAttackRangeTile(tile);
            }
        }
    }

    public void moveMinion(OverlayTile destination){
        var start = currentMinion.currentTile;

        //check if destination in range
        if (moveRange.Count <= 0 || !moveRange.Contains(destination)) {
            Debug.Log("Can't reach there");
            return;
        }

        //Find path
        path = pathFinder.FindPath(start, destination, minionLocations);
        //update location array
        if (path.Count > 0){
            isMoving = true;
            currentMinion.moveLeft -= 1;
            animatorList[currentMinion].SetBool("isMoving", true);
            animatorList[currentMinion].enabled = true;
            // clear the range list if move succeed
            moveRange = new List<OverlayTile>();
            StartCoroutine(movement());
        } else {
            //trigger error event
            Debug.Log("Can't reach there");
        }


    }

    IEnumerator movement() {
        while (path.Count > 0) {
            if (_charaManager.MoveToTile(currentMinion, path[0])){
                var tile = path[0];
                path.RemoveAt(0); 
                updateMinionLocation(currentMinion, tile);
            }
            yield return null;
        }
        isMoving = false;
        animatorList[currentMinion].SetBool("isMoving", false);
        animatorList[currentMinion].enabled = false;
        clearAllStates();
    }

    public void updateMinionLocation(Character minion, OverlayTile des){
        if (minionLocations.ContainsKey(minion.currentTile)){
            minionLocations.Remove(minion.currentTile);
            minion.currentTile.isBlocked = false;
        }
        if (des != null) {
            minionLocations.Add(des, minion);
            minion.currentTile = des;
            minion.currentTile.isBlocked = true;
        }
    }

    public void minionAttack(Character atkee) {

        if (currentMinion.attackLeft > 0) {
            animatorList[currentMinion].ResetTrigger("Attack");
            animatorList[currentMinion].SetTrigger("Attack");
            currentMinion.transform.localScale = _charaManager.getMinionFacing(currentMinion.currentTile.gridLocation,atkee.currentTile.gridLocation);


            currentMinion.attack(atkee);
            if (atkee.isDead()) {
                // dead
                removeMinion(atkee);
            }
        }

        clearAllStates();
    }

    public void removeMinion(Character minion) {
        minionList.Remove(minion);
        updateMinionLocation(minion, null);
        minion.removeCharacter();
    }

    public void clearAllStates(Team team = Team.Blue) {
        moveRange = new List<OverlayTile>();
        attackRange = new List<OverlayTile>();
        currentMinion = null;
    }

    public bool checkInAttackRange(Character c1, Character c2) {
        return RangeFinder.checkInRange(c1.currentTile, c2.currentTile, c1.atkRange);
    }


    public bool checkSameTeam(Character minion1, Character minion2){
        return minion1.team == minion2.team;
    }

    public Character getCharacterFromTile(OverlayTile tile){
        return minionLocations.ContainsKey(tile) ? minionLocations[tile] : null;
    }

    public void refreshSteps(Team team) {
        foreach (var minion in minionList) {
            if (minion.team == team) {
                minion.refreshMoves();
            } 
        }
    }

    public Team? checkStatus(){

        bool hasBlue = false;
        bool hasRed = false;
        foreach (var minion in minionList) {
            if (minion.team == Team.Blue)
                hasBlue = true;
            else
                hasRed = true;
        }

        if (!hasBlue){
            //red won
            return Team.Red;
        } else if (!hasRed){
            return Team.Blue;
        } 

        return null;
    }

}
