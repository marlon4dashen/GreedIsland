using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;


public class RangeFinder
{
    public static List<OverlayTile> GetTilesInMoveRange(OverlayTile tile, int range)
    {
        var startingTile = tile;
        var inRangeTiles = new List<OverlayTile>();
        int stepCount = 0;
        inRangeTiles.Add(startingTile);
        //Should contain the surroundingTiles of the previous step. 
        var tilesForPreviousStep = new List<OverlayTile>();
        tilesForPreviousStep.Add(startingTile);
        while (stepCount < range)
        {
            var surroundingTiles = new List<OverlayTile>();
            foreach (var item in tilesForPreviousStep)
            {
                var tiles = MapManager.Instance.GetSurroundingTiles(new Vector2Int(item.gridLocation.x, item.gridLocation.y), true); // with height bc can't jump two tiles
                foreach (OverlayTile curTile in tiles){
                    if (!curTile.isBlocked){
                        surroundingTiles.Add(curTile);
                    }
                }
            }
            inRangeTiles.AddRange(surroundingTiles);
            tilesForPreviousStep = surroundingTiles.Distinct().ToList();
            stepCount++;
        }
        return inRangeTiles.Distinct().ToList();
    }

    //get all tiles within attack range
    public static List<OverlayTile> GetTilesInAttackRange(OverlayTile tile, int range)
    {
        var startingTile = tile;
        var inRangeTiles = new List<OverlayTile>();
        var seen = new List<OverlayTile>();
        inRangeTiles.Add(startingTile);
        //Should contain the surroundingTiles of the previous step. 
        var tilesForPreviousStep = new List<OverlayTile>();
        tilesForPreviousStep.Add(startingTile);
        while (tilesForPreviousStep.Count > 0)
        {
            var surroundingTiles = new List<OverlayTile>();
            foreach (var item in tilesForPreviousStep)
            {
                var tiles = MapManager.Instance.GetSurroundingTiles(new Vector2Int(item.gridLocation.x, item.gridLocation.y), false); //without height bc ranged attack are projectile
                foreach (OverlayTile curTile in tiles){
                    if (checkInRange(startingTile, curTile, range) && !seen.Contains(curTile)){
                        surroundingTiles.Add(curTile);
                        seen.Add(curTile);
                    } 
                }
            }
            inRangeTiles.AddRange(surroundingTiles);
            tilesForPreviousStep = surroundingTiles.Distinct().ToList();
        }
        return inRangeTiles.Distinct().ToList(); 
    }

    public static bool checkInRange(OverlayTile t1, OverlayTile t2, int range) {

        var x_diff = t1.gridLocation.x - t2.gridLocation.x;
        var y_diff = t1.gridLocation.y - t2.gridLocation.y;        
        return (x_diff * x_diff + y_diff * y_diff) <= range * range;
    }

}