using UnityEngine;
using TMPro;

public class SpellTooltipUI : MonoBehaviour
{
    public RectTransform panel;
    public TMP_Text titleText;
    public TMP_Text descText;
    public TMP_Text tipText;

    Canvas _canvas;

    void Awake()
    {
        _canvas = GetComponentInParent<Canvas>(true);
        Hide();
    }

    public void Show(SpellDefinition spell, RectTransform targetSlot)
    {
        if (!panel || !spell) return;

        titleText.text = spell.spellName;
        descText.text = spell.description;
        tipText.text = spell.tip;

        panel.gameObject.SetActive(true);

        // Position near slot
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, targetSlot.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, screenPos, null, out var local);
        panel.anchoredPosition = local + new Vector2(0f, 80f); // offset above
    }

    public void Hide()
    {
        if (panel) panel.gameObject.SetActive(false);
    }
}