using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveDigger : Character
{
    private void Awake() {
        atkDamage = 10;
        atkRange = 1;
        moveRange = 5;
        hp = 30;
        mana = 0;
    }
}