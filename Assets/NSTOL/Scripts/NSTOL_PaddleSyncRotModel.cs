using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime.Serialization;

[RealtimeModel]
public class NSTOL_PaddleSyncRotModel 
{
    [RealtimeProperty(5, true, true)]
    private Vector3 _rotation;

   
}