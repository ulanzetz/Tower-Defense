using NUnit.Framework;
using System;
using System.Linq;
using UnityEngine;

[TestFixture]
public class ModelTests
{
    private static Game game;
    private static Player leftPlayer => game.LeftPlayer;
    private static Player rightPlayer => game.RightPlayer;

    private Vector2 GetRandomVectorOutTheRoad(Bounds2D bounds)
    {
        var rnd = new System.Random();
        var vec = new Vector2(
            rnd.Next((int)bounds.Min.x,
                     (int)bounds.Max.x),
            rnd.Next((int)bounds.Min.y,
                     (int)bounds.Max.y));
        while (game.Map.RoadNodes.Contains(vec))
            vec = new Vector2(
                rnd.Next((int)bounds.Min.x,
                            (int)bounds.Max.x),
                rnd.Next((int)bounds.Min.y,
                            (int)bounds.Max.y));
        return vec;
    }

    [SetUp]
    public void SetupGame() => game = new Game();

    [Test]
    public void MapIsSymmetric()
    {
        var map = game.Map;
        foreach (var rodeNode in map.RoadNodes)
            Assert.IsTrue(map.RoadNodes.Contains(new Vector2(-rodeNode.x, rodeNode.y)));
    }

    [Test]
    public void PlayerCanBuyUnit()
    {
        var prevPlayerGold = game.LeftPlayer.Gold;
        game.LeftPlayer.BuyUnit();
        Assert.AreEqual(prevPlayerGold - GameConstants.UnitPrice, game.LeftPlayer.Gold);
        Assert.AreEqual(game.Units.Count(), 1);
        Assert.AreEqual(game.LeftPlayer.Units.Count, 1);
    }

    [Test]
    public void PlayerBuyAndOwnUnit()
    {
        game.LeftPlayer.BuyUnit();
        var unit = game.Units.First();
        Assert.AreSame(game.LeftPlayer, unit.Owner);
        Assert.NotNull(game.LeftPlayer.Units.First().Owner);
        Assert.AreSame(game.LeftPlayer, game.LeftPlayer.Units.First().Owner);
    }

    [Test]
    public void UnitsSpawnsInCorrectPlace()
    {
        game.LeftPlayer.BuyUnit();
        game.RightPlayer.BuyUnit();
        var leftUnit = game.LeftPlayer.Units.First();
        var rightUnit = game.RightPlayer.Units.First();
        Assert.AreEqual(game.Map.Bounds.LeftDown, leftUnit.Position); //почему именно LeftDown только?
        Assert.AreEqual(game.Map.Bounds.RightDown, rightUnit.Position);
    }

    [Test]
    public void PlayerCanBuyTowers()
    {
        var randomPlace = GetRandomVectorOutTheRoad(leftPlayer.Area);
        var prevPlayerMoney = leftPlayer.Gold;
        game.LeftPlayer.BuildTower(randomPlace);
        Assert.AreEqual(prevPlayerMoney - GameConstants.TowerPrice, leftPlayer.Gold);
    }

    [Test]
    public void PlayerCanNotBuildTowersOnRoads()
    {
        Assert.Throws<InvalidOperationException>(() => leftPlayer.BuildTower(game.Map.RoadNodes.First()));
    }

    [Test]
    public void PlayersBothBuyAndOwnUnits()
    {
        leftPlayer.BuyUnit();
        var leftUnit = game.Units.First();
        rightPlayer.BuyUnit();
        var rightUnit = game.Units.Where(x => !x.Equals(leftUnit)).First();
        leftPlayer.BuyUnit();
        var leftUnit2 = game.Units.Where(x => !x.Equals(leftUnit) && !x.Equals(rightUnit)).First();
        Assert.AreEqual(3, game.Units.Count());
        Assert.AreSame(leftUnit.Owner, leftUnit2.Owner);
        Assert.AreNotSame(leftUnit.Owner, rightUnit.Owner);
        Assert.AreNotSame(leftUnit2.Owner, rightUnit.Owner);
        Assert.AreEqual(2, leftPlayer.Units.Count());
        Assert.AreEqual(1, rightPlayer.Units.Count());
    }

