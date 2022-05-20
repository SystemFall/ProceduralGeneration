using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Camera camera;
    private Transform _cameraTransform;

    private float _cameraDistance;
    private float _cameraHeight;

    private Transform _player;

    private Vector3 _cameraStartpoint;
    private Vector3 _playerStartpoint;
    
    private Vector3 _currentPosition;
    private float _currentAngle;

    private void Start()
    {
        _player = transform;
        _cameraTransform = camera.transform;
        _cameraStartpoint = _cameraTransform.position;
        _playerStartpoint = _player.position;
        _cameraDistance = 8f;
        _cameraHeight = 4f;
        _currentAngle = 0;
    }
    void Update()
    {
        //Debug.Log();
        _cameraDistance += -Input.GetAxis("Mouse ScrollWheel") * 10;

        if (_cameraDistance > 20)
            _cameraDistance = 20;
        if (_cameraDistance < 0)
            _cameraDistance = 0;

        _currentAngle += -Input.GetAxis("Mouse X") / 15;
        _currentPosition = _player.position;// + _player.right.normalized;

        _currentPosition.y = _player.position.y + _cameraHeight;
        _currentPosition.x += Mathf.Cos(_currentAngle) * _cameraDistance;
        _currentPosition.z += Mathf.Sin(_currentAngle) * _cameraDistance;

        _cameraTransform.position = _currentPosition;

        _cameraTransform.LookAt(_player.position + _cameraTransform.right.normalized * 0.8f + _player.up.normalized * 1.3f, Vector3.up);
    }
}