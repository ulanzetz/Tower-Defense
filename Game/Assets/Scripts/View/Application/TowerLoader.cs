using UnityEngine;

/// <summary>
/// Загрузку башен
/// </summary>
class TowerLoader : MonoBehaviour
{
    public ViewCore ViewCore = null;
    public GameObject TowerPrefab = null;

    private void Start()
    {
        var prefabTransform = TowerPrefab.transform;
        var game = ViewCore.Game;
        game.OnTowerAdd += tower =>
        {
            var towerObject = Instantiate(TowerPrefab, transform);
            towerObject.transform.position = new Vector3(tower.Position.x, tower.Position.y, prefabTransform.position.z);
            towerObject.GetComponent<SpriteRenderer>().color = tower.Owner == game.LeftPlayer ? Color.green : Color.red;
            tower.OnDeath += () => Destroy(towerObject);
        };
    }
}