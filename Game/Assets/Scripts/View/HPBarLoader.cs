using UnityEngine;

/// <summary>
/// Скрипт, загружающий HPBar'ы для каждого ActiveObject
/// </summary>
class HPBarLoader : MonoBehaviour
{
    public ViewCore ViewCore;
    public GameObject TowerBar = null;
    public GameObject UnitBar = null;

    private void Start()
    {
        ViewCore.Game.Towers.ForEach(t => AddHPBar(t, TowerBar));
        ViewCore.Game.OnUnitAdd += (u => AddHPBar(u, UnitBar));
    }

    private void AddHPBar(ActiveObject user, GameObject bar)
    {
        var hpBar = Instantiate(bar, transform);
        hpBar.transform.position =
            new Vector3(user.Position.x, user.Position.y, 0f) +
            bar.transform.position;
        hpBar.GetComponent<HPViewer>().User = user;
    }
}