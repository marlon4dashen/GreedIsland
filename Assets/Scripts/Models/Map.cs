using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class Map
{
    private int width;
    private int height;
    private Node[,] tileArray;
    private HashSet<Point> water;

    public Map(int width, int height, int numWater) {

        this.width = width;
        this.height = height;
        tileArray = new Node[height, width];
        this.water = generateWater(numWater);
        // this.box = 
        for ( var i = 0; i < height; i ++) {
            for ( var j = 0; j < width; j ++){
                if (this.water.Contains(new Point(i, j))){
                    tileArray[i, j] = new Node(i, j, "water");
                }else {
                    tileArray[i, j] = new Node(i, j,  "land");
                }
            }
        }
    }

    private HashSet<Point> generateWater(int num) {
        var set = new HashSet<Point>();
        System.Random rnd = new System.Random();
        for (var i = 0; i < num; i++) {
            Point p; 
            do {
                int x = rnd.Next(2, this.height - 2);
                int y = rnd.Next(2, this.width - 2);
                p = new Point(x, y);
            }while(set.Contains(p));
            set.Add(p);
        }
        return set;
    }

    public Node[,] nodes {
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
