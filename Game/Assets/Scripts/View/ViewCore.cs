using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Основной скрипт, вопроизводящий шаги игры
/// </summary>
class ViewCore : MonoBehaviour
{
    public short FramesToTurn = 1;

    public static Game Game { get; private set; }

    private short framesCount = 0;

    private void Start()
    {
        Game = new Game();
        Game.OnPlayerWin += player =>
        {
            var gameOver = transform.Find("GameOver").gameObject;
            gameOver.SetActive(true);
            gameOver.GetComponentInChildren<Text>().text = $"Game Over.\nWinner: {player.Name}";
        };
        StartCoroutine(LoadBots());
    }

    private void Update()
    {
        if(++framesCount == FramesToTurn)
        {
            framesCount = 0;
            Game.MakeTurn();
        }
    }

    private IEnumerator LoadBots()
    {
        yield return new WaitForSeconds(1f);
        var leftPlayerController = new BotController(Game.LeftPlayer);
        var leftBot = new SimpleBot(leftPlayerController);
        var rightPlayerController = new BotController(Game.RightPlayer);
        var rightBot = new SimpleBot(rightPlayerController);
    }
}