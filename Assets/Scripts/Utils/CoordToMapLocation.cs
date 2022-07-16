using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class CoordToMapLocation
{
    private static Vector3 cellSize;
    private static Vector3Int origin;

    public static void init(Tilemap tilemap){
        cellSize = tilemap.cellSize;
        origin = tilemap.origin;
        Debug.Log(cellSize);
        Debug.Log(origin);
    }

    public static int GetXOnMap(int x){
        return (int) (Math.Ceiling(cellSize.x) * x + origin.x);
    }

    public static int GetYOnMap(int y){
        return (int) (y *  (Math.Ceiling(cellSize.y)) + origin.y);
    }

    public static Vector2Int GetLocOnMap2d(Vector2Int pos){
        return new Vector2Int(GetXOnMap(pos.x), GetYOnMap(pos.y));
    }

    public static Vector3Int GetLocOnMap(Vector3Int pos){
        return new Vector3Int(GetXOnMap(pos.x), GetYOnMap(pos.y), pos.z);
    }


}
