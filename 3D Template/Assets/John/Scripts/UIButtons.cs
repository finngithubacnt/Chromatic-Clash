using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    public int scene;
    public void play()
    {
        SceneManager.LoadScene(scene);
    }
    public void OnApplicationQuit()
    {
        Application.Quit();
    }
}