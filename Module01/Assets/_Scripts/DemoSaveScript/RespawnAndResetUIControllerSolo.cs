
// RespawnAndResetUIControllerSolo.cs
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem; // optional

public class RespawnAndResetUIControllerSolo : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private Button continueButton;  // respawn to last checkpoint
    [SerializeField] private Button resetButton;     // reset to hard-coded start
    [SerializeField] private Button cancelButton;    // optional

    [Header("Player & Checkpoints")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform fallbackSpawn;   // used by Continue if no checkpoint

    [Header("Reset (Hard-Coded Destination)")]
    [Tooltip("If set, Reset will move the player to this Transform's position/rotation.")]
    [SerializeField] private Transform resetTargetTransform;

    [Tooltip("If true, use the hard-coded coordinates below when Reset is clicked. If both Transform and coordinates are provided, coordinates win.")]
    [SerializeField] private bool useHardCodedCoordinates = false;

    [Tooltip("Hard-coded world position used on Reset when 'useHardCodedCoordinates' is true.")]
    [SerializeField] private Vector3 resetPosition = new Vector3(0f, 1.8f, 0f);

    [Tooltip("Optional hard-coded world rotation (Euler angles in degrees) used on Reset when 'useHardCodedCoordinates' is true.")]
    [SerializeField] private Vector3 resetRotationEuler = Vector3.zero;

    [Header("Options")]
    [SerializeField] private bool applyRotation = true;
    [SerializeField] private bool snapInFixedUpdateOnReset = true;
    [SerializeField] private bool resetRigidbodyVelocityOnReset = true;
    [Tooltip("Clear checkpoint save when pressing Reset (optional).")]
    [SerializeField] private bool clearCheckpointSaveOnReset = true;

    [Header("Input (Optional)")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private string gameplayActionMap = "Player";
    [SerializeField] private string uiActionMap = "UI";

    // cached
    private Rigidbody rb;
    private CharacterController cc;

    private void Awake()
    {
        if (panelRoot) panelRoot.SetActive(false);

        if (!player)
            Debug.LogWarning("[RespawnAndResetUI] Player not assigned.");
        else
        {
            rb = player.GetComponent<Rigidbody>();
            cc = player.GetComponent<CharacterController>();
        }

        if (continueButton) continueButton.onClick.AddListener(OnContinueClicked);
        if (resetButton)    resetButton.onClick.AddListener(OnResetClicked);
        if (cancelButton)   cancelButton.onClick.AddListener(Hide);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Show()
    {
        if (!panelRoot) return;

        panelRoot.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        EventSystem.current?.SetSelectedGameObject(continueButton ? continueButton.gameObject : resetButton?.gameObject);
        playerInput?.SwitchCurrentActionMap(uiActionMap);
    }

    public void Hide()
    {
        if (panelRoot) panelRoot.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerInput?.SwitchCurrentActionMap(gameplayActionMap);
    }

    // Continue: respawn to last checkpoint (using your existing helper)
    private void OnContinueClicked()
    {
        if (!player)
        {
            Debug.LogWarning("[RespawnAndResetUI] No player assigned.");
            Hide();
            return;
        }

        RespawnHelperSolo.Respawn(player, fallbackSpawn, applyRotation);
        Hide();
    }

    // Reset: move to hard-coded location specified by user
    private void OnResetClicked()
    {
        if (!player)
        {
            Debug.LogWarning("[RespawnAndResetUI] No player assigned.");
            Hide();
            return;
        }

        if (clearCheckpointSaveOnReset)
        {
            try { SaveSystemSolo.ClearSave(); } catch { /* ignore if not present */ }
        }

        StartCoroutine(ApplyResetToSpecificLocation());
    }

    private IEnumerator ApplyResetToSpecificLocation()
    {
        // Choose destination based on user-provided settings
        Vector3 targetPos;
        Quaternion targetRot = Quaternion.identity;

        if (useHardCodedCoordinates)
        {
            targetPos = resetPosition;
            if (applyRotation)
                targetRot = Quaternion.Euler(resetRotationEuler);
        }
        else if (resetTargetTransform != null)
        {
            targetPos = resetTargetTransform.position;
            if (applyRotation)
                targetRot = resetTargetTransform.rotation;
        }
        else
        {
            Debug.LogWarning("[RespawnAndResetUI] No hard-coded reset target provided; staying put.");
            Hide();
            yield break;
        }

        bool hadCC = cc != null && cc.enabled;
        if (hadCC) cc.enabled = false;

        if (snapInFixedUpdateOnReset)
            yield return new WaitForFixedUpdate();

        // Apply position/rotation safely
        if (rb != null)
        {
            rb.position = targetPos;
            if (applyRotation) rb.rotation = targetRot;

            if (resetRigidbodyVelocityOnReset)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.Sleep();
            }
        }
        else
        {
            player.position = targetPos;
            if (applyRotation) player.rotation = targetRot;
        }

        if (hadCC) cc.enabled = true;

        Hide();

#if UNITY_EDITOR
        Debug.Log($"[RespawnAndResetUI] Reset to hard-coded location {targetPos} (rot: {(applyRotation ? targetRot.eulerAngles.ToString() : "unchanged")}).");
#endif
    }
}