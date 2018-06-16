using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class SmartBot : Bot
{
    private List<Vector2>[] paths = new List<Vector2>[2];
    private Dictionary<int, int> unitPathNumber = new Dictionary<int, int>();
    private System.Random rng = new System.Random();

    protected override void OnStart()
    {
        paths[0] = Algorithms.GetShortestPath(
            Map.Bounds.LeftDown,
            Map.Bounds.RightDown,
            n => Algorithms.GetRoadNeighbours(n, Map)
        );

        if (!Left)
            paths[0].Reverse();

        var bothCanHave = paths[0].Take(5).Concat(paths[0].Skip(Math.Max(0, paths[0].Count - 5)).Take(5));

        paths[1] = Algorithms.GetShortestPath(
            Map.Bounds.LeftDown,
            Map.Bounds.RightDown,
            n => Algorithms.GetRoadNeighbours(n, Map).Except(paths[0].Except(bothCanHave))
        );

        if (paths[1] == null)
            paths[1] = paths[0];
        else if (!Left)
            paths[1].Reverse();

        BuildTowersInBestPlaces(3);
        while (Gold >= GameConstants.UnitPrice)
            BuyUnitAndGetID();
    }

    protected override void OnUnitReachDestination(int unitID)
    {
        var unitPos = GetUnitPosition(unitID);
        if (!unitPathNumber.ContainsKey(unitID))
            unitPathNumber[unitID] = rng.Next(paths.Length);
        var path = paths[unitPathNumber[unitID]];
        if (unitPos == path[path.Count - 1])
            return;
        var dest = path[path.IndexOf(unitPos) + 1];
        MoveUnit(unitID, dest);
    }

    private void BuildTowersInBestPlaces(int count) => 
        GetEmptyNodesWithBestRoadIntersection(count).ForEach(p => BuildTower(p));

    private IEnumerable<Vector2> GetEmptyNodesWithBestRoadIntersection(int count)
    {
        var side = Left ? Map.Left : Map.Right;
        var emptyPositsion = side.Except(Map.RoadNodes);
        return emptyPositsion.
            OrderByDescending(e => Map.RoadNodes.Where(r => (e - r).magnitude <= GameConstants.TowerRange).Count()).
            Take(count);
    }
}