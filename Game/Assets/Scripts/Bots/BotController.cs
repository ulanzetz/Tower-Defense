using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Контроллер, связывающий бота с игрой
/// </summary>
class BotController
{
    private Player player;
    private Dictionary<int, Unit> units = new Dictionary<int, Unit>();
    private int unitCount = 0;

    public delegate void UnitIDHandler(int unitID);
    public event UnitIDHandler OnReachDestination;

    public BotController(Player player)
    {
        this.player = player;
    }

    public int BuyUnitAndGetID()
    {
        var unit = player.BuyUnit();
        var id = ++unitCount;
        units[id] = unit;
        unit.OnReachDestination += () => OnReachDestination(id);
        OnReachDestination(id);
        return unitCount;
    }

    public void BuildTower(Vector2 position) => player.BuildTower(position);
    public void MoveUnit(int unitID, Vector2 destination) => units[unitID].Move(destination);
    public Vector2 GetUnitPositon(int unitID) => units[unitID].Position;
    public Map Map => player.Game.Map;
    public bool Left => player == player.Game.LeftPlayer;
    public int Gold => player.Gold;
}