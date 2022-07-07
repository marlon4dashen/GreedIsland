using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterManager : MonoBehaviour
{

    private static CharacterManager _instance;
    public static CharacterManager Instance {
        get => _instance;
    }

    private int speed;

    private void Awake(){
        _instance = this;
        speed = 1;
    }

    public void init(List<Character> characters){
        foreach (var minion in characters){
            minion.transform.position = new Vector3(minion.currentTile.transform.position.x, minion.currentTile.transform.position.y+0.0001f, (int) SortingOrders.Character);
            minion.GetComponent<SpriteRenderer>().sortingOrder = minion.currentTile.GetComponent<SpriteRenderer>().sortingOrder;
        }
    }

    public bool MoveToTile(Character minion, OverlayTile tile){
        var step = speed * Time.deltaTime;
        minion.transform.localScale = getMinionFacing(minion.currentTile.gridLocation, tile.gridLocation);
        minion.transform.position = Vector2.MoveTowards(minion.transform.position, tile.transform.position, step);
        minion.transform.position = new Vector3(minion.transform.position.x, minion.transform.position.y, (int) SortingOrders.Character);

        if(Vector2.Distance(minion.transform.position, tile.transform.position) < 0.00001f)
        {
            PositionMinionOnTile(minion, tile);
            return true;
        }
        return false;
    }

    private void PositionMinionOnTile(Character minion, OverlayTile tile){
        minion.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y+0.0001f, (int) SortingOrders.Character);
        minion.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
    }

    private Vector3 getMinionFacing(Vector3Int start, Vector3Int next) {
        int deltaX = next.x - start.x;
        int deltaY = next.y - start.y;
        return new Vector3(deltaX != 0 ? -1 * Math.Sign(deltaX) : Math.Sign(deltaY), 1, 1);
    }
}
