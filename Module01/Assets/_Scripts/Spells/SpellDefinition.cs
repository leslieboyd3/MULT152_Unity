// Assets/_Scripts/Spells/SpellDefinition.cs
using UnityEngine;

[CreateAssetMenu(fileName = "Spell_", menuName = "Game/Spell")]
public class SpellDefinition : ScriptableObject
{
    [Header("Display")]
    public string spellName = "Fireball";
    [TextArea] public string description = "Hurls a flaming orb.";
    [TextArea] public string tip = "Great against clustered enemies.";
    public Sprite icon;

    [Header("Casting")]
    public float cooldown = 0.75f;
    public float projectileSpeed = 18f;
    public int damage = 25;
    public float spreadDegrees = 0f;  // 0 for straight, >0 for slight variance
    public float lifeSeconds = 5f;

    [Header("Prefabs & FX")]
    public GameObject projectilePrefab; // requires Projectile.cs + Rigidbody
    public AudioClip castSfx;
    public GameObject castVfxPrefab;    // optional at muzzle/caster hands

    [Header("Flags")]
    public bool requiresLineOfSight = true;
}