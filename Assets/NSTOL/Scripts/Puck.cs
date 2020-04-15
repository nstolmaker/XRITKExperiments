using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puck : MonoBehaviour
{

    [SerializeField]
    private Vector3 _position;
    [SerializeField]
    private Vector3 _previousPosition;
    [SerializeField]
    private Quaternion _rotation;
    [SerializeField]
    private Quaternion _previousRotation;
    private NSTOL_PuckSync _syncComponent;

    private void Start()
    {
        _syncComponent = GetComponent<NSTOL_PuckSync>();
    }

    private void Update()
    {
        _rotation = transform.rotation;
        _position = transform.position;
        // TODO: Check for changes to rotation too. Probably dont need to for VR-tracked objects because hands shake and stuff, but its a good idea especially for computer-controlled objects
        if (_position != _previousPosition)
        {
            _previousRotation = _rotation;
            _previousPosition = _position;
            _syncComponent.SetObjectTransform(transform);
        }
    }

    private void FixedUpdate()
    {

    }
}
