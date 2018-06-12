using UnityEngine;

class GameBehaviour : MonoBehaviour
{
    public int MiddlePointCount = 4;
    public Game Game;
    private void Start() => Game = new Game(MiddlePointCount);
}