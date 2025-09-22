using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Canvas menuCanvas;

    public void HostGame()
    {
        LanDiscovery.Instance.StartHostDiscovery();

        NetworkManager.Singleton.StartHost();
        menuCanvas.enabled = false;
    }

    public void JoinGame()
    {
        LanDiscovery.Instance.StartClientDiscovery();

        // Try to connect once we discover an IP
        InvokeRepeating(nameof(TryJoinDiscoveredHost), 1f, 1f);
    }

    private void TryJoinDiscoveredHost()
    {
        string ip = LanDiscovery.Instance.GetDiscoveredIP();
        if (!string.IsNullOrEmpty(ip))
        {
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.ConnectionData.Address = ip;

            Debug.Log($"[MainMenu] Connecting to discovered host: {ip}");
            NetworkManager.Singleton.StartClient();

            menuCanvas.enabled = false;
            CancelInvoke(nameof(TryJoinDiscoveredHost));
        }
    }
}
