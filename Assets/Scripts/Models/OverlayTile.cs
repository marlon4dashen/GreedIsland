using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayTile : MonoBehaviour
{

    public int G;
    public int H;
    public int F { get { return G + H; } }
    public bool isBlocked;
    public OverlayTile previous;
    public Vector3Int gridLocation;

    // Update is called once per frame
    void Update()
    {
    }

}
