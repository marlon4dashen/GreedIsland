using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{

    private static MapManager _instance;
    public static MapManager Instance {
        get { return _instance; }
    }

    private Tilemap _gameZoneTilemap;
    private TilesHolder _tilesHolder;
    private GameData _gameData;
    private Map _map;

    private void Awake(){
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        } else{
            _instance = this;
            _gameZoneTilemap = GetComponent<Tilemap>();
            _tilesHolder = GetComponent<TilesHolder>();
        }
    }

    public void init(Map gameMap){
        _map = gameMap;
        int width = _map.Width;
        int height = _map.Height;

        DrawInitialMap(width, height);
    }
    private void DrawInitialMap(int width, int height){
        var origin =  _gameZoneTilemap.origin;
        var cellSize =  _gameZoneTilemap.cellSize;
       _gameZoneTilemap.ClearAllTiles();
        Debug.Log(cellSize);
        var currentCellPosition = origin;
        for (var h = height - 1; h >= 0; h--) {
            for (var w = width - 1; w >= 0; w--){
                _gameZoneTilemap.SetTile(currentCellPosition,
                _tilesHolder.GetTileByName(_map.nodes[h, w].type));
                currentCellPosition = new Vector3Int(
                    (int) (cellSize.x + currentCellPosition.x),
                    currentCellPosition.y, origin.z);
            }
            Debug.Log(currentCellPosition);
            currentCellPosition = new Vector3Int(origin.x,(int)(Math.Ceiling(cellSize.y + currentCellPosition.y)), origin.z);
        }

        _gameZoneTilemap.CompressBounds();
    }

    private void DrawOverlay(){
        return;
    }

}
