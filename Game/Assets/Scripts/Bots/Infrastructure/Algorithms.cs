using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Algorithms
{
    public static List<Vector2> GetShortestPath(
        Vector2 start, Vector2 end, Func<Vector2, IEnumerable<Vector2>> neighbours)
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
            foreach (var neighbour in neighbours(node).Except(visited))
            {
                previous[neighbour] = node;
                queue.Enqueue(neighbour);
            }
        }
        return null;
    }

    public static IEnumerable<Vector2> GetRoadNeighbours(Vector2 node, Map map)
    {
        return new[] {node + new Vector2(map.TileSize.x, 0f),
        node + new Vector2(-map.TileSize.x, 0f),
        node + new Vector2(0f, map.TileSize.y),
        node + new Vector2(0f, -map.TileSize.y) }.Intersect(map.RoadNodes);
    }
}