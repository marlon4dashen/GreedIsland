using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorManager : MonoBehaviour
{

    void Start()
    {
        GameEvents.current.OnSelectCharacter += DrawSelector;
        GameEvents.current.OnDeselect+= HideSelector;
    }

    private void DrawSelector(OverlayTile tile){
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 1, (int) SortingOrders.Character);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = (int) SortingOrders.Character;
    }

    private void HideSelector(OverlayTile tile) {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 1, (int) SortingOrders.Character);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = (int) SortingOrders.Character;
    }
}
