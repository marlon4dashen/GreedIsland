using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Map
{
    private int width;
    private int height;
    private Point[,] tileArray;

    public Map(int width, int height ) {

        this.width = width;
        this.height = height;
        tileArray = new Point[height, width];
        // this.water = generateWater
        // this.box = 
        for ( var i = 0; i < height; i ++) {
            for ( var j = 0; j < width; j ++){
                tileArray[i, j] = new Point(i, j,  "land");
            }
        }
    }

    // private List<Point> generateWater

    public Point[,] points {
        get => tileArray;
    }

    public int Width {
        get => width;
    }

    public int Height {
        get => height;
    }

    // public bool hasPathTo(Tile a, Tile b){

    //     //find path from a to b
    //     // return how many steps
    //     return true;
    // }

    // public Tile[] pathTo(Tile a, Tile b){
    //     // return tile array from a to b
    //     return new Tile[size];
    // }

    public bool update(){
        //Update map
        return true;
    }

}
