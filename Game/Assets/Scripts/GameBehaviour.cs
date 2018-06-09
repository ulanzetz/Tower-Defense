using UnityEngine;

class GameBehaviour : MonoBehaviour
{
    public Game Game;
    private void Start() => Game = new Game();
}