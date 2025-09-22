using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private Canvas menuCanvas; 

    public void HostGame()
    {
        NetworkManager.Singleton.StartHost();
        menuCanvas.enabled = false;


    }

    public void JoinGame()
    {
        NetworkManager.Singleton.StartClient();
        menuCanvas.enabled = false; 
    }
    public class SetIP : MonoBehaviour
    {
        public string ipAddress = "10.104.138.29"; // replace with host's LAN IP

        void Start()
        {
            var transport = NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>();
            transport.ConnectionData.Address = ipAddress;
        }
    }

}
