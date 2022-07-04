using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{

    private CharacterManager _instance;
    public CharacterManager instance {
        get => _instance;
    }
    private Tilemap _tilemap;
    private 

    public Character[] characterList;

    private void Awake(){
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        } else{
            _instance = this;
        }
    }

    void Start() {
        
    }

    void init(Character c, Dictionary<Vector2Int, OverlayTile> tilemap){
        _tilemap = tilemap;
        var charPos = c.Pos;
        var charPos2d = new Vector2Int(charPos.x, charPos.y);

        OverlayTile tile_pos = tilemap[charPos2d].transform.position;
        var charPosOnMap = new Vector3(tile_pos.x, tile_pos.y+0.0001f,charPos.z);

    }
}
