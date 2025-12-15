
// CheckpointSolo.cs
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CheckpointSolo : MonoBehaviour
{
    [Header("Player Detection")]
    [Tooltip("If true, the checkpoint saves the position of whatever enters with the 'Player' tag.")]
    public bool usePlayerTag = true;

    [Tooltip("Use rotation when saving (e.g., for facing direction).")]
    public bool includeRotation = true;

    [Header("Optional Direct Reference")]
    [Tooltip("If specified, this transform will be saved regardless of what triggers the collider.")]
    public Transform explicitPlayerTransform;

    private void Reset()
    {
        var col = GetComponent<Collider>();
        if (col) col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform target = explicitPlayerTransform;

        if (target == null)
        {
            if (usePlayerTag)
            {
                if (!other.CompareTag("Player")) return; // ignore non-player
                target = other.transform;
            }
            else
            {
                // If not using tags, save whatever entered
                target = other.transform;
            }
        }

        SaveSystemSolo.SavePlayer(target, includeRotation);
#if UNITY_EDITOR
        Debug.Log($"[CheckpointSolo] Saved position at {target.position}");
#endif
    }
}