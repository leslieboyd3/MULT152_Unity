using System;
using System.IO;
using UnityEngine;

public static class JsonSaveSystem
{
    static string Dir => Application.persistentDataPath;
    static string PathFor(string slot) => System.IO.Path.Combine(Dir, $"{slot}.json");

    public static bool Save(string slot, SaveData data)
    {
        try{
            string json = JsonUtility.ToJson(data, prettyPrint:false);
            string tmp = PathFor(slot) + ".tmp";
            File.WriteAllText(tmp, json);
            if (File.Exists(PathFor(slot))) File.Replace(tmp, PathFor(slot), null);
            else File.Move(tmp, PathFor(slot));
            return true;
        } catch (Exception e){ Debug.LogError(e); return false; }
    }

    public static bool Load(string slot, out SaveData data)
    {
        data = null;
        try{
            string path = PathFor(slot);
            if (!File.Exists(path)) return false;
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<SaveData>(json);
            return data != null;
        } catch (Exception e){ Debug.LogError(e); return false; }
    }
}