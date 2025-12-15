
// RespawnHelperSolo.cs
using System.Collections;
using UnityEngine;

public static class RespawnHelperSolo
{
    /// <summary>
    /// Respawns the player Transform using the last saved checkpoint (SaveSystemSolo).
    /// If no save exists, optionally uses a fallbackTransform.
    /// </summary>
    public static void Respawn(Transform player, Transform fallbackTransform = null, bool applyRotation = true)
    {
        var go = player.gameObject;
        var respawner = go.GetComponent<RespawnApplierSolo>();
        if (respawner == null)
        {
            respawner = go.AddComponent<RespawnApplierSolo>();
        }

        respawner.ApplyRespawn(applyRotation, fallbackTransform);
    }
}

/// <summary>
/// A tiny MonoBehaviour used to run a coroutine on the player object to safely apply the respawn.
/// </summary>
public class RespawnApplierSolo : MonoBehaviour
{
    [Tooltip("If true, snap on next FixedUpdate to avoid physics jitter.")]
    public bool snapInFixedUpdate = true;

    [Tooltip("Reset Rigidbody velocities when respawning.")]
    public bool resetRigidbodyVelocity = true;

    private Rigidbody rb;
    private CharacterController cc;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();
    }

    public void ApplyRespawn(bool applyRotation, Transform fallbackTransform)
    {
        if (SaveSystemSolo.TryLoad(out var data))
        {
            StartCoroutine(ApplySpawnCoroutine(data.playerPosition, applyRotation ? (Quaternion?)data.playerRotation : null));
        }
        else if (fallbackTransform != null)
        {
            StartCoroutine(ApplySpawnCoroutine(fallbackTransform.position, applyRotation ? (Quaternion?)fallbackTransform.rotation : null));
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("[RespawnHelperSolo] No save found and no fallback provided; staying in place.");
#endif
        }
    }

    private IEnumerator ApplySpawnCoroutine(Vector3 newPos, Quaternion? newRot)
    {
        // Temporarily disable CharacterController to allow setting transform directly
        bool hadCC = cc != null && cc.enabled;
        if (hadCC) cc.enabled = false;

        if (snapInFixedUpdate)
            yield return new WaitForFixedUpdate();

        if (rb != null)
        {
            // Move rigidbody safely
            rb.position = newPos;
            if (newRot.HasValue) rb.rotation = newRot.Value;

            if (resetRigidbodyVelocity)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.Sleep();
            }
        }
        else
        {
            transform.position = newPos;
            if (newRot.HasValue) transform.rotation = newRot.Value;
        }

        if (hadCC) cc.enabled = true;

#if UNITY_EDITOR
        Debug.Log($"[RespawnHelperSolo] Respawned to {newPos}");
#endif
    }
}