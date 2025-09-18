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
}
