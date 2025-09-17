using Unity.Netcode;
using UnityEngine;

public class NetworkPersist : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject); // keep this object between scenes
    }
}
