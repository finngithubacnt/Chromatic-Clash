
using Unity.Netcode;
using UnityEngine;

public class PlayerCamera : NetworkBehaviour
{
    void Start()
    {
        if (!IsOwner)
        {
            // disable cameras for players that aren’t yours
            GetComponentInChildren<Camera>().enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
        }
    }
}
