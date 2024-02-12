using System.Collections.Generic;
using UnityEngine;

//Manages a dynamic camera system that adjusts its position and zoom based on the positions of multiple players
public class CameraController : MonoBehaviour
{
    [SerializeField] private List<Transform> _playerList = new();
    [SerializeField] private float _minZoom = 10f; //Closest Camera zoom
    [SerializeField] private float _maxZoom = 30f; //Farthest camera zoom
    [SerializeField] private float _zoomLimiter = 50f; //Controls the senstivity of the zoom changes
    [SerializeField] private float _smoothTime = 0.5f; //Smoothness of camera movement

    private Vector3 _cameraMoveVelocity;
    private Camera _camera;

    private void Awake()
    {
        //Get the canera component
        _camera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        //Check if there are no players, if so, do not adjust camera
        if (_playerList.Count == 0)
            return;

        MoveCamera();
        ZoomCamera();
    }

    //Moves the camera to the center point between all players
    private void MoveCamera()
    {
        Vector3 _centrePoint = GetCentrePointOfPlayers();

        //Maintain the camera's y position constant
        Vector3 _newPosition = new(_centrePoint.x, 30f, _centrePoint.z);

        transform.position = Vector3.SmoothDamp(transform.position, _newPosition, ref _cameraMoveVelocity, _smoothTime);
    }

    //Adjust the camera's zoom based on distance between all players
    private void ZoomCamera()
    {
        float _greatestDistance = GetGreatestDistanceBetweenPlayers();
        float _newZoom = Mathf.Lerp(_minZoom, _maxZoom, _greatestDistance / _zoomLimiter);
        _camera.orthographicSize = Mathf.Clamp(Mathf.Lerp(_camera.orthographicSize, _newZoom, Time.deltaTime),_minZoom,_maxZoom);
    }

    private Vector3 GetCentrePointOfPlayers()
    {
        if(_playerList.Count == 1)
        {
            //If there is only one player, the centre point is their position
            return _playerList[0].position;
        }

        var bounds = new Bounds(_playerList[0].position,Vector3.zero);
        foreach(Transform player in _playerList)
        {
            //Expand the bounds to include all player positions
            bounds.Encapsulate(player.position);
        }

        //Return the average position of all players
        return bounds.center;
    }

    private float GetGreatestDistanceBetweenPlayers()
    {
        if(_playerList.Count <= 1) return 0f;

        var bounds = new Bounds(_playerList[0].position, Vector3.zero);
        foreach (Transform player in _playerList)
        {
            //Expand the bounds to include all player positions
            bounds.Encapsulate(player.position);
        }

        //Use the larger of the 2 dimensions to accommodate both for 2D and 3D views
        return Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
    }
}
