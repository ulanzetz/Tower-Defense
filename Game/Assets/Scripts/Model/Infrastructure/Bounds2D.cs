using UnityEngine;

/// <summary>
/// Ограниченная область на плоскости
/// </summary>
public class Bounds2D
{
    public readonly Vector2 Min;
    public readonly Vector2 Max;

    public Bounds2D(Vector2 min, Vector2 max)
    {
        Min = min;
        Max = max;
    }

    /// <summary>
    /// Минимальный и макисимальный X
    /// </summary>
    public Vector2 X => new Vector2(Min.x, Max.x);

    /// <summary>
    /// Минимальный и максимальный Y
    /// </summary>
    public Vector2 Y => new Vector2(Min.y, Max.y);

    /// <summary>
    /// Центр области
    /// </summary>
    public Vector2 Middle => (Min + Max) * 0.5f;

    public bool Contains(Vector2 pos) => 
        pos.x <= Max.x && pos.x >= Min.x && pos.y <= Max.y && pos.y >= Min.y;

    public Bounds2D Left => new Bounds2D(Min, new Vector2(Middle.x, Max.y));

    public Bounds2D Right => new Bounds2D(new Vector2(Middle.x, Min.y), Max);

    public Vector2 LeftDown => Min;

    public Vector2 RightDown => new Vector2(Max.x, Min.y);
 }