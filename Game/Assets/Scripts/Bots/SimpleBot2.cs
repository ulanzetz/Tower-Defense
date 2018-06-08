using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

class SimpleBot2
{
    private BotController controller;
    private Map map;

    public SimpleBot2(BotController controller)
    {
        this.controller = controller;
        map = controller.Map;
        var rnd = new System.Random();
        var towerPlaces = GetTowerPlaces(map.Left, map.RoadNodes);
        if (!controller.Left)
            towerPlaces = GetTowerPlaces(map.Right, map.RoadNodes);

        towerPlaces.Take(3).ForEach(n => controller.BuildTower(n.Key));

        controller.OnReachDestination += unitId =>
        {
            var unitPos = controller.GetUnitPositon(unitId);
            var possibleWays = GetDeltaForDFS(unitPos, controller.Left).Intersect(map.RoadNodes).ToList();
            var possibleWaysWithoutDown = possibleWays.Where(vec => vec != unitPos + new Vector2(0, -map.TileSize.y)).ToList();
            var needGoDown = NeedGoDown(unitPos, possibleWays);
            if (possibleWays.Count() == 2)
            {
                if (!needGoDown)
                {
                    var r = rnd.Next(possibleWaysWithoutDown.Count());
                    var dest = possibleWaysWithoutDown[r];
                    controller.MoveUnit(unitId, dest);
                }
                else
                {
                    var r = rnd.Next(possibleWays.Count());
                    var dest = possibleWays[r];
                    controller.MoveUnit(unitId, dest);
                }
            }
            else if (possibleWays.Count() == 3)
            {
                controller.MoveUnit(unitId, unitPos + new Vector2(map.TileSize.x, 0));
            }
            else if (possibleWays.Count() == 1)
                controller.MoveUnit(unitId, possibleWays[0]);
            else return;
        };

        while (controller.Gold >= GameConstants.UnitPrice)
        {
            controller.BuyUnitAndGetID();
        }
    }

    private bool NeedGoDown(Vector2 unitPos, List<Vector2> possibleWays)
    {
        if (possibleWays.Count() > 1)
            return false;
        if (possibleWays.First() == unitPos + new Vector2(0, -map.TileSize.y))
            return true;
        return false;
    }

    private Dictionary<Vector2, int> GetTowerPlaces(IEnumerable<Vector2> map, IEnumerable<Vector2> roadNodes)
    {
        var nodeUsefulness = new Dictionary<Vector2, int>();
        foreach (var node in map.Except(roadNodes))
        {
            nodeUsefulness.Add(node, GetAllVectorsForNodeInTowerRange(node).Intersect(roadNodes).Count());
        }
        return nodeUsefulness.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    private IEnumerable<Vector2> GetAllVectorsForNodeInTowerRange(Vector2 node)
    {
        for (var i = 1; i <= GameConstants.TowerRange; i++)
        {
            yield return node + new Vector2(i * map.TileSize.x, 0);
            yield return node + new Vector2(-map.TileSize.x * i, 0);
            yield return node + new Vector2(0, map.TileSize.y * i);
            yield return node + new Vector2(0, -map.TileSize.y * i);
            if (new Vector2(map.TileSize.x * i, map.TileSize.y * i).magnitude <= GameConstants.TowerRange)
            {
                yield return node + new Vector2(i * map.TileSize.x, i * map.TileSize.y);
                yield return node + new Vector2(-i * map.TileSize.x, i * map.TileSize.y);
                yield return node + new Vector2(i * map.TileSize.x, -i * map.TileSize.y);
                yield return node + new Vector2(-i * map.TileSize.x, -i * map.TileSize.y);
            }
        }
    }


    public IEnumerable<Vector2> GetDeltaForDFS(Vector2 node, bool left)
    {
        if (left)
            yield return node + new Vector2(map.TileSize.x, 0f);
        else
            yield return node + new Vector2(-map.TileSize.x, 0f);
        yield return node + new Vector2(0f, map.TileSize.y);
        yield return node + new Vector2(0f, -map.TileSize.y);

    }

    public IEnumerable<Vector2> GetNeighbours(Vector2 node)
    {
        yield return node + new Vector2(map.TileSize.x, 0f);
        yield return node + new Vector2(-map.TileSize.x, 0f);
        yield return node + new Vector2(0f, map.TileSize.y);
        yield return node + new Vector2(0f, -map.TileSize.y);
    }
}