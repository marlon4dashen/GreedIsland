using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : Character
{
    private void Awake() {
        atkDamage = 10;
        atkRange = 1;
        moveRange = 3;
        hp = 50;
        mana = 0;
    }
}
