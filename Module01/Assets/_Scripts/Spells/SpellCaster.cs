using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpellInventory))]
public class SpellCaster : MonoBehaviour
{
    [Header("Refs")]
    public Transform castOrigin; // muzzle or camera; set to Camera for 3rd/1st person
    public Camera aimCamera;     // used for forward direction
    public SpellBarUI spellBarUI; // UI hook (updates slots/selection/cooldowns)

    [Header("Input")]
    public InputActionAsset actions;
    public string mapName = "Gameplay";
    public string castAction = "Cast";
    public string nextAction = "NextSpell";
    public string prevAction = "PrevSpell";
    public string[] slotActions = { "Slot1", "Slot2", "Slot3", "Slot4", "Slot5", "Slot6", "Slot7", "Slot8" };

    SpellInventory _inv;
    InputAction _cast, _next, _prev;
    InputAction[] _slots;
    float _cooldownTimer;

    void Awake()
    {
        _inv = GetComponent<SpellInventory>();
        if (!aimCamera) aimCamera = Camera.main;
        if (!castOrigin && aimCamera) castOrigin = aimCamera.transform;
    }

    void OnEnable()
    {
        if (!actions) return;
        var map = actions.FindActionMap(mapName, false);
        if (map != null)
        {
            _cast = map.FindAction(castAction, false); _cast?.Enable();
            _next = map.FindAction(nextAction, false); _next?.Enable();
            _prev = map.FindAction(prevAction, false); _prev?.Enable();

            _slots = new InputAction[slotActions.Length];
            for (int i = 0; i < slotActions.Length; i++)
            {
                _slots[i] = map.FindAction(slotActions[i], false);
                _slots[i]?.Enable();
            }
        }
    }

    void OnDisable()
    {
        _cast?.Disable();
        _next?.Disable();
        _prev?.Disable();
        if (_slots != null)
        {
            foreach (var a in _slots) a?.Disable();
        }
    }

    void Update()
    {
        // Slot selection (1..8)
        if (_slots != null)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i] != null && _slots[i].WasPressedThisFrame())
                {
                    _inv.SelectIndex(i);
                    spellBarUI?.SetSelected(i);
                }
            }
        }

        if (_next != null && _next.WasPressedThisFrame())
        {
            _inv.Next();
            spellBarUI?.SetSelected(_inv.SelectedIndex);
        }

        if (_prev != null && _prev.WasPressedThisFrame())
        {
            _inv.Prev();
            spellBarUI?.SetSelected(_inv.SelectedIndex);
        }

        if (_cooldownTimer > 0f) _cooldownTimer -= Time.deltaTime;

        if (_cast != null && _cast.WasPressedThisFrame())
        {
            TryCast();
        }
    }

    void TryCast()
    {
        var spell = _inv.Current;
        if (spell == null) return;
        if (_cooldownTimer > 0f) return;

        // Direction
        Vector3 dir = aimCamera ? aimCamera.transform.forward : transform.forward;

        // Spread
        if (spell.spreadDegrees > 0f)
        {
            var spread = Random.insideUnitCircle * (spell.spreadDegrees * Mathf.Deg2Rad);
            dir = (Quaternion.AngleAxis(spread.x * Mathf.Rad2Deg, Vector3.up) *
                   Quaternion.AngleAxis(spread.y * Mathf.Rad2Deg, Vector3.right)) * dir;
        }

        // Projectile
        if (spell.projectilePrefab)
        {
            var go = Instantiate(spell.projectilePrefab, castOrigin.position, Quaternion.LookRotation(dir));
            var proj = go.GetComponent<Projectile>();
            if (!proj) proj = go.AddComponent<Projectile>();

            proj.Init(transform, dir * spell.projectileSpeed, spell.damage, spell.lifeSeconds);
        }

        // VFX & SFX
        if (spell.castVfxPrefab)
            Instantiate(spell.castVfxPrefab, castOrigin.position, Quaternion.LookRotation(dir));
        if (spell.castSfx)
            AudioSource.PlayClipAtPoint(spell.castSfx, castOrigin.position, 0.9f);

        // Start cooldown
        _cooldownTimer = Mathf.Max(0.01f, spell.cooldown);
        spellBarUI?.StartCooldown(_inv.SelectedIndex, _cooldownTimer);

        // UI notify (icon highlight/flash)
        spellBarUI?.Flash(_inv.SelectedIndex);
    }
}