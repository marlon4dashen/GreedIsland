using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : Point
{
    private bool accessible;
    private string type;

    public string Type {
        get => type;
    }

    public bool Access {
        get => accessible;
        set => this.accessible = value;
        
    }

    public Node(int x, int y, int z, string type) : base(x, y, z)
    {
        this.type = type;
        this.accessible = true;
    }

}
