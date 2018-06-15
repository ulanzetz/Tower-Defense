using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map
{
    public readonly Bounds2D Bounds;
    public readonly Vector2 TileSize;
    public readonly HashSet<Vector2> RoadNodes;

    public IEnumerable<Vector2> Left =>
        Utilities.Range(Bounds.Min.x, Bounds.Middle.x, TileSize.x).
        SelectMany(
            x => Utilities.Range(Bounds.Min.y, Bounds.Max.y, TileSize.y).
            Select(y => new Vector2(x, y))
        );

    public IEnumerable<Vector2> Right =>
        Utilities.Range(Bounds.Middle.x, Bounds.Max.x, TileSize.x).
        SelectMany(
            x => Utilities.Range(Bounds.Min.y, Bounds.Max.y, TileSize.y).
            Select(y => new Vector2(x, y))
        );

    public IEnumerable<Vector2> All => Left.Concat(Right);

    private Map(Bounds2D bounds, Vector2 tileSize, HashSet<Vector2> roadNodes)
    {
        Bounds = bounds;
        TileSize = tileSize;
        RoadNodes = roadNodes;
    }

    /// <summary>
    /// Генерирует случайную карту
    /// </summary>
    /// <param name="middlePointsCount">Количество точек в центре карты</param>
    /// <returns>Сгенерированная карта</returns>
    public static Map GetRandomMap(int middlePointsCount = 1) =>
        GetRandomMap(GameConstants.GameBounds, GameConstants.TileSize, middlePointsCount);

    /// <summary>
    /// Генерирует случайную карты
    /// </summary>
    /// <param name="bounds">Размеры карты</param>
    /// <param name="tileSize">Размер тайла</param>
    /// <param name="middlePointsCount">Количество точек в центре карты</param>
    /// <returns>Сгенерированная карта</returns>
    public static Map GetRandomMap(Bounds2D bounds, Vector2 tileSize, int middlePointsCount)
    {
        var min = bounds.Min;
        var max = bounds.Max;
        var middle = bounds.Middle;

        var roadNodes = new HashSet<Vector2>();
        var openYPoints = Utilities.Range(min.y, max.y, tileSize.y).ToList();
        var rnd = new System.Random();
        var xToMiddle = Utilities.Range(min.x, middle.x, tileSize.x).ToList();

        for (int i = 0; i != middlePointsCount; ++i)
        {
            var middleY = openYPoints[rnd.Next(openYPoints.Count)];
            var yToMiddle = Utilities.Range(min.y, middleY, tileSize.y).ToList();
            var indexes = new[] { 0, 0 };
            while (true)
            {
                var current = new Vector2(xToMiddle[indexes[0]], yToMiddle[indexes[1]]);
                roadNodes.Add(current);
                if (current.x == middle.x)
                {
                    openYPoints.Remove(current.y);
                    break;
                }
                roadNodes.Add(new Vector2(middle.x - current.x, current.y));
                if (indexes[0] < xToMiddle.Count - 1)
                {
                    if (indexes[1] < yToMiddle.Count - 1)
                        ++indexes[rnd.Next(2)];
                    else
                        ++indexes[0];
                }
                else if (indexes[1] < yToMiddle.Count - 1)
                    ++indexes[1];
                else
                    break;
            }
        }
        return new Map(bounds, tileSize, roadNodes);
    }
}