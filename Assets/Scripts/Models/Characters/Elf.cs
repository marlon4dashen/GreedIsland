using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elf : Character
{
    private void Awake() {
        atkDamage = 10;
        atkRange = 4;
        moveRange = 10;
        hp = 30;
        mana = 0;
    }
}
