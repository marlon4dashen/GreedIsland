using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private const float CameraPositionModifier = 0.5f;
    private const float CameraSizeModifier = 1.2f;
    private Camera _camera;

    private static CameraManager _instance;
    public static CameraManager Instance {
        get { return _instance; }
    }

    private void Awake() {
        if (_instance != null && _instance != this){
            Destroy(this.gameObject);
        }else{
            _instance = this;
            _camera = Camera.main;
        }

    }

    public void ModifyCamera(int width){
        //standardize to a 15 by 15 tilemap
        var modifier = (width / 3) * CameraPositionModifier;
        _camera.transform.position = new Vector3(
            _camera.transform.position.x + modifier,
            _camera.transform.position.y - modifier,
            _camera.transform.position.z
        );
        _camera.orthographicSize = Mathf.Pow(CameraSizeModifier, (width / 2.0f)) * _camera.orthographicSize;
    }
}
