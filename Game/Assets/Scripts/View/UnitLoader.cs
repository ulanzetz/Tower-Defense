using UnityEngine;

/// <summary>
/// Скрипт создающий модели юнитов по эвенту
/// </summary>
class UnitLoader : MonoBehaviour
{
    public ViewCore ViewCore = null;
    public GameObject UnitPrefab = null;

    private void Start() =>
        ViewCore.Game.OnUnitAdd += unit =>
        {
            var unitViewer = Instantiate(UnitPrefab, transform).GetComponent<UnitViewer>();
            unitViewer.Prefab = UnitPrefab;
            unitViewer.Unit = unit;
            unitViewer.Direction = unit.Owner.Direction;
        };
}