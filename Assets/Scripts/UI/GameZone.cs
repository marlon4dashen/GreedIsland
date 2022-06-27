using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameZone : MonoBehaviour
{

    private Tilemap _gameZoneTilemap;
    private TilesHolder _tilesHolder;
    private GameData _gameData;
    private Map _map;

    private void Awake(){
        _gameZoneTilemap = GetComponent<Tilemap>();
        _tilesHolder = GetComponent<TilesHolder>();
        _gameData = FindObjectOfType<GameData>();
        _map = new Map(15, 5);
    }

    void Start(){
        var width = _map.Width;
        var height = _map.Height;
        var origin = _gameZoneTilemap.origin;
        var cellSize = _gameZoneTilemap.cellSize;
        _gameZoneTilemap.ClearAllTiles();
        var currentCellPosition = origin;

        for (var h = 0; h < height; h++) {
            for (var w = 0; w < width; w++){
                _gameZoneTilemap.SetTile(currentCellPosition,
                _tilesHolder.GetLandTile());
                Debug.Log(h + " " + w);
                currentCellPosition = new Vector3Int(
                    (int) (cellSize.x + currentCellPosition.x),
                    currentCellPosition.y, origin.z);
            }
            currentCellPosition = new Vector3Int(origin.x,(int) (cellSize.y + currentCellPosition.y), origin.z);
        }

        _gameZoneTilemap.CompressBounds();
    
    }

    void Update()
    {
        
    }
}
