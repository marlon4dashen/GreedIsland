using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
    public int x;
    public int y;
    public bool accessible;
    public string type;

    public Point(int x, int y, string type){
        this.x = x;
        this.y = y;
        this.type = type;
        this.accessible = setAccess();
    }

    private bool setAccess(){
        return this.type == "land";
    }

}
