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
    private MouseController mouseController;
    private GameEvents gameEvents;
    private Map _map;

    void Start() {
        mapManager = MapManager.Instance;
        camManager = CameraManager.Instance;
        charaController = CharacterController.Instance;
        mouseController = MouseController.Instance;
        gameEvents = GameEvents.current;
        charaController.init(gameEvents, mapManager);
        mouseController.init(charaController, gameEvents);
        ConfigHandler.init();
        CoordToMapLocation.init(mapManager.Tilemap);
        _map = new Map(15, 15);
        int width = _map.Width;
        camManager.ModifyCamera(width);
        mapManager.init(_map, gameEvents);
        var minionsData = ConfigHandler.readConfig();
        charaController.positionMinions(minionsData, mapManager.mapDict);
    }
    
    void LateUpdate() {
        mouseController.startListen();
    }
}