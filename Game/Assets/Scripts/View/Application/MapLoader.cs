using UnityEngine;
using System.Linq;

/// <summary>
/// Загрузку тайлов карты
/// </summary>
class MapLoader : MonoBehaviour
{
    public ViewCore ViewCore = null;
    /// <summary>
    /// Префаб тайла, из которых будет собираться карта
    /// </summary>
    public GameObject TilePrefab = null;
    /// <summary>
    /// Спрайты травы
    /// </summary>
    public Sprite[] GrassSprites = null;
    /// <summary>
    /// Спрайт дороги
    /// </summary>
    public Sprite RoadSprite = null;

    private void Start()
    {
        var map = ViewCore.Game.Map;
        var min = GameConstants.MapBounds.Min;
        var max = GameConstants.MapBounds.Max;
        var size = GameConstants.TileSize;

        var xPoints = Utilities.Range(min.x, max.x, size.x).ToList();
        var yPoints = Utilities.Range(min.y, max.y, size.y).ToList();

        foreach(var x in xPoints)
            foreach(var y in yPoints)
            {
                var tile = Instantiate(TilePrefab, transform);
                var pos = new Vector2(x, y);
                tile.transform.position = pos;
                tile.GetComponent<SpriteRenderer>().sprite =
                    map.RoadNodes.Contains(pos) ?
                    RoadSprite :
                    GrassSprites[Random.Range(0, GrassSprites.Length)];
            }
    }
}