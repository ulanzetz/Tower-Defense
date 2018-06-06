using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class SimpleBot
{
    private BotController controller;
    private Map map;

    public SimpleBot(BotController controller)
    {
        this.controller = controller;
        map = controller.Map;
        var path = GetShortestPath(map.Bounds.LeftDown, map.Bounds.RightDown);
        if(!controller.Left)
            path.Reverse();
        controller.OnReachDestination += unitId =>
        {
            var unitPos = controller.GetUnitPositon(unitId);
            if (unitPos == path[path.Count - 1])
                return;
            var dest = path[path.IndexOf(unitPos) + 1];
            controller.MoveUnit(unitId, dest);
        };
        while (controller.Gold >= GameConstants.UnitPrice)
            controller.BuyUnitAndGetID();
    }

    public List<Vector2> GetShortestPath(Vector2 start, Vector2 end)
    {
        var queue = new Queue<Vector2>();
        var visited = new HashSet<Vector2>();
        var previous = new Dictionary<Vector2, Vector2>();
        queue.Enqueue(start);
        while(queue.Count > 0)
        {
            var node = queue.Dequeue();
            if(node == end)
            {
                var path = new Stack<Vector2>();
                var current = end;
                path.Push(current);
                while (current != start)
                {
                    path.Push(previous[current]);
                    current = previous[current];
                }
                return new List<Vector2>(path);
            }
            visited.Add(node);
            foreach (var neighbour in GetNeighbours(node).Intersect(map.RoadNodes).Except(visited))
            {
                previous[neighbour] = node;
                queue.Enqueue(neighbour);
            }
        }
        return null;
    }

    public IEnumerable<Vector2> GetNeighbours(Vector2 node)
    {
        yield return node + new Vector2(map.TileSize.x, 0f);
        yield return node + new Vector2(-map.TileSize.x, 0f);
        yield return node + new Vector2(0f, map.TileSize.y);
        yield return node + new Vector2(0f, -map.TileSize.y);
    }
}