using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Основной скрипт, вопроизводящий шаги игры
/// </summary>
class ViewCore : MonoBehaviour
{
    public short FramesToTurn = 1;
    public GameBehaviour GameBehaviour = null;
 
    public Game Game => GameBehaviour.Game;

    private short framesCount = 0;

    private void Start()
    {
        Game.OnPlayerWin += player =>
        {
            var gameOver = transform.Find("GameOver").gameObject;
            gameOver.SetActive(true);
            gameOver.GetComponentInChildren<Text>().text = $"Game Over.\nWinner: {player.Name}";
        };
    }

    private void Update()
    {
        if(++framesCount == FramesToTurn)
        {
            framesCount = 0;
            Game.MakeTurn();
        }
    }
}