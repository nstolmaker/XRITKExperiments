using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.XR.Interaction.Toolkit;

public class NSTOL_ObjectSync : RealtimeComponent
{
    private Transform _transform;
    //private Vector3 _rotation;
    private NSTOL_ObjectSyncModel _model;

    private Vector3 _previousPosition;
    private Vector3 _position;

    private void Start()
    {
        //_model = GetComponent<NSTOL_ObjectSyncModel>();
        _transform = GetComponent<Transform>();
        _position = _transform.position;
    }

    private void Update()
    {
        if (transform.position != _previousPosition)
        {
            SetPosition(transform.position);
            _previousPosition = transform.position;
        }
    }

    private NSTOL_ObjectSyncModel model 
    {
        set
        {
            if (_model != null)
            {
                _model.positionDidChange -= PositionDidChange;
            }

            _model = value;

            if (_model != null)
            {
                UpdateObjectPosition();

                _model.positionDidChange += PositionDidChange;
            }
        }
    }

    private void PositionDidChange(NSTOL_ObjectSyncModel model, Vector3 value)
    {
        UpdateObjectPosition();
    }

    private void UpdateObjectPosition()
    {
        //DebugHelpers.Log("Running UpdateObjectPosition in NSTOL_ObjectSync");
        _transform.position = _model.position;
    }

    public void SetPosition(Vector3 position)
    {
        //DebugHelpers.Log("Running SetPosition!! in NSTOL_ObjectSync");
        // Set the color on the model
        // This will fire the colorChanged event on the model, which will update the renderer for both the local player and all remote players.
        _model.position = position;
    }
}
