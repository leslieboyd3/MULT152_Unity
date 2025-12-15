using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartupHUD : MonoBehaviour
{
    [Header("TMP UI (optional)")]
    [SerializeField] private TMP_InputField addressField;
    [SerializeField] private TMP_InputField portField;
    [SerializeField] private Button startHostButton;
    [SerializeField] private Button startClientButton;
    [SerializeField] private Button startServerButton;
    [SerializeField] private TMP_Text statusText;

    [Header("Defaults")]
    public string address = "127.0.0.1";
    public ushort port = 7777;

    private bool useTMP;

    
    void Awake() 
    {
        // Detect TMP setup
        useTMP = addressField && portField &&
                 startHostButton && startClientButton && startServerButton;

        if (useTMP) 
        {
            addressField.text = address;
            portField.text    = port.ToString();
            startHostButton.onClick.AddListener(StartHost);
            startClientButton.onClick.AddListener(StartClient);
            startServerButton.onClick.AddListener(StartServer);
        }
    }

    void OnGUI()
    {
        if (useTMP) return;

        GUILayout.BeginArea(new Rect(10, 10, 210, 160), GUI.skin.box);
        GUILayout.Label("NGO Local Test");
        address = GUILayout.TextField(address);
        string portText = GUILayout.TextField(port.ToString());
        if (ushort.TryParse(portText, out var parsed)) port = parsed;

        if (GUILayout.Button("Start Host")) StartHost();
        if (GUILayout.Button("Start Client")) StartClient();
        if (GUILayout.Button("Start Server")) StartServer();
        GUILayout.EndArea();
    }
    
    void ReadInputs() {
        if (useTMP) {
            address = string.IsNullOrWhiteSpace(addressField.text) ? "127.0.0.1" : addressField.text.Trim();
            if (!ushort.TryParse(portField.text, out var parsed)) {
                if (statusText) statusText.text = "Invalid port, using 7777.";
                port = 7777;
            } else port = parsed;
        }
        // Legacy IMGUI already reads values in OnGUI
    }

    void StartHost() {
        ReadInputs();
        var nm  = NetworkManager.Singleton;
        var utp = nm.GetComponent<UnityTransport>();
        utp.SetConnectionData(address, port);
        bool ok = nm.StartHost();
        if (statusText) statusText.text = ok ? $"Host started {address}:{port}" : "Failed to start Host";
    }

    void StartClient() {
        ReadInputs();
        var nm  = NetworkManager.Singleton;
        var utp = nm.GetComponent<UnityTransport>();
        utp.SetConnectionData(address, port);
        bool ok = nm.StartClient();
        if (statusText) statusText.text = ok ? $"Client connecting to {address}:{port}" : "Failed to start Client";
    }

    void StartServer() {
        ReadInputs();
        var nm  = NetworkManager.Singleton;
        var utp = nm.GetComponent<UnityTransport>();
        utp.SetConnectionData(address, port);
        bool ok = nm.StartServer();
        if (statusText) statusText.text = ok ? $"Server started on :{port}" : "Failed to start Server";
    }
}