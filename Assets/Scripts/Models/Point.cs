using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
    private int x;
    private int y;
    private int z;

    public Point(int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public int X{
        get => x;
        set => this.x = value;
    }

    public int Y{
        get => y;
        set => this.y = value;
    }

    public int Z{
        get => z;
        set => this.z = value;
    }

    public override bool Equals(object obj) {
        if (obj == null || GetType() != obj.GetType()) return false;
        Point p = obj as Point;
        return this.x == p.X && this.y == p.Y;
    }

    public override int GetHashCode(){
        return (this.x * 10 + this.y).GetHashCode();
    }
}
