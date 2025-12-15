
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private SaveDriver saveDriver;
    [SerializeField] private TMP_Text savedMessageText;
    [SerializeField] private float messageDuration = 2f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            saveDriver.SaveSlot("slot1");
            Debug.Log("Checkpoint reached. Autosaved!");
            ShowSaveMessage("Game Saved!!");
        }
    }
    
    
private void ShowSaveMessage(string message)
    {
        if (savedMessageText != null)
        {
            savedMessageText.text = message;
            savedMessageText.gameObject.SetActive(true);
            CancelInvoke(nameof(HideSaveMessage));
            Invoke(nameof(HideSaveMessage), messageDuration);
        }
    }

    private void HideSaveMessage()
    {
        if (savedMessageText != null)
            savedMessageText.gameObject.SetActive(false);
    }
}