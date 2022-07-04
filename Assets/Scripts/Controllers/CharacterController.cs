using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class CharacterController : MonoBehaviour
{

    public Farmer farmerPrefab;
    public Elf ElfPrefab;
    public GameObject minionContainer;
    private Character[] minionList;

    public static void positionMinions(Dictionary<Vector2Int, OverlayTile> mapDict) {
        
        var minionStrToObj = new Dictionary<string, Character>() {
            {"farmer", farmerPrefab},
            {"elf", ElfPrefab}
        };


    }

}