    [Test]
    public void UnitCanMoveOnRoad()
    {
        leftPlayer.BuyUnit();
        var leftUnit = game.Units.First();
        var unitSpawnPlace = game.Map.RoadNodes.First();
        var destination = game.Map.RoadNodes
            .Where(x => ((x - unitSpawnPlace).normalized == Vector2.up ||
                         (x - unitSpawnPlace).normalized == Vector2.left ||
                         (x - unitSpawnPlace).normalized == Vector2.down ||
                         (x - unitSpawnPlace).normalized == Vector2.right))
            .First();
        Assert.DoesNotThrow(() => leftUnit.Move(destination));
    }

    [Test]
    public void UnitCanNotMoveWhereHeStands()
    {
        leftPlayer.BuyUnit();
        var leftUnit = game.Units.First();
        Assert.Throws<ArgumentException>(() => leftUnit.Move(leftUnit.Position));
    }

    [Test]
    public void PlayerCanNotPlaceTowerOnOpponentArea()
    {
        var randOpponentVector = GetRandomVectorOutTheRoad(rightPlayer.Area);
        Assert.Throws<InvalidOperationException>(() => leftPlayer.BuildTower(randOpponentVector));
    }

    [Test]
    public void UnitDecreaseHPManually()
    {
        leftPlayer.BuyUnit();
        var unit = game.Units.First();
    }

    [Test]
    public void PlayersBothBuyAndOwnTowers()
    {
        var rndPlaceLeft1 = GetRandomVectorOutTheRoad(leftPlayer.Area);
        var rndPlaceLeft2 = GetRandomVectorOutTheRoad(leftPlayer.Area);
        var rndPlaceRight = GetRandomVectorOutTheRoad(rightPlayer.Area);

        Assert.DoesNotThrow(() => leftPlayer.BuildTower(rndPlaceLeft1));
        var leftTower1 = leftPlayer.Towers.First();
        Assert.DoesNotThrow(() => rightPlayer.BuildTower(rndPlaceRight));
        var rightTower = rightPlayer.Towers.First();
        Assert.DoesNotThrow(() => leftPlayer.BuildTower(rndPlaceLeft2));
        var leftTower2 = leftPlayer.Towers.Where(x => !x.Equals(leftTower1)).First();

        Assert.AreSame(leftTower1.Owner, leftTower2.Owner);
        Assert.AreNotSame(leftTower1.Owner, rightTower.Owner);
    }

    [Test]
    public void UnitsHPDecreasedByAttacks()
    {
        leftPlayer.BuyUnit();
        rightPlayer.BuyUnit();
        var leftUnit = leftPlayer.Units.First();
        var rightUnit = rightPlayer.Units.First();
        leftUnit.Position = rightUnit.Position + Vector2.left;
        game.MakeTurn();
        Assert.AreEqual(GameConstants.UnitHealth - GameConstants.UnitDamage, leftUnit.HP);
        Assert.AreEqual(GameConstants.UnitHealth - GameConstants.UnitDamage, rightUnit.HP);
    }

    [Test]
    public void UnitsAttackNearestUnit()
    {
        leftPlayer.BuyUnit();
        rightPlayer.BuyUnit();
        var leftUnit1 = leftPlayer.Units.First();
        var rightUnit = rightPlayer.Units.First();
        leftPlayer.BuyUnit();
        var leftUnit2 = leftPlayer.Units.Where(x => !x.Equals(leftUnit1)).First();
        leftUnit1.Position = rightUnit.Position + Vector2.left;
        leftUnit2.Position = rightUnit.Position + 2 * Vector2.left;
        game.MakeTurn();
        Assert.AreEqual(GameConstants.UnitHealth, leftUnit2.HP);
        Assert.AreEqual(GameConstants.UnitHealth - GameConstants.UnitDamage, leftUnit1.HP);
        Assert.AreEqual(GameConstants.UnitHealth - 2 * GameConstants.UnitDamage, rightUnit.HP);
    }

    [Test]
    public void UnitsCanNotMoveDiagonally()
    {
        leftPlayer.BuyUnit();
        var unit = leftPlayer.Units.First();
        Assert.Throws<ArgumentException>(
            () => unit.Move(unit.Position + (Vector2.up + Vector2.right)));
    }
}