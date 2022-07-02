using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class PathFinder
{
    public List<OverlayTile> FindPath(OverlayTile start, OverlayTile end)
    {
        List<OverlayTile> tilesToCheck = new List<OverlayTile>();
        List<OverlayTile> checkedTiles = new List<OverlayTile>();


        tilesToCheck.Add(start);
        while (tilesToCheck.Count > 0)
        {
            Debug.Log(tilesToCheck.Count);


            OverlayTile currentTile = tilesToCheck.OrderBy(x => x.F).First();
            tilesToCheck.Remove(currentTile);
            checkedTiles.Add(currentTile);

            if(currentTile == end)
            {
                // Debug.Log(start.gridLocation);
                return GetFinishedList(start, end);
            }

            var neighbourTiles = GetNeightbourTiles(currentTile);
            foreach (var neighbour in neighbourTiles)
            {
                if(neighbour.isBlocked || checkedTiles.Contains(neighbour) || Mathf.Abs(currentTile.gridLocation.z - neighbour.gridLocation.z) > 1)
                {
                    continue;
                }
                neighbour.G = GetManhattenDistance(start, neighbour);
                neighbour.H = GetManhattenDistance(end, neighbour);

                neighbour.previous = currentTile;

                if(!tilesToCheck.Contains(neighbour))
                {
                    tilesToCheck.Add(neighbour);
                }
            }
        }
        return new List<OverlayTile>();
    }


    private int GetManhattenDistance(OverlayTile start, OverlayTile neighbour)
    {
        return Mathf.Abs(start.gridLocation.x - neighbour.gridLocation.x) + Mathf.Abs(start.gridLocation.y - neighbour.gridLocation.y);
    }

    private List<OverlayTile> GetFinishedList(OverlayTile start, OverlayTile end)
    {
        List<OverlayTile> finishedList = new List<OverlayTile>();
        OverlayTile currentTile = end;

        while (currentTile != start)
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.previous;
        }

        finishedList.Reverse();
        // Debug.Log(finishedList);

        return finishedList;
    }

    private List<OverlayTile> GetNeightbourTiles(OverlayTile currentTile)
    {
        var map = MapManager.Instance.mapDict;

        List<OverlayTile> neighbours = new List<OverlayTile>();

        Vector2Int rightLocationToCheck = new Vector2Int(currentTile.gridLocation.x + 1,currentTile.gridLocation.y);
        Vector2Int leftLocationToCheck = new Vector2Int(currentTile.gridLocation.x - 1,currentTile.gridLocation.y);
        Vector2Int topLocationToCheck = new Vector2Int(currentTile.gridLocation.x,currentTile.gridLocation.y + 1);
        Vector2Int botLocationToCheck = new Vector2Int(currentTile.gridLocation.x,currentTile.gridLocation.y - 1);

        if(map.ContainsKey(rightLocationToCheck))
        {
            neighbours.Add(map[rightLocationToCheck]);
        }
        if(map.ContainsKey(leftLocationToCheck))
        {
            neighbours.Add(map[leftLocationToCheck]);
        }
        if(map.ContainsKey(topLocationToCheck))
        {
            neighbours.Add(map[topLocationToCheck]);
        }
        if(map.ContainsKey(botLocationToCheck))
        {
            neighbours.Add(map[botLocationToCheck]);
        }
        return neighbours;
    }
}
