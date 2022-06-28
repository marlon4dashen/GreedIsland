using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilesHolder : MonoBehaviour
{
    public Tile GetTileByName(string name) {
        return (Tile) Resources.Load(name, typeof(Tile));
    }

}
