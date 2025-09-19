using Unity.Netcode;
using UnityEngine;

public class SetIP : MonoBehaviour
{
    public string ipAddress = "10.104.138.29";

    void Start()
    {
        var transport = NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>();
        transport.ConnectionData.Address = ipAddress;
    }
}
