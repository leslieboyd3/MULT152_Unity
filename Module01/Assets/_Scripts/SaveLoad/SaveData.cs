using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int version = 1;
    public string sceneName;
    public Vector3Simple playerPos;
    public int health;
    public List<string> inventory = new();
}

[Serializable]
public struct Vector3Simple
{
    public float x, y, z;
    public Vector3Simple(Vector3 v) { x = v.x; y = v.y; z = v.z; }
    public Vector3 ToVector3() => new(x, y, z);
}