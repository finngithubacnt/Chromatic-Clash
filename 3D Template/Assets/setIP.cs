using System.Net;
using System.Net.Sockets;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class HostIPManager : NetworkBehaviour
{
    // This will sync from host to all clients
    public static HostIPManager Instance;
    public NetworkVariable<FixedString128Bytes> hostIP = new NetworkVariable<FixedString128Bytes>();

    void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            string ip = GetLocalIPAddress();
            hostIP.Value = ip;
            Debug.Log($"[HostIPManager] Host IP is {ip}");
        }
        else
        {
            Debug.Log($"[HostIPManager] Joined, waiting for host IP...");
            hostIP.OnValueChanged += (oldVal, newVal) =>
            {
                Debug.Log($"[HostIPManager] Host IP received: {newVal}");
                SetClientIP(newVal.ToString());
            };
        }
    }

    private string GetLocalIPAddress()
    {
        string localIP = "127.0.0.1"; // fallback
        var host = Dns.GetHostEntry(Dns.GetHostName());

        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork) // IPv4 only
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }

    private void SetClientIP(string ip)
    {
        var transport = NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>();
        transport.ConnectionData.Address = ip;
        Debug.Log($"[HostIPManager] Client transport set to {ip}");
    }
}
