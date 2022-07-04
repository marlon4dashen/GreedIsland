using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{

    private static CharacterManager _instance;
    public static CharacterManager Instance {
        get => _instance;
    }

    private void Awake(){
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        } else{
            _instance = this;
        }
    }

    public void init(Character[] characters){
        foreach (var minion in characters){
            minion.transform.position = new Vector3(minion.currentTile.transform.position.x, minion.currentTile.transform.position.y+0.0001f, (int) SortingOrders.Character);
            minion.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        }
    }

    public void moveAlongPath(Character c, List<OverlayTile> path){
        return;
    }
}
