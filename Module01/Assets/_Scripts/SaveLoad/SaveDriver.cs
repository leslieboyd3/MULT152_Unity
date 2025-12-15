using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveDriver : MonoBehaviour
{
    public Transform player;
    public int health = 100;

    public void SaveSlot(string slot="slot1")
    {
        var data = new SaveData{
            sceneName = SceneManager.GetActiveScene().name,
            playerPos = new Vector3Simple(player.position),
            health = health
        };
        JsonSaveSystem.Save(slot, data);
    }

    public void LoadSlot(string slot="slot1")
    {
        if (JsonSaveSystem.Load(slot, out var data))
        {
            // Basic version check
            if (data.version != 1) Debug.LogWarning("Version mismatch");
            // If scene differs, you’d async-load then apply position after load.
            player.position = data.playerPos.ToVector3();
            health = data.health;
        }
    }

    public void WipeSlot(string slot="slot1")
    {
        var p = Application.persistentDataPath + "/" + slot + ".json";
        if (System.IO.File.Exists(p)) System.IO.File.Delete(p);
    }
}