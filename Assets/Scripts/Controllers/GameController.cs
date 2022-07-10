using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class GameController : MonoBehaviour
{

    //managers 
    private MapManager mapManager;
    private CameraManager camManager;

    //controllers
    private CharacterController charaController;
    private MouseController mouseController;

    //events
    private GameEvents gameEvents;

    //random game map
    private Map _map;

    //game state
    private GameState state;

    void Start() {
        state = GameState.START;
        StartCoroutine(SetUp());

    }
    
    IEnumerator SetUp() {
        mapManager = MapManager.Instance;
        camManager = CameraManager.Instance;
        charaController = CharacterController.Instance;
        mouseController = MouseController.Instance;
        gameEvents = GameEvents.current;

        gameEvents.OnStateChange += modifyState;

        charaController.init(gameEvents, mapManager);
        mouseController.init(charaController, gameEvents);
        ConfigHandler.init();
        CoordToMapLocation.init(mapManager.Tilemap);
        _map = new Map(15, 15);
        int width = _map.Width;
        camManager.ModifyCamera(width);
        mapManager.init(_map, gameEvents);
        var minionsData = ConfigHandler.readConfig();
        charaController.positionMinions(minionsData, mapManager.mapDict);

        //blue is us, red is enemy

        yield return new WaitForSeconds(2f);
        //TODO: update control panel board messages to start the game

        //set state to player turn first
        state = GameState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn() {
        if (state == GameState.PLAYERTURN){
            StartCoroutine(startPlayerTurn());
        } 
    }

    IEnumerator startPlayerTurn() {
        while (state == GameState.PLAYERTURN) {
            mouseController.onTurn(Team.Blue);

            yield return null;
        }

        if (state == GameState.BLUEWON) {
            Debug.Log("Blue won!");
        } else if (state == GameState.REDWON) {
            Debug.Log("Red won!");
        } else {
            StartCoroutine(startEnemyTurn());
        }
    }


    IEnumerator startEnemyTurn() {
        while (state == GameState.ENEMYTURN) {
            mouseController.onTurn(Team.Red);
            yield return null;
        }

        if (state == GameState.BLUEWON) {
            Debug.Log("Blue won!");
        } else if (state == GameState.REDWON) {
            Debug.Log("Red won!");
        } else {
            PlayerTurn();
        }
    }

    private void modifyState(GameState newState) {
        state = newState;
    }


    // void LateUpdate() {
    //     mouseController.startListen();
    // }
}