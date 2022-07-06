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
    private List<OverlayTile> path;

    private static CharacterController _instance;
    private Dictionary<Character, Animator> animatorList;
    private Vector2 lastTile;



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
        minionStrToObj.Add("GraveDigger", graveDiggerPrefab);
        pathFinder = new PathFinder();
        path = new List<OverlayTile>();
        isMoving = false;
        animatorList = new Dictionary<Character, Animator>();
    }


    public void init(GameEvents currentEvents){
        _charaManager = CharacterManager.Instance;
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
            currAnimator = minion.GetComponent<Animator>();
            currAnimator.enabled = false;
            animatorList.Add(minion, currAnimator);
        }
        _charaManager.init(minionList);

    }

    public void selectMinion(OverlayTile selected){
        currentMinion = minionLocations[selected];
    }

    public void moveMinion(OverlayTile destination){
        var start = currentMinion.currentTile;
        if (path.Count == 0){
            path = pathFinder.FindPath(start, destination);
        }
        isMoving = true;
        animatorList[currentMinion].enabled = true;
        animatorList[currentMinion].SetBool("isMoving", true);
        minionLocations.Remove(currentMinion.currentTile);
        minionLocations.Add(destination, currentMinion);

    }

    public void continuePath() {

        //move minion along the path toward next available tile

        if (_charaManager.MoveToTile(currentMinion, path[0])){
            var tile = path[0];
            path.RemoveAt(0);
            currentMinion.transform.localScale = getMinionFacing(currentMinion.currentTile.gridLocation,tile.gridLocation);
            currentMinion.currentTile = tile;
        }
        if (path.Count == 0) {
            isMoving = false;

            animatorList[currentMinion].SetBool("isMoving", false);
            animatorList[currentMinion].enabled = false;
            currentMinion = null;
        }
    }

    private Vector3 getMinionFacing(Vector3Int start, Vector3Int next) {
        int deltaX = next.x - start.x;
        int deltaY = next.y - start.y;
        return new Vector3(deltaX != 0 ? -1 * Math.Sign(deltaX) : Math.Sign(deltaY), 1, 1);
    }
    public bool checkCharacterOnTile(OverlayTile tile) {
        return minionLocations.ContainsKey(tile);
    }

}
