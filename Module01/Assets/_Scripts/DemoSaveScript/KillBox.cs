
// KillBox.cs
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class KillBox : MonoBehaviour
{
    [Header("Detection")]
    [Tooltip("If true, only objects tagged 'Player' will trigger the UI.")]
    [SerializeField] private bool usePlayerTag = true;

    [Header("UI")]
    [Tooltip("Reference to the unified UI controller that handles Continue/Reset and closing the panel.")]
    [SerializeField] private RespawnAndResetUIControllerSolo respawnResetUI;

    private void Reset()
    {
        var col = GetComponent<Collider>();
        if (col) col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only react to player if requested
        if (usePlayerTag && !other.CompareTag("Player"))
            return;

        // Show the death/reset panel
        if (respawnResetUI != null)
        {
            respawnResetUI.Show();
        }
#if UNITY_EDITOR
        else
        {
            Debug.LogWarning("[KillBox] RespawnAndResetUIControllerSolo is not assigned. The UI will not show.");
        }
#endif
    }
}