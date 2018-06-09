using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class SimpleBot : Bot
{
    private List<Vector2> path;

    protected override void OnStart()
    {
        path = GetShortestPath(Map.Bounds.LeftDown, Map.Bounds.RightDown);
        if (!Left)
            path.Reverse();
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

    private List<Vector2> GetShortestPath(Vector2 start, Vector2 end)
    {
        var queue = new Queue<Vector2>();
        var visited = new HashSet<Vector2>();
        var previous = new Dictionary<Vector2, Vector2>();
        queue.Enqueue(start);
        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            if (node == end)
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
            foreach (var neighbour in GetNeighbours(node).Intersect(Map.RoadNodes).Except(visited))
            {
                previous[neighbour] = node;
                queue.Enqueue(neighbour);
            }
        }
        return null;
    }

    private IEnumerable<Vector2> GetNeighbours(Vector2 node)
    {
        yield return node + new Vector2(Map.TileSize.x, 0f);
        yield return node + new Vector2(-Map.TileSize.x, 0f);
        yield return node + new Vector2(0f, Map.TileSize.y);
        yield return node + new Vector2(0f, -Map.TileSize.y);
    }
}

