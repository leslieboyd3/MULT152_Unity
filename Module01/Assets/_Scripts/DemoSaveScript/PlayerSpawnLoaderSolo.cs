
// PlayerSpawnLoaderSolo.cs
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerSpawnLoaderSolo : MonoBehaviour
{
    [Header("Options")]
    [Tooltip("If true, move the player during the next FixedUpdate to avoid physics jitter.")]
    public bool snapInFixedUpdate = true;

    [Tooltip("If the player has a Rigidbody, reset velocities when applying the saved position.")]
    public bool resetRigidbodyVelocity = true;

    [Tooltip("Apply saved rotation (if present).")]
    public bool applyRotation = true;

    private Rigidbody rb;
    private CharacterController cc;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();

        if (SaveSystemSolo.TryLoad(out var data))
        {
            StartCoroutine(ApplySpawn(data));
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("[PlayerSpawnLoaderSolo] No save found; spawning at default position.");
#endif
        }
    }

    private IEnumerator ApplySpawn(SaveDataSolo data)
    {
        // Temporarily disable CharacterController to allow setting transform directly
        bool hadCC = cc != null && cc.enabled;
        if (hadCC) cc.enabled = false;

        if (snapInFixedUpdate)
            yield return new WaitForFixedUpdate();

        // Apply position (and rotation if desired)
        if (rb != null)
        {
            // For Rigidbody, set position/rotation directly before physics resumes
            rb.position = data.playerPosition;

            if (applyRotation)
                rb.rotation = data.playerRotation;

            if (resetRigidbodyVelocity)
            {
                rb.linearVelocity = Vector3.zero;        // <-- fixed
                rb.angularVelocity = Vector3.zero;
                rb.Sleep();
            }
        }
        else
        {
            transform.position = data.playerPosition;

            if (applyRotation)
                transform.rotation = data.playerRotation;
        }

        // Re-enable CharacterController if needed
        if (hadCC) cc.enabled = true;

#if UNITY_EDITOR
        Debug.Log($"[PlayerSpawnLoaderSolo] Applied saved spawn at {data.playerPosition}");
#endif

        yield break;
    }
}