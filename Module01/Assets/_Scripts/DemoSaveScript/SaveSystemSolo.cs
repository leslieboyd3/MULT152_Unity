
// SaveSystemSolo.cs
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveSystemSolo
{
    private static readonly string SavePath =
        Path.Combine(Application.persistentDataPath, "save.json"); // or "save_solo.json"

    public static void SavePlayer(Transform player, bool includeRotation = true)
    {
        var data = new SaveDataSolo
        {
            playerPosition = player.position,
            playerRotation = includeRotation ? player.rotation : Quaternion.identity,
            sceneName = SceneManager.GetActiveScene().name,
            unixTimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() // <-- fixed name
        };

        var json = JsonUtility.ToJson(data, prettyPrint: true);

        try
        {
            File.WriteAllText(SavePath, json);
#if UNITY_EDITOR
            Debug.Log($"[SaveSystemSolo] Saved to: {SavePath}\n{json}");
#endif
        }
        catch (Exception ex)
        {
            Debug.LogError($"[SaveSystemSolo] Failed to save: {ex.Message}");
        }
    }

    public static bool TryLoad(out SaveDataSolo data)
    {
        data = null;

        if (!File.Exists(SavePath))
        {
#if UNITY_EDITOR
            Debug.Log("[SaveSystemSolo] No save file found.");
#endif
            return false;
        }

        try
        {
            string json = File.ReadAllText(SavePath);
            data = JsonUtility.FromJson<SaveDataSolo>(json);

            if (data == null)
            {
                Debug.LogError("[SaveSystemSolo] Save file exists but deserialized to null.");
                return false;
            }

#if UNITY_EDITOR
            Debug.Log($"[SaveSystemSolo] Loaded save from: {SavePath}");
#endif
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[SaveSystemSolo] Failed to load: {ex.Message}");
            return false;
        }
    }

    public static void ClearSave()
    {
        try
        {
            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
#if UNITY_EDITOR
                Debug.Log("[SaveSystemSolo] Save file deleted.");
#endif
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[SaveSystemSolo] Failed to delete save: {ex.Message}");
        }
    }

    public static string GetSavePath() => SavePath;
}