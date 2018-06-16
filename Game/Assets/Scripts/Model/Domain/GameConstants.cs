using Vector2 = UnityEngine.Vector2;

public static class GameConstants
{
    /// <summary>
    /// Начальное золото игрока
    /// </summary>
    public const int StartGold = 100;
    /// <summary>
    /// Цена юнита
    /// </summary>
    public const int UnitPrice = 5;
    /// <summary>
    /// Цена башни
    /// </summary>
    public const int TowerPrice = 20;
    /// <summary>
    /// Здоровье башни
    /// </summary>
    public const int TowerHealth = 100;
    /// <summary>
    /// Здоровье юнита
    /// </summary>
    public const int UnitHealth = 50;
    /// <summary>
    /// Урон юнита
    /// </summary>
    public const int UnitDamage = 8;
    /// <summary>
    /// Урон банши
    /// </summary>
    public const int TowerDamage = 10;
    /// <summary>
    /// Задержка атаки юнита
    /// </summary>
    public const int UnitTurnsToAttack = 100;
    /// <summary>
    /// Задержка атаки башни
    /// </summary>
    public const int TowerTurnsToAttack = 100;
    /// <summary>
    /// Дальность атаки юнита
    /// </summary>
    public const float UnitRange = 21f;
    /// <summary>
    /// Дальность атаки башни
    /// </summary>
    public const float TowerRange = 170f;
    /// <summary>
    /// Скорость движения юнита
    /// </summary>
    public const float UnitSpeed = 0.5f;
    /// <summary>
    /// Радиус зоны, в которой может быть только 1 юнит
    /// </summary>
    public const float UnitAreaRange = 20f;
    /// <summary>
    /// Границы прогружаемой карты
    /// </summary>
    public static readonly Bounds2D MapBounds = new Bounds2D(new Vector2(-660, -780), new Vector2(660, 240));
    /// <summary>
    /// Границы игровой карты
    /// </summary>
    public static readonly Bounds2D GameBounds = new Bounds2D(new Vector2(-660, -660), new Vector2(660, 60));
    /// <summary>
    /// Размер одного тайла
    /// </summary>
    public static Vector2 TileSize = new Vector2(60f, 60f);
}