using Unity.Netcode;
using UnityEngine;

public class PlayerSpawn : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            // Only run on the machine that owns this player
            if (IsHost)
            {
                Transform hostSpawn = GameObject.Find("HostSpawn").transform;
                transform.position = hostSpawn.position;
                transform.rotation = hostSpawn.rotation;
            }
            else
            {
                Transform clientSpawn = GameObject.Find("ClientSpawn").transform;
                transform.position = clientSpawn.position;
                transform.rotation = clientSpawn.rotation;
            }
        }
    }
}
