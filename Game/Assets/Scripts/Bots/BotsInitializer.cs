using UnityEngine;

/// <summary>
/// Скрипт, инициализирующий контроллеры для ботов
/// </summary>
class BotsInitializer : MonoBehaviour
{
    public ViewCore ViewCore = null;
    public Bot LeftBot = null;
    public Bot RightBot = null;

    private void Start()
    {
        LeftBot.Controller = new BotController(ViewCore.Game.LeftPlayer);
        RightBot.Controller = new BotController(ViewCore.Game.RightPlayer);
    }
}