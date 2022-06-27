using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilesHolder : MonoBehaviour
{
    private Tile _landTile;
    private Tile _waterTile;

    private void Awake() {
        _landTile = (Tile) Resources.Load("land", typeof(Tile));
        _waterTile = (Tile) Resources.Load("water", typeof(Tile));
    }

    public Tile GetLandTile(){
        return _landTile;
    }

    public Tile GetWaterTile(){
        return _waterTile;
    }

}
