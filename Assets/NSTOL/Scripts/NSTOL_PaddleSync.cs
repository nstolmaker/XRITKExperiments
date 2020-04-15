using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class NSTOL_PaddleSync : RealtimeComponent
{ 
    private Vector3 _position;
    private Quaternion _rotation;
    private NSTOL_PaddleSyncModel _model;

    void Start()
    {
         
    }

    private NSTOL_PaddleSyncModel model
    {
        set
        {
            if (_model != null)
            {
                _model.positionDidChange -= PositionDidChange;
                //_model.rotationDidChange -= RotationDidChange;
            }

            _model = value;

            if (_model != null)
            {
                UpdatePaddle();
                _model.positionDidChange += PositionDidChange;
                //_model.rotationDidChange += RotationDidChange;
            }
        }

    }

    private void PositionDidChange(NSTOL_PaddleSyncModel model, Vector3 value)
    {
        UpdatePaddle();
    }

    private void RotationDidChange(NSTOL_PaddleSyncModel model, Vector3 value)
    {
        // i think the position trigger should be enough to trigger both rotation and position updates. if we do both of them like this, but have them both target the same function (updatePaddle()) it seems like it gets stuck in an infinite loop.
        //UpdatePaddle();
    }


    // called by remote server via positionDidChange
    private void UpdatePaddle()
    {
        // in the future maybe it'd be useful to store the target transform, but for now we can safely assume it's a component on the current object
        //transform.position = _model.position;
        //transform.eulerAngles = _model.rotation;
        transform.SetPositionAndRotation(_model.position, _model.rotation);
    }

    public void SetPaddleTransform(Transform remoteTransform)
    {
        // Set the position and rotation on the model
        // This will fire the positionDidChange and rotationDidChange sevent on the model, which will update the renderer for both the local player and all remote players.
        _model.rotation = remoteTransform.rotation;
        _model.position = remoteTransform.position;
    }
}
