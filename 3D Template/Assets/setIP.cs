using Unity.Netcode;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class SetIP : MonoBehaviour
{
    void Awake()
    {
        var transport = NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>();

        // get the local machine's IPv4 address
        string localIP = GetLocalIPAddress();
        Debug.Log($"[SetIP] Local IP detected: {localIP}");

        // assign it for the host to listen on
        transport.ConnectionData.Address = localIP;
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
}
