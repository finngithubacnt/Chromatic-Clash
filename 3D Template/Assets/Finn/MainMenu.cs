using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void HostGame()
    {
        SceneManager.LoadScene("Testing Networking");
        NetworkManager.Singleton.StartHost();
        
    }

    public void JoinGame()
    {
        SceneManager.LoadScene("Testing Networking");
        NetworkManager.Singleton.StartClient();
        
    }
}
