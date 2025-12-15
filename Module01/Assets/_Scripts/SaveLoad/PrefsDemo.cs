using UnityEngine;

public class PrefsDemo : MonoBehaviour
{
    const string VOL = "audio_volume";
    const string INVERTY = "invert_y";

    public void SavePrefs(float volume, bool invertY)
    {
        PlayerPrefs.SetFloat(VOL, Mathf.Clamp01(volume));
        PlayerPrefs.SetInt(INVERTY, invertY ? 1 : 0);
        PlayerPrefs.Save(); // force flush
    }

    public (float volume, bool invertY) LoadPrefs()
    {
        float v = PlayerPrefs.GetFloat(VOL, 0.8f);
        bool inv = PlayerPrefs.GetInt(INVERTY, 0) == 1;
        return (v, inv);
    }

    public void WipePrefs() => PlayerPrefs.DeleteAll();
}