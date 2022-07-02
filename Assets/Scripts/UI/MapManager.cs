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
    public Map map{ get => _map; }
    public OverlayTile overlayTilePrefab;
    public GameObject overlayContainer;
    public Dictionary<Vector2Int, OverlayTile> mapDict;


    private void Awake(){
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        } else{
            _instance = this;
            _gameZoneTilemap = GetComponentInChildren<Tilemap>();
            _tilesHolder = GetComponentInChildren<TilesHolder>();
            mapDict = new Dictionary<Vector2Int, OverlayTile>();
        }
    }

    public void init(Map gameMap){
        _map = gameMap;
        int width = _map.Width;
        int height = _map.Height;

        DrawInitialMap(width, height);
        DrawOverlay();
    }
    private void DrawInitialMap(int width, int height){
        var origin =  _gameZoneTilemap.origin;
        var cellSize =  _gameZoneTilemap.cellSize;
        _gameZoneTilemap.ClearAllTiles();
        _gameZoneTilemap.GetComponent<TilemapRenderer>().sortingOrder = (int) SortingOrders.Base;
        var currentCellPosition = origin;
        for (var h = height - 1; h >= 0; h--) {
            for (var w = width - 1; w >= 0; w--){
                _gameZoneTilemap.SetTile(currentCellPosition,
                _tilesHolder.GetTileByName(_map.nodes[h, w].type));
                currentCellPosition = new Vector3Int(
                    (int) (cellSize.x + currentCellPosition.x),
                    currentCellPosition.y, origin.z);
        var nodeMap = _map.nodes;
        for (var z = nodeMap.Count - 1; z >= 0; z--){
            var nodelist = nodeMap[z];
            foreach(Node node in nodelist) {
                var x = (int) (cellSize.x * node.X + (float) origin.x);
                var currentCellPosition = new Vector3Int(x, node.Y * (int) (Math.Ceiling(cellSize.y)) + origin.y, (int)(origin.z + z)); // cellsize.y = 0.5, so math.ceiling gives the coordinate the next whole # (ps. unity sucks)
                _gameZoneTilemap.SetTile(currentCellPosition, _tilesHolder.GetTileByName(node.Type));
            }
        }
        _gameZoneTilemap.CompressBounds();
    }

    private void DrawOverlay(){
        BoundsInt bounds = _gameZoneTilemap.cellBounds;
        var cellSize =  _gameZoneTilemap.cellSize;
        var overlays = _map.overlays;
        var origin =  _gameZoneTilemap.origin;

        foreach (var node in overlays) {
            var x = (int) (cellSize.x * node.X + (float) origin.x);
            var tileLocation = new Vector3Int(x, node.Y * (int) (Math.Ceiling(cellSize.y)) + origin.y, (int)(origin.z + node.Z));
            var cellWorldPosition = _gameZoneTilemap.GetCellCenterWorld(tileLocation);
            var overlayTile = Instantiate(overlayTilePrefab, overlayContainer.transform);
            overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y + cellSize.y / 2, cellWorldPosition.z + 1);
            overlayTile.GetComponent<SpriteRenderer>().sortingOrder = _gameZoneTilemap.GetComponent<TilemapRenderer>().sortingOrder;
        }
    }

}
