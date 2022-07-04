using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Vector3Int gridLocation;

    public Vector3Int Pos {
        get => gridLocation;
        set => gridLocation = value;
    }

    protected Character(int x, int y, Tilemap tilemap){
    }

}
