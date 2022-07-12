using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


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
        var bottomArr = new List<Node>();
        for ( var i = -2; i <= height + 1; i ++) {
            for ( var j = -2; j <= width + 1; j ++){

                var newBottom = new Node(i, j, -1, "under"); //Base lnd renders
                if ( i == -2 && ( j > width/5 && j <= 3*width/5))
                {
                    bottomArr.Add(new Node(i-1, j, -1, "under"));
                }else if(j == -2 && ( i > 2*height/5 && i <= 4*height/5))
                {
                    bottomArr.Add(new Node(i, j-1, -1, "under"));
                }else if((i == -2 || i == height+1) && (j == -2 || j == width+1))
                {
                    bottomArr.Add(new Node(i-2, j-2, -1, "under"));
                    continue;

                }
                bottomArr.Add(newBottom);
            }
        }
        tileArray.Add(-1, bottomArr);
        for ( var i = 0; i < height; i ++) {
            for ( var j = 0; j < width; j ++){
                var newLand = new Node(i, j, 1, "Land"); //Base land renders secondsecond
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

        var heads = new HashSet<Node>();
        var steps = new HashSet<Node>();

        for (int i = 0; i < numOfHeads; i++){

            int headX;
            int headY;
            do {
                var leftOrRight = rnd.Next(0, 2);
                headX = leftOrRight == 0 ? rnd.Next(0, this.height) : this.height - 1;
                headY = leftOrRight == 0 ? this.width - 1 : rnd.Next(0, this.width);
            }while(heads.Contains(new Node(headX, headY, 3, "land"))); //make sure no head is already taken

            for (int w = 1; w < (int) (this.width / 4); w++){
                for (int j = 0; j < 4; j++){
                    int exist = rnd.Next(0, 2);
                    int deltaX = dirs[j].x * w;
                    int deltaY = dirs[j].y * w;
                    if (deltaX + headX >= 0 && deltaX + headX < this.height && deltaY + headY >= 0 && deltaY + headY < this.width && exist == 1){
                        var newStep = new Node(deltaX + headX, deltaY + headY, 2, "land"); // 2 represents step layer
                        if (!steps.Contains(newStep) && !heads.Contains(newStep) && !water.Contains(newStep)){
                            steps.Add(newStep);
                            overlayLocations.Remove(newStep); //remove the same object with lower z
                            overlayLocations.Add(newStep); // add back the same node with higher z
                        }
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

        tileArray.Add(2, steps.ToList());
        tileArray.Add(3, heads.ToList());
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
