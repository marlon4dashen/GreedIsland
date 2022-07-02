using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MouseController : MonoBehaviour
{

    // Update is called once per frame
    public static void startListen()
    {
        var focusedTileHit = GetFocusedOnTile();
        if (focusedTileHit.HasValue){
            OverlayTile overlayTile = focusedTileHit.Value.collider.gameObject.GetComponent<OverlayTile>();
            GameEvents.current.CursorEnter(overlayTile.transform.position);
            // if (Input.GetMouseButtonDown(0)){
            //     overlayTile.GetComponent<OverlayTile>().ShowTile();

            //     if (minion == null)
            //     {
            //         minion = Instantiate(minionPrefab).GetComponent<CharacterInfo>();
            //         PositionCharacterOnTile(overlayTile);
            //         minion.standingOnTile = overlayTile;
            //     } else
            //     {
            //         path = pathFinder.FindPath(minion.standingOnTile, overlayTile);
            //         Debug.Log(minion.standingOnTile.gridLocation);
            //         Debug.Log(overlayTile.gridLocation);
            //         overlayTile.gameObject.GetComponent<OverlayTile>().HideTile();
            //     }
            // }

        }else{
            GameEvents.current.CursorExit();
        }
    }

    public static RaycastHit2D? GetFocusedOnTile(){

        Vector3 mousePos3d = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos = new Vector2(mousePos3d.x, mousePos3d.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero);

        if (hits.Length > 0){
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }

        return null;

    }
}
