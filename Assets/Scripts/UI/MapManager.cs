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
    public Tilemap Tilemap {
        get => _gameZoneTilemap;
    }
    private TilesHolder _tilesHolder;
    private Map _map;
    private GameEvents events;
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

    public void init(Map gameMap, GameEvents currentEvents){
        _map = gameMap;

        //add events
        events = currentEvents;
        events.OnDeselect += DeselectTiles;
        int width = _map.Width;
        int height = _map.Height;

        DrawInitialMap(width, height);
        DrawOverlay();
    }
    private void DrawInitialMap(int width, int height){
        var origin =  _gameZoneTilemap.origin;
        var cellSize =  _gameZoneTilemap.cellSize;
        Debug.Log(_gameZoneTilemap);
        _gameZoneTilemap.ClearAllTiles();
        _gameZoneTilemap.GetComponent<TilemapRenderer>().sortingOrder = (int) SortingOrders.Base;
        var nodeMap = _map.nodes;
        for (var z = nodeMap.Count - 2 ; z >= -1; z--){
            var nodelist = nodeMap[z];
            foreach(Node node in nodelist) {
                if(z==1){
                    Debug.Log(CoordToMapLocation.GetLocOnMap(node.GetPos()));
                    // Debug.Log(node.GetPos());

                }
                _gameZoneTilemap.SetTile(CoordToMapLocation.GetLocOnMap(node.GetPos()), _tilesHolder.GetTileByName(node.Type));

            }
        }
        // _gameZoneTilemap.CompressBounds();
    }

    private void DrawOverlay(){
        BoundsInt bounds = _gameZoneTilemap.cellBounds;
        var cellSize =  _gameZoneTilemap.cellSize;
        var overlays = _map.overlays;
        var origin =  _gameZoneTilemap.origin;
        Debug.Log(_gameZoneTilemap);



        foreach (var node in overlays) {
            var tileLocation = CoordToMapLocation.GetLocOnMap(node.GetPos());
            var cellWorldPosition = _gameZoneTilemap.GetCellCenterWorld(tileLocation);
            var overlayTile = Instantiate(overlayTilePrefab, overlayContainer.transform);
            overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y + cellSize.y/2, cellWorldPosition.z + 1);
            overlayTile.GetComponent<SpriteRenderer>().sortingOrder = _gameZoneTilemap.GetComponent<TilemapRenderer>().sortingOrder;
            var tilePos2d = CoordToMapLocation.GetLocOnMap2d(node.GetPos2d());
            overlayTile.gridLocation = tileLocation;
            mapDict.Add(tilePos2d, overlayTile);
        }
    }

    public List<OverlayTile> GetSurroundingTiles(Vector2Int originTile, bool withHeight)
    {
        var surroundingTiles = new List<OverlayTile>();
        int zValue = withHeight ? 1 : 10;
        Vector2Int TileToCheck = new Vector2Int(originTile.x + 1, originTile.y);
        if (mapDict.ContainsKey(TileToCheck))
        {
            if (Mathf.Abs(mapDict[TileToCheck].transform.position.z - mapDict[originTile].transform.position.z) <= zValue)
                surroundingTiles.Add(mapDict[TileToCheck]);
        }
        TileToCheck = new Vector2Int(originTile.x - 1, originTile.y);
        if (mapDict.ContainsKey(TileToCheck))
        {
            if (Mathf.Abs(mapDict[TileToCheck].transform.position.z - mapDict[originTile].transform.position.z) <= zValue)
                surroundingTiles.Add(mapDict[TileToCheck]);
        }
        TileToCheck = new Vector2Int(originTile.x, originTile.y + 1);
        if (mapDict.ContainsKey(TileToCheck))
        {
            if (Mathf.Abs(mapDict[TileToCheck].transform.position.z - mapDict[originTile].transform.position.z) <= zValue)
                surroundingTiles.Add(mapDict[TileToCheck]);
        }
        TileToCheck = new Vector2Int(originTile.x, originTile.y - 1);
        if (mapDict.ContainsKey(TileToCheck))
        {
            if (Mathf.Abs(mapDict[TileToCheck].transform.position.z - mapDict[originTile].transform.position.z) <= zValue)
                surroundingTiles.Add(mapDict[TileToCheck]);
        }
        return surroundingTiles;
    }

    public void PaintRangeTile(OverlayTile tile){
        tile.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.45f, 0.31f, 0.8f);
    }

    public void PaintCharacterTile(OverlayTile tile) {
        tile.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0, 0, 0.8f);
    }

    public void PaintAttackRangeTile(OverlayTile tile){
        tile.GetComponent<SpriteRenderer>().color = new Color(0, 0.2f, 0.8f, 0.8f);
    }

    public void DeselectTiles() {
        foreach (KeyValuePair<Vector2Int, OverlayTile> tile in mapDict){
            tile.Value.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }
    }

}
