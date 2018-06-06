using System.Collections.Generic;
using UnityEngine;

class BotController
{
    private Player player;
    private Dictionary<int, Unit> units = new Dictionary<int, Unit>();
    private int unitCount = 0;

    public delegate void UnitIDHandler(int unitId);
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
    public void MoveUnit(int unitId, Vector2 destination) => units[unitId].Move(destination);
    public Vector2 GetUnitPositon(int unitId) => units[unitId].Position;
    public Map Map => player.Game.Map;
    public bool Left => player == player.Game.LeftPlayer;
    public int Gold => player.Gold;
}