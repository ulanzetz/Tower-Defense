using UnityEngine;

/// <summary>
/// Скрипт, инициализирующий контроллеры для ботов
/// </summary>
class BotsInitializer : MonoBehaviour
{
    public ViewCore ViewCore;
    public Bot LeftBot;
    public Bot RightBot;

    private void Start()
    {
        LeftBot.Controller = new BotController(ViewCore.Game.LeftPlayer);
        RightBot.Controller = new BotController(ViewCore.Game.RightPlayer);
    }
}