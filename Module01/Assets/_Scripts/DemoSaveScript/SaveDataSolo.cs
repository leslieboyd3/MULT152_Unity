
// SaveDataSolo.cs
using System;
using UnityEngine;

[Serializable]
public class SaveDataSolo
{
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public string sceneName;
    public long unixTimeStamp; // keep your original spelling
}