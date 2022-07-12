using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnDisplay : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI turnDisplay;
    void Start()
    {
        turnDisplay.text = "Starting";
        GameEvents.current.OnSwitchTeam += setText;
    }

    public void setText(Team team){
        turnDisplay.text = "On " + team + "'s turn";
    }
}
