using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class CharacterController : MonoBehaviour
{

    public Farmer farmerPrefab;
    public Elf ElfPrefab;
    public GameObject minionContainer;
    private List<Character> minionList;
    private Dictionary<string, Character> minionStrToObj;

    private static CharacterController _instance;
    public static CharacterController Instance {
        get { return _instance; }
    }

    private CharacterManager _charaManager;

    private void Awake(){
        _instance = this;
        _charaManager = CharacterManager.Instance;
        minionList = new List<Character>();
        minionStrToObj = new Dictionary<string, Character>();
        minionStrToObj.Add("Elf", ElfPrefab);
        minionStrToObj.Add("Farmer", farmerPrefab);
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
            minionList.Add(minion);
        }
        _charaManager.init(minionList);

    }

}
