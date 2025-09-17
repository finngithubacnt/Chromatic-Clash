using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void HostGame()
    {
        NetworkManager.Singleton.StartHost();
        SceneManager.LoadScene("Testing_Networking");
    }

    public void JoinGame()
    {
        NetworkManager.Singleton.StartClient();
        SceneManager.LoadScene("Testing_Networking");
    }
}
