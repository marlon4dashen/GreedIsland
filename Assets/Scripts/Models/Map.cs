using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Map
{

    public Tile[] map;
    public int size;

    public bool hasPathTo(Tile a, Tile b){

        //find path from a to b
        // return how many steps
        return true;
    }

    public Tile[] pathTo(Tile a, Tile b){
        // return tile array from a to b
        return new Tile[size];
    }

    public bool update(){
        //Update map
        return true;
    }

}
