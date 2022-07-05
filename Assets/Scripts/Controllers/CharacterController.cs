using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class CharacterController : MonoBehaviour
{

    public Farmer farmerPrefab;
    public Elf ElfPrefab;
    public Character currentMinion;
    public bool isMoving;

    public GameObject minionContainer;
    private List<Character> minionList;
    private Dictionary<OverlayTile, Character> minionLocations;
    private Dictionary<string, Character> minionStrToObj;
    private GameEvents events;
    private PathFinder pathFinder;
    private List<OverlayTile> path;

    private static CharacterController _instance;
    public static CharacterController Instance {
        get { return _instance; }
    }

    private CharacterManager _charaManager;

    private void Awake(){
        _instance = this;
        minionList = new List<Character>();
        minionLocations = new Dictionary<OverlayTile, Character>();
        minionStrToObj = new Dictionary<string, Character>();
        minionStrToObj.Add("Elf", ElfPrefab);
        minionStrToObj.Add("Farmer", farmerPrefab);
        pathFinder = new PathFinder();
        path = new List<OverlayTile>();
        isMoving = false;
    }

    public void init(GameEvents currentEvents){
        _charaManager = CharacterManager.Instance;
        events = currentEvents;
        events.OnCharacterMove += moveMinion;
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

    public void selectMinion(){
        
    }

    public void moveMinion(OverlayTile start, OverlayTile destination){
        if (path.Count == 0){
            path = pathFinder.FindPath(start, destination);
        }
        currentMinion = minionLocations[start];
        isMoving = true;
        minionLocations.Remove(start);
        minionLocations.Add(destination, currentMinion);
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


    public bool checkCharacterOnTile(OverlayTile tile) {
        return minionLocations.ContainsKey(tile);
    }

}
