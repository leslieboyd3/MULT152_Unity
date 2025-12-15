
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;   // for Button
using TMPro;            // for TMP_Text, TMP_InputField

namespace NetcodeDemo {
    public class StartupGUI : MonoBehaviour {
        [Header("TMP/UGUI References")]
        [SerializeField] private TMP_InputField addressField;
        [SerializeField] private TMP_InputField portField;
        [SerializeField] private Button startHostButton;
        [SerializeField] private Button startClientButton;
        [SerializeField] private Button startServerButton;
        [SerializeField] private TMP_Text statusText; // optional

        [Header("Defaults")]
        [SerializeField] private string defaultAddress = "127.0.0.1";
        [SerializeField] private ushort defaultPort = 7777;

        private string address;
        private ushort port;

        void Awake() {
            // Initialize fields
            address = defaultAddress;
            port    = defaultPort;

            if (addressField) addressField.text = defaultAddress;
            if (portField)    portField.text    = defaultPort.ToString();

            // Wire button events
            if (startHostButton)   startHostButton.onClick.AddListener(StartHost);
            if (startClientButton) startClientButton.onClick.AddListener(StartClient);
            if (startServerButton) startServerButton.onClick.AddListener(StartServer);
        }

        /// <summary>
        /// Read and validate the Address and Port input fields.
        /// </summary>
        private void ReadInputs() {
            // Address
            if (addressField) {
                var raw = addressField.text?.Trim();
                address = string.IsNullOrEmpty(raw) ? defaultAddress : raw;
            } else {
                address = defaultAddress;
            }

            // Port
            if (portField && ushort.TryParse(portField.text, out var parsed)) {
                port = parsed;
            } else {
                port = defaultPort;
                SetStatus($"Invalid port, using {defaultPort}.");
            }
        }

        private void StartHost() {
            ReadInputs();
            var nm  = NetworkManager.Singleton;
            if (nm == null) { SetStatus("NetworkManager not found."); return; }

            var utp = nm.GetComponent<UnityTransport>();
            if (utp == null) { SetStatus("UnityTransport not found on NetworkManager."); return; }

            // Unity Transport 2.x SetConnectionData(ip, port [, listenAddr])
            utp.SetConnectionData(address, port);

            bool ok = nm.StartHost();
            SetStatus(ok ? $"Host started {address}:{port}" : "Failed to start Host");
        }

        private void StartClient() {
            ReadInputs();
            var nm  = NetworkManager.Singleton;
            if (nm == null) { SetStatus("NetworkManager not found."); return; }

            var utp = nm.GetComponent<UnityTransport>();
            if (utp == null) { SetStatus("UnityTransport not found on NetworkManager."); return; }

            utp.SetConnectionData(address, port);

            bool ok = nm.StartClient();
            SetStatus(ok ? $"Client connecting to {address}:{port}" : "Failed to start Client");
        }

        private void StartServer() {
            ReadInputs();
            var nm  = NetworkManager.Singleton;
            if (nm == null) { SetStatus("NetworkManager not found."); return; }

            var utp = nm.GetComponent<UnityTransport>();
            if (utp == null) { SetStatus("UnityTransport not found on NetworkManager."); return; }

            // For dedicated server, listening port matters; address can remain local
            utp.SetConnectionData(address, port);

            bool ok = nm.StartServer();
            SetStatus(ok ? $"Server started on :{port}" : "Failed to start Server");
        }

        private void SetStatus(string msg) {
            if (statusText) statusText.text = msg;
            else Debug.Log(msg);
        }
    }
}
