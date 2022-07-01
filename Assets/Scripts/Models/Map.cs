using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;


public class Map
{
    private int width;
    private int height;
    private Dictionary<int, List<Node>> tileArray; //int = z; 
    private HashSet<Node> overlayLocations;
    private System.Random rnd;

    public Map(int width, int height) {
        overlayLocations = new HashSet<Node>();
        rnd = new System.Random();
        this.width = width;
        this.height = height;
        tileArray = new Dictionary<int, List<Node>>();
        var water = generateWater();
        tileArray.Add(0, water.ToList());
        var baseArr = new List<Node>();
        for ( var i = 0; i < height; i ++) {
            for ( var j = 0; j < width; j ++){
                var newLand = new Node(i, j, 1, "land"); //Base land renders second
                if (!water.Contains(newLand)) {
                    baseArr.Add(newLand);
                    overlayLocations.Add(newLand);
                }
            }
        }
        tileArray.Add(1, baseArr);
        generateTerrain(water);
    }

    private void generateTerrain(HashSet<Node> water){

        int numOfHeads = rnd.Next(1, 10);

        var dirs = new Vector2Int[4] {
            new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, -1)
        };
        
        var heads = new List<Node>();
        var steps = new List<Node>();

        for (int i = 0; i < numOfHeads; i++){

            int headX;
            int headY;
            do {
                headX = rnd.Next(2, this.height);
                headY = rnd.Next(0, this.width);
            }while(water.Contains(new Node(headX, headY, 3, "land"))); //make sure no land is built on water

            int varSteps = rnd.Next(0, 5);

            for (int j = 0; j < varSteps; j++){
                if (dirs[j].x + headX >= 0 && dirs[j].x + headX < this.height && dirs[j].y + headY >= 0 && dirs[j].y + headY < this.width){
                    var newStep = new Node(dirs[j].x + headX, dirs[j].y + headY, 2, "land"); // 2 represents step layer
                    if (!water.Contains(newStep)){
                        steps.Add(newStep);
                        overlayLocations.Remove(newStep); //remove the same object with lower z
                        overlayLocations.Add(newStep); // add back the same node with higher z
                    }
                }
            }

            var newHead = new Node(headX, headY, 3, "land"); // 3 represents head layer
            steps.Add(new Node(headX, headY, 2, "land"));
            heads.Add(newHead);
            overlayLocations.Remove(newHead); //remove the same object with lower z
            overlayLocations.Add(newHead); // add back the same node with higher z

            // TODO: set access
        }

        tileArray.Add(2, steps);
        tileArray.Add(3, heads);
    }

    // private void generateBlocks(){

    // }

    private HashSet<Node> generateWater() {
        var set = new HashSet<Node>();
        

        //the pond is of size waterX * waterY

        int startX = rnd.Next((int) this.height / 3, (int) this.height * 2 / 3);
        int startY = rnd.Next((int) this.width / 3, (int) this.width * 2 / 3);

        int endX= rnd.Next((int) this.height / 3, (int) this.height * 2 / 3);
        int endY = rnd.Next((int) this.width / 3, (int) this.width * 2 / 3);


        if (startX > endX) {
            var tmp = startX;
            startX = endX;
            endX = tmp;
        }

        if (startY > endY) {
            var tmp1 = startY;
            startY = endY;
            endY = tmp1;
        }

        for (int x = startX; x <= endX; x++){
            for (int y = startY; y <= endY; y++){
                int z = 0; //water renders on lowest level;
                Node a = new Node(x, y, z, "water");
                a.Access = false;
                set.Add(a);
            }
        }

        return set;
    }

    public Dictionary<int, List<Node>> nodes {
        get => tileArray;
    }


    public List<Node> overlays {
        get => overlayLocations.ToList();
    }

    public int Width {
        get => width;
    }

    public int Height {
        get => height;
    }

}
