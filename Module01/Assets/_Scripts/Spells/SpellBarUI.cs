using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SpellBarUI : MonoBehaviour
{
    [Header("Slots (8)")]
    public SpellBarSlot[] slots;

    [Header("Selection")]
    public Image selectionFrame; // a highlight image you move under the selected slot

    [Header("Tooltip")]
    public SpellTooltipUI tooltip;

    [Header("Coloring")]
    public Color selectedColor = Color.white;
    public Color normalColor = new Color(1, 1, 1, 0.7f);

    SpellInventory _inv;
    Coroutine[] _cooling;

    void Awake()
    {
        _inv = FindAnyObjectByType<SpellInventory>(FindObjectsInactive.Include);
        _cooling = new Coroutine[slots.Length];
    }

    public void PopulateFromInventory()
    {
        if (!_inv || slots == null) return;

        for (int i = 0; i < slots.Length; i++)
        {
            var key = (i + 1).ToString(); // 1..8
            slots[i].Setup(i, (i < _inv.hotbar.Length) ? _inv.hotbar[i] : null, key, tooltip);
        }

        SetSelected(_inv.SelectedIndex);
    }

    public void SetSelected(int index)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            var img = slots[i]?.iconImage;
            if (img) img.color = (i == index) ? selectedColor : normalColor;
        }

        if (selectionFrame && index >= 0 && index < slots.Length)
        {
            selectionFrame.transform.SetParent(slots[index].transform, false);
            selectionFrame.rectTransform.anchoredPosition = Vector2.zero;
            selectionFrame.gameObject.SetActive(true);
        }
    }

    public void StartCooldown(int slotIndex, float seconds)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length) return;
        if (_cooling[slotIndex] != null) StopCoroutine(_cooling[slotIndex]);
        _cooling[slotIndex] = StartCoroutine(CooldownRoutine(slots[slotIndex], seconds));
    }

    IEnumerator CooldownRoutine(SpellBarSlot slot, float seconds)
    {
        float t = seconds;
        while (t > 0f)
        {
            slot.SetCooldown(t / seconds);
            t -= Time.deltaTime;
            yield return null;
        }
        slot.SetCooldown(0f);
    }

    public void Flash(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length) return;
        slots[slotIndex]?.Flash();
    }
}