using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
public class NSTOL_PuckSync : RealtimeComponent
{
    [SerializeField]
    public Vector3 _position;
    [SerializeField]
    public Quaternion _rotation;
    private NSTOL_PuckSyncModel _model;



    private NSTOL_PuckSyncModel model
    {
        set
        {
            if (_model != null)
            {
                _model.positionDidChange -= ObjectDidChange;
            }
            _model = value;
            if (_model != null)
            {
                _model.positionDidChange += ObjectDidChange;
            }
        }
    }

    private void ObjectDidChange(NSTOL_PuckSyncModel model, Vector3 value)
    {
        UpdateObjectTransform();
    }


    private void UpdateObjectTransform()
    {
        transform.SetPositionAndRotation(_model.position, _model.rotation);
    }

    public void SetObjectTransform(Transform remoteTransform)
    {
        _model.position = remoteTransform.position;
        _model.rotation = remoteTransform.rotation;
    }

}
