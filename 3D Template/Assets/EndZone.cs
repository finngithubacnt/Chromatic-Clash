using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class EndZone : NetworkBehaviour
{
    public GameObject endUIPrefab; // UI prefab with winner text + buttons
    private static bool gameEnded = false;

    void OnTriggerEnter(Collider other)
    {
        if (!IsServer || gameEnded) return;

        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            // Decide winner by ownership
            string winner = player.IsOwner && IsHost ? "Red (Host)" : "Blue (Client)";

            gameEnded = true;

            // Call all clients to show popup
            ShowEndPopupClientRpc(winner);
        }
    }

    [ClientRpc]
    private void ShowEndPopupClientRpc(string winner)
    {
        // Spawn UI on each client
        GameObject popup = Instantiate(endUIPrefab);
        EndUI endUI = popup.GetComponent<EndUI>();
        endUI.Setup(winner);
    }

    [ServerRpc(RequireOwnership = false)]
    public void RestartGameServerRpc()
    {
        gameEnded = false;
        // Load the host scene for everyone (replace "HostScene" with your scene name)
        NetworkManager.SceneManager.LoadScene("HostScene", LoadSceneMode.Single);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ContinueGameServerRpc()
    {
        gameEnded = false;
        // Just remove the UI, game continues
        HideEndPopupClientRpc();
    }

    [ClientRpc]
    private void HideEndPopupClientRpc()
    {
        EndUI ui = FindObjectOfType<EndUI>();
        if (ui != null) Destroy(ui.gameObject);
    }
}
