using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{

    private MapManager mapManager;
    private Map _map;

    private void Awake(){
        mapManager = MapManager.Instance;
    }

    void Start() {
        _map = new Map(10, 10, 5);
        Debug.Log(mapManager);
        mapManager.init(_map);
    }
}