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
    public Character currentMinion;
    private Animator currAnimator;
    public bool isMoving;

    public GameObject minionContainer;
    private List<Character> minionList;
    private Dictionary<OverlayTile, Character> minionLocations;
    private Dictionary<string, Character> minionStrToObj;
    private GameEvents events;
    private PathFinder pathFinder;

    //state variables
    private List<OverlayTile> path;
    private List<OverlayTile> moveRange;

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
            minionLocations.Add(init_tile, minion);
            minion.currentTile = init_tile;
            minionList.Add(minion);
            currAnimator = minion.GetComponent<Animator>();
            currAnimator.enabled = false;
            animatorList.Add(minion, currAnimator);
        }
        _charaManager.init(minionList);

    }

    public void selectMinion(OverlayTile selected){

        if (!minionLocations.ContainsKey(selected)) {
            return;
        }
        currentMinion = minionLocations[selected];

        // if the selected minion has steps left in current round, paint range tiles
        Debug.Log(currentMinion.moveLeft);
        if (currentMinion.moveLeft > 0) {
            Debug.Log(currentMinion.moveLeft);
            Debug.Log(moveRange);
            moveRange = RangeFinder.GetTilesInMoveRange(selected, currentMinion.moveRange);

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

    public void moveMinion(OverlayTile destination){
        var start = currentMinion.currentTile;
        
        //check if destination in range
        if (moveRange.Count <= 0 || !moveRange.Contains(destination)) {
            Debug.Log("Can't reach there");
            return;
        }

        //Find path
        path = pathFinder.FindPath(start, destination);
        //update location array
        if (path.Count > 0){
            isMoving = true;

            currentMinion.moveLeft -= 1;
            minionLocations.Remove(currentMinion.currentTile);
            minionLocations.Add(destination, currentMinion);
            animatorList[currentMinion].SetBool("isMoving", true);
            animatorList[currentMinion].enabled = true;
            // clear the range list if move succeed
            moveRange = new List<OverlayTile>();
        } else {
            //trigger error event
            Debug.Log("Can't reach there");
        }


    }

    public void continuePath() {

        //move minion along the path toward next available tile
        if (_charaManager.MoveToTile(currentMinion, path[0])){
            var tile = path[0];
            path.RemoveAt(0); 
            currentMinion.currentTile = tile;
        }
        if (path.Count == 0) {
            isMoving = false;
            animatorList[currentMinion].SetBool("isMoving", false);
            animatorList[currentMinion].enabled = false;
            currentMinion = null;
        }
    }

    public void minionAttack(Character atker, Character atkee) {

    }

    public void clearAllStates() {
        moveRange = new List<OverlayTile>();

    }

    public bool checkInAttackRange(Character c1, Character c2) {

        var x_diff = c1.currentTile.gridLocation.x - c2.currentTile.gridLocation.x;
        var y_diff = c1.currentTile.gridLocation.y - c2.currentTile.gridLocation.y;

        return Math.Sqrt(x_diff * x_diff + y_diff * y_diff) <= (double) c1.atkRange;
    }


    public bool checkSameTeam(Character minion1, Character minion2){
        return minion1.team == minion2.team;
    }

    public Character getCharacterFromTile(OverlayTile tile){
        return minionLocations.ContainsKey(tile) ? minionLocations[tile] : null;
    }

}
