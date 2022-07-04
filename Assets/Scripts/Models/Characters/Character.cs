using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Tilemaps;
using UnityEngine;

public abstract class Character
{
    private Vector3Int gridLocation;

    public Vector3Int Pos {
        get => gridLocation;
        set => gridLocation = value;
    }

    protected Character(int x, int y, Tilemap tilemap){
        this.gridLocation = new Vector3Int(GetXOnMap(x), GetYOnMap(y), (int) SortingOrders.Character);
    }

    private int GetXOnMap(int x){
        return (int) (this.cellSize.x * x + (float) this.origin.x);
    }

    private int GetYOnMap(int y){
        return y * (int) (Math.Ceiling(this.cellSize.y)) + this.origin.y;
    }

}
