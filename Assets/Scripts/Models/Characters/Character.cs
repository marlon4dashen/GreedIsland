using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Tilemaps;
using UnityEngine;

public abstract class Character : MonoBehaviour
{

    public OverlayTile currentTile;
    public int atkDamage;
    public int atkRange;
    public int moveRange;

    public int hp;
    public int mana;


}
