using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputBridge input;

    public GameObject menuPanel;
    public Button closeButton;

    public static bool IsMenuOpen { get; private set; }

    void Start()
    {
        if (menuPanel != null)
            menuPanel.SetActive(false);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseMenu);

        HideCursor();
    }

    void Update()
    {
        // Use InputBridge instead of raw key checks
        if (input != null && input.menuTogglePressed)
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        IsMenuOpen = !IsMenuOpen;
        menuPanel.SetActive(IsMenuOpen);
        Time.timeScale = IsMenuOpen ? 0f : 1f;

        if (IsMenuOpen)
            ShowCursor();
        else
            HideCursor();
    }

    public void CloseMenu()
    {
        IsMenuOpen = false;
        menuPanel.SetActive(false);
        Time.timeScale = 1f;
        HideCursor();
    }

    private void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}