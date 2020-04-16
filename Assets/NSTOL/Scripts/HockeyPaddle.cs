using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HockeyPaddle : MonoBehaviour
{
    /*
    [SerializeField]
    private Vector3 _position;
    private Vector3 _previousPosition;
    private Quaternion _rotation;
    private Quaternion _previousRotation;
    private NSTOL_PaddleSync _paddleSync;

    public string playerName = "Player Name";

    private void Start()
    {
        _paddleSync = GetComponent<NSTOL_PaddleSync>();
    }

    private void Update()
    {
        // TODO: Check for changes to rotation too. Probably dont need to for VR-tracked objects because hands shake and stuff, but its a good idea especially for computer-controlled objects
        if (_position != _previousPosition)
        {
            _previousRotation = _rotation;
            _previousPosition = _position;
            _paddleSync.SetPaddleTransform(transform);
        }
    }

    private void FixedUpdate()
    {
        _rotation = transform.rotation;
        _position = transform.position;
    }
    */
}
