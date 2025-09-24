using UnityEngine;
using UnityEngine.UI;

public class EndUI : MonoBehaviour
{
    public Text winnerText;
    public Button restartButton;
    public Button continueButton;

    private EndZone endZone;

    public void Setup(string winner)
    {
        winnerText.text = $"{winner} Wins!";
        endZone = FindObjectOfType<EndZone>();

        restartButton.onClick.AddListener(() =>
        {
            endZone.RestartGameServerRpc();
        });

        continueButton.onClick.AddListener(() =>
        {
            endZone.ContinueGameServerRpc();
        });
    }
}
