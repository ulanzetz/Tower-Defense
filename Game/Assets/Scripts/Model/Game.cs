using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Основной класс игры
/// </summary>
public class Game
{
    public readonly Map Map;
    public readonly Player LeftPlayer;
    public readonly Player RightPlayer;

    public delegate void UnitHandler(Unit unit);
    public delegate void TowerHandler(Tower tower);
    public delegate void PlayerHandler(Player player);

    public event UnitHandler OnUnitAdd;
    public event TowerHandler OnTowerAdd;
    public event PlayerHandler OnPlayerWin;

    public Game(int mapMiddlePoints = 2)
    {
        Map = Map.GetRandomMap(mapMiddlePoints);
        LeftPlayer = new Player(this, GameConstants.StartGold, true);
        RightPlayer = new Player(this, GameConstants.StartGold, false);
    }

    public Player GetEnemy(Player player) => player == LeftPlayer ? RightPlayer : LeftPlayer;

    public IEnumerable<ActiveObject> GetEnemies(Player player) => GetEnemy(player).Ownership;
    
    public IEnumerable<Tower> Towers => LeftPlayer.Towers.Concat(RightPlayer.Towers);

    public IEnumerable<Unit> Units => LeftPlayer.Units.Concat(RightPlayer.Units);

    public IEnumerable<ActiveObject> ActiveObjects => LeftPlayer.Ownership.Concat(RightPlayer.Ownership);

    public void MakeTurn() => ActiveObjects.ToList().ForEach(o => o.OnTurn());

    public void AddUnit(Unit unit) => OnUnitAdd?.Invoke(unit);

    public void AddTower(Tower tower) => OnTowerAdd?.Invoke(tower);

    public void EndGame(Player winner) => OnPlayerWin?.Invoke(winner);
}