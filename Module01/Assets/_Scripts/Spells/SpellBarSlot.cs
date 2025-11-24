using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SpellBarSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI")]
    public Image iconImage;
    public Image cooldownFill;   // radial filled image
    public TMP_Text keyText;

    [Header("Runtime")]
    public int slotIndex;
    public SpellDefinition spell;

    SpellTooltipUI _tooltip;

    public void Setup(int index, SpellDefinition s, string keyLabel, SpellTooltipUI tooltip)
    {
        slotIndex = index;
        spell = s;
        _tooltip = tooltip;

        if (keyText) keyText.text = keyLabel;
        if (iconImage)
        {
            iconImage.enabled = (spell != null && spell.icon);
            iconImage.sprite = spell ? spell.icon : null;
        }
        if (cooldownFill)
        {
            cooldownFill.type = Image.Type.Filled;
            cooldownFill.fillMethod = Image.FillMethod.Radial360;
            cooldownFill.fillAmount = 0f;
            cooldownFill.enabled = false;
        }
    }

    public void SetCooldown(float tNorm) // 0..1
    {
        if (!cooldownFill) return;
        cooldownFill.enabled = tNorm > 0f;
        cooldownFill.fillAmount = Mathf.Clamp01(tNorm);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_tooltip && spell) _tooltip.Show(spell, transform as RectTransform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tooltip?.Hide();
    }

    // Optional: visual flash on cast
    public void Flash()
    {
        // simple alpha pulse or outline; left for brevity
    }
}