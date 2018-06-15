using UnityEngine;
using UnityEngine.UI;

class GameOverViewer : MonoBehaviour
{
    public ViewCore ViewCore = null;
    private void Start()
    {
        var text = GetComponentInChildren<Text>();
        text.gameObject.SetActive(false);
        ViewCore.Game.OnPlayerWin += (player) =>
        {
            text.gameObject.SetActive(true);
            text.text = $"Game Over\nWinner: {player.Name}";
        };
    }
}