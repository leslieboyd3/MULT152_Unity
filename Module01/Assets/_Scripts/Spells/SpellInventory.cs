using System.Collections.Generic;
using UnityEngine;

public class SpellInventory : MonoBehaviour
{
    [Header("Available Spells")]
    public List<SpellDefinition> allSpells = new();

    [Header("Hotbar (0..7)")]
    public SpellDefinition[] hotbar = new SpellDefinition[8];

    public int SelectedIndex { get; private set; } = 0; // 0..7

    public SpellDefinition Current => hotbar != null && hotbar.Length > 0 ? hotbar[Mathf.Clamp(SelectedIndex, 0, hotbar.Length - 1)] : null;

    public void SetSlot(int index, SpellDefinition spell)
    {
        if (index < 0 || index >= hotbar.Length) return;
        hotbar[index] = spell;
    }

    public void SelectIndex(int index)
    {
        if (index < 0 || index >= hotbar.Length) return;
        SelectedIndex = index;
    }

    public void Next()
    {
        SelectedIndex = (SelectedIndex + 1) % hotbar.Length;
    }

    public void Prev()
    {
        SelectedIndex = (SelectedIndex - 1 + hotbar.Length) % hotbar.Length;
    }
}