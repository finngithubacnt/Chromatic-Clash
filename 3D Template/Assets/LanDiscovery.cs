using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class LanDiscovery : MonoBehaviour
{
    public static LanDiscovery Instance;

    private UdpClient udpClient;
    private Thread listenThread;
    private bool running;

    private string discoveredIP = null;
    private int port = 7777; // default NGO port

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void OnDestroy()
    {
        StopDiscovery();
    }

    // ---------------- Host ----------------
    public void StartHostDiscovery()
    {
        StopDiscovery();
        running = true;

        udpClient = new UdpClient();
        udpClient.EnableBroadcast = true;

        // Start broadcasting on a background thread
        listenThread = new Thread(() =>
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Broadcast, 47777);
            string localIP = GetLocalIPAddress();

            while (running)
            {
                string message = $"NGO_HOST:{localIP}:{port}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                udpClient.Send(data, data.Length, ep);
                Thread.Sleep(1000);
            }
        });
        listenThread.IsBackground = true;
        listenThread.Start();

        Debug.Log("[LanDiscovery] Broadcasting host...");
    }

    // ---------------- Client ----------------
    public void StartClientDiscovery()
    {
        StopDiscovery();
        running = true;

        udpClient = new UdpClient(47777);

        listenThread = new Thread(() =>
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
            while (running)
            {
                byte[] data = udpClient.Receive(ref ep);
                string message = Encoding.UTF8.GetString(data);

                if (message.StartsWith("NGO_HOST"))
                {
                    string[] parts = message.Split(':');
                    discoveredIP = parts[1];
                    Debug.Log($"[LanDiscovery] Found host at {discoveredIP}");
                }
            }
        });
        listenThread.IsBackground = true;
        listenThread.Start();

        Debug.Log("[LanDiscovery] Listening for host...");
    }

    public string GetDiscoveredIP()
    {
        return discoveredIP;
    }

    public void StopDiscovery()
    {
        running = false;
        if (udpClient != null) udpClient.Close();
        if (listenThread != null) listenThread.Abort();
        udpClient = null;
        listenThread = null;
    }

    private string GetLocalIPAddress()
    {
        string localIP = "127.0.0.1";
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }
}
