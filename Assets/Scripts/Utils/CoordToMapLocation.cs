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
    }

    public static int GetXOnMap(int x){
        return (int) (cellSize.x * x + (float) origin.x);
    }

    public static int GetYOnMap(int y){
        return y * (int) (Math.Ceiling(cellSize.y)) + origin.y;
    }


}
