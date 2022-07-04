using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class GameController : MonoBehaviour
{

    private MapManager mapManager;
    private CameraManager camManager;

    private CharacterController charaController;
    private Map _map;

    private void Awake(){
        mapManager = MapManager.Instance;
        camManager = CameraManager.Instance;
        charaController = CharacterController.Instance;
        Debug.Log(charaController);
        ConfigHandler.init();
        CoordToMapLocation.init(mapManager.Tilemap);
    }

    void Start() {
        _map = new Map(15, 15, mapManager.Tilemap);

        int width = _map.Width;
        camManager.ModifyCamera(width);
        mapManager.init(_map);

        var minionsData = ConfigHandler.readConfig();
        Debug.Log(charaController);
        charaController.positionMinions(minionsData, mapManager.mapDict);
        //a list of prefab Instantiate()
        // var minion = new Character(0, 0, mapManager.Tilemap);
        // charaManager.init(character, mapManager.mapDict);
    }
    
    void LateUpdate() {
        MouseController.startListen();
    }
}