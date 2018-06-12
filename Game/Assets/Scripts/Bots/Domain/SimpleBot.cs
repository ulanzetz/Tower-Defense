using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class SimpleBot : Bot
{
    private List<Vector2> path;

    protected override void OnStart()
    {
        path = Algorithms.GetShortestPath(
            Map.Bounds.LeftDown, 
            Map.Bounds.RightDown, 
            n => Algorithms.GetRoadNeighbours(n, Map)
        );
        if (!Left)
            path.Reverse();
        BuildTowersOnRandomNodes(3);
        while (Gold >= GameConstants.UnitPrice)
            BuyUnitAndGetID();
    }

    protected override void OnUnitReachDestination(int unitID)
    {
        var unitPos = GetUnitPosition(unitID);
        if (unitPos == path[path.Count - 1])
            return;
        var dest = path[path.IndexOf(unitPos) + 1];
        MoveUnit(unitID, dest);
    }

    private void BuildTowersOnRandomNodes(int count)
    {
        var side = Left ? Map.Left : Map.Right;
        var rng = new System.Random();
        var emptyPositions = side.Except(Map.RoadNodes).ToList();
        for(int i = 0; i != count; ++i)
        {
            var number = rng.Next(emptyPositions.Count);
            BuildTower(emptyPositions[number]);
            emptyPositions.RemoveAt(number);
        }
    }
}

