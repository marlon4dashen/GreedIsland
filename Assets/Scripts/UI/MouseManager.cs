using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cursor;
    public float speed;
    public GameObject minionPrefab;
    private CharacterInfo minion;
    private PathFinder pathFinder;
    private List<OverlayTile> path;
    private void Start()
    {
        speed = 4; // use minion's speed
        pathFinder = new PathFinder();
        path = new List<OverlayTile>();

    }

    // Update is called once per frame
    void LateUpdate()
    {
        var focusedTileHit = GetFocusedOnTile();
        if (focusedTileHit.HasValue){
            OverlayTile overlayTile = focusedTileHit.Value.collider.gameObject.GetComponent<OverlayTile>();
            transform.position = overlayTile.transform.position;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = (int) SortingOrders.Cursor;

            if (Input.GetMouseButtonDown(0)){
                overlayTile.GetComponent<OverlayTile>().ShowTile();

                if (minion == null)
                {
                    minion = Instantiate(minionPrefab).GetComponent<CharacterInfo>();
                    PositionCharacterOnTile(overlayTile);
                    minion.standingOnTile = overlayTile;
                } else
                {
                    path = pathFinder.FindPath(minion.standingOnTile, overlayTile);
                    Debug.Log(path.Count);
                    overlayTile.gameObject.GetComponent<OverlayTile>().HideTile();
                }
            }

        }
        if(path.Count > 0){
            MoveAlongPath();
        }

    }

    private void MoveAlongPath(){
        var step = speed * Time.deltaTime;
        var zIndex = path[0].transform.position.z;
        minion.transform.position = Vector2.MoveTowards(minion.transform.position, path[0].transform.position, step);
        minion.transform.position = new Vector3(minion.transform.position.x, minion.transform.position.y, zIndex);

        if(Vector2.Distance(minion.transform.position, path[0].transform.position) < 0.00001f)
        {
            PositionCharacterOnTile(path[0]);
            path.RemoveAt(0);
        }
    }

    public RaycastHit2D? GetFocusedOnTile(){

        Vector3 mousePos3d = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos = new Vector2(mousePos3d.x, mousePos3d.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero);

        if (hits.Length > 0){
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }

        return null;

    }

    private void PositionCharacterOnTile(OverlayTile tile)
    {
        minion.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y+0.0001f, tile.transform.position.z);
        minion.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        minion.standingOnTile = tile;
    }
}
