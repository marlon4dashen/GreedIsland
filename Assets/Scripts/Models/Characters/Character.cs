using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Tilemaps;
using UnityEngine;

public abstract class Character : MonoBehaviour
{

    public OverlayTile currentTile;
    public Team team;
    public int atkDamage;
    public int atkRange;
    public int moveRange;

    //changable fields
    public int hp;
    public int mana;

    public int moveLeft;
    public int attackLeft;
    public int abilityLeft;


    // public abstract void ability();

    public void refreshMoves() {
        this.moveLeft = 1;
        this.attackLeft = 1;
        this.abilityLeft = 1;
    }

    public void attack(Character atkee) {
        atkee.hp -= this.atkDamage;
        this.attackLeft -= 1;
        Debug.Log(atkee.hp);
    }

    public bool isDead() {
        return this.hp <= 0;
    }

    public void removeCharacter() {
        Destroy(gameObject);
    }

}
