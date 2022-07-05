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
    public bool isMoving;

    public GameObject minionContainer;
    private List<Character> minionList;
    private Dictionary<OverlayTile, Character> minionLocations;
    private Dictionary<string, Character> minionStrToObj;
    private GameEvents events;
    private PathFinder pathFinder;
    private List<OverlayTile> path;
    private List<OverlayTile> moveRange;

    private static CharacterController _instance;

    private Rigidbody2D rb;


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
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();

    }

    public void init(GameEvents currentEvents, MapManager mapManager){
        _charaManager = CharacterManager.Instance;
        _mapManager = mapManager;
        events = currentEvents;
        events.OnCharacterMove += moveMinion;
        events.OnSelectCharacter += selectMinion;
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
        }
        _charaManager.init(minionList);

    }

    public void selectMinion(OverlayTile selected){
        currentMinion = minionLocations[selected];

        // if the selected minion has steps left in current round, paint range tiles
        Debug.Log(currentMinion.moveLeft);
        if (currentMinion.moveLeft > 0) {
            moveRange = RangeFinder.GetTilesInRange(selected, currentMinion.moveRange);
            _mapManager.PaintRangeTile(moveRange);
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
            currentMinion = null;
        }
    }


    public Character getCharacterFromTile(OverlayTile tile){
        return minionLocations[tile];
    }

    public bool checkCharacterOnTile(OverlayTile tile) {
        return minionLocations.ContainsKey(tile);
    }

}
