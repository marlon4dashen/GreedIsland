using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{

    private MapManager mapManager;
    private CameraManager camManager;
    private Map _map;

    private void Awake(){
        mapManager = MapManager.Instance;
        camManager = CameraManager.Instance;
    }

    void Start() {
        _map = new Map(15, 15, mapManager.Tilemap);
        int width = _map.Width;
        camManager.ModifyCamera(width);
        mapManager.init(_map);
    }
}