using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class NSTOL_ObjectSyncModel 
{
    [RealtimeProperty(2, true, true)]
    private Vector3 _position;
    //private Vector3 _rotation;

}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class NSTOL_ObjectSyncModel : IModel {
    // Properties
    public UnityEngine.Vector3 position {
        get { return _cache.LookForValueInCache(_position, entry => entry.positionSet, entry => entry.position); }
        set { if (value == position) return; _cache.UpdateLocalCache(entry => { entry.positionSet = true; entry.position = value; return entry; }); FirePositionDidChange(value); }
    }
    
    // Events
    public delegate void PositionDidChange(NSTOL_ObjectSyncModel model, UnityEngine.Vector3 value);
    public event         PositionDidChange positionDidChange;
    
    // Delta updates
    private struct LocalCacheEntry {
        public bool                positionSet;
        public UnityEngine.Vector3 position;
    }
    
    private LocalChangeCache<LocalCacheEntry> _cache;
    
    public NSTOL_ObjectSyncModel() {
        _cache = new LocalChangeCache<LocalCacheEntry>();
    }
    
    // Events
    public void FirePositionDidChange(UnityEngine.Vector3 value) {
        try {
            if (positionDidChange != null)
                positionDidChange(this, value);
        } catch (System.Exception exception) {
            Debug.LogException(exception);
        }
    }
    
    // Serialization
    enum PropertyID {
        Position = 2,
    }
    
    public int WriteLength(StreamContext context) {
        int length = 0;
        
        if (context.fullModel) {
            // Mark unreliable properties as clean and flatten the in-flight cache.
            // TODO: Move this out of WriteLength() once we have a prepareToWrite method.
            _position = position;
            _cache.Clear();
            
            // Write all properties
            length += WriteStream.WriteBytesLength((uint)PropertyID.Position, WriteStream.Vector3ToBytesLength());
        } else {
            // Reliable properties
            if (context.reliableChannel) {
                LocalCacheEntry entry = _cache.localCache;
                if (entry.positionSet)
                    length += WriteStream.WriteBytesLength((uint)PropertyID.Position, WriteStream.Vector3ToBytesLength());
            }
        }
        
        return length;
    }
    
    public void Write(WriteStream stream, StreamContext context) {
        if (context.fullModel) {
            // Write all properties
            stream.WriteBytes((uint)PropertyID.Position, WriteStream.Vector3ToBytes(_position));
        } else {
            // Reliable properties
            if (context.reliableChannel) {
                LocalCacheEntry entry = _cache.localCache;
                if (entry.positionSet)
                    _cache.PushLocalCacheToInflight(context.updateID);
                
                if (entry.positionSet)
                    stream.WriteBytes((uint)PropertyID.Position, WriteStream.Vector3ToBytes(entry.position));
            }
        }
    }
    
    public void Read(ReadStream stream, StreamContext context) {
        bool positionExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.positionSet);
        
        // Remove from in-flight
        if (context.deltaUpdatesOnly && context.reliableChannel)
            _cache.RemoveUpdateFromInflight(context.updateID);
        
        // Loop through each property and deserialize
        uint propertyID;
        while (stream.ReadNextPropertyID(out propertyID)) {
            switch (propertyID) {
                case (uint)PropertyID.Position: {
                    UnityEngine.Vector3 previousValue = _position;
                    
                    _position = ReadStream.Vector3FromBytes(stream.ReadBytes());
                    
                    if (!positionExistsInChangeCache && _position != previousValue)
                        FirePositionDidChange(_position);
                    break;
                }
                default:
                    stream.SkipProperty();
                    break;
            }
        }
    }
}
/* ----- End Normal Autogenerated Code ----- */