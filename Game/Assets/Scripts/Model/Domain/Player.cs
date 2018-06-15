using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Класс игрока
/// </summary>
public class Player
{
    public HashSet<Tower> Towers = new HashSet<Tower>();
    public HashSet<Unit> Units = new HashSet<Unit>();

    public int Gold { get; private set; }

    public readonly Game Game;
    private readonly bool left;

    public Player(Game game, int gold, bool left)
    {
        Game = game;
        Gold = gold;
        this.left = left;
    }

    public IEnumerable<ActiveObject> Ownership => Towers.Cast<ActiveObject>().Concat(Units);
    public IEnumerable<ActiveObject> Enemies => Game.GetEnemies(this);
    public float Direction => left ? 1 : -1;
    public Bounds2D Area => left ? GameConstants.GameBounds.Left : GameConstants.GameBounds.Right;
    public Vector2 UnitRespawn => left ? Area.Min : new Vector2(Area.Max.x, Area.Min.y);
    
    public Tower BuildTower(Vector2 pos)
    {
        if (Gold < GameConstants.TowerPrice)
            throw new InvalidOperationException("You havn't enoght money");
        if (!Area.Contains(pos))
            throw new InvalidOperationException("You can't build tower in that position");
        if (Game.Map.RoadNodes.Contains(pos))
            throw new InvalidOperationException("You can't build tower on road node");
        Gold -= GameConstants.TowerPrice;
        var tower = new Tower(pos, this);
        Towers.Add(tower);
        Game.AddTower(tower);
        return tower;
    }

    public Unit BuyUnit()
    {
        if (Gold < GameConstants.UnitPrice)
            throw new InvalidOperationException("You havn't enought money");
        Gold -= GameConstants.UnitPrice;
        var unit = new Unit(UnitRespawn, this, GameConstants.UnitSpeed);
        Units.Add(unit);
        Game.AddUnit(unit);
        return unit;
    }

    public void DeleteOwnership(ActiveObject obj)
    {
        if (obj is Tower) Towers.Remove(obj as Tower);
        else if (obj is Unit) Units.Remove(obj as Unit);
        else
            throw new ArgumentException("Incorrent object to delete");
    }

    public string Name => left ? "Left" : "Right";
}