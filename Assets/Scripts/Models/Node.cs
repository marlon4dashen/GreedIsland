using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Point point;
    public bool accessible;
    public string type;

    public Node(int x, int y, string type){
        this.point = new Point(x, y);
        this.type = type;
        this.accessible = setAccess();
    }

    private bool setAccess(){
        return this.type == "land";
    }

}
