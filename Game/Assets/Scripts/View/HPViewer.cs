using UnityEngine;

/// <summary>
/// Скрипт, обновляющий значение HP в TextMesh'e
/// </summary>
class HPViewer : MonoBehaviour
{
    public ActiveObject User
    {
        set
        {
            var maxHp = value is Tower ? GameConstants.TowerHealth : GameConstants.UnitHealth;
            textMesh.text = $"{value.HP}/{maxHp}";
            value.OnHPChanged += () => textMesh.text = $"{value.HP}/{maxHp}";
            value.OnDeath += () => Destroy(gameObject);
            value.OnMove += () => transform.position = new Vector3(value.Position.x, value.Position.y, 0f) + viewDelta;
        }
    }

    private TextMesh textMesh;
    private Vector3 viewDelta;

    private void Awake()
    {
        textMesh = GetComponent<TextMesh>();
        viewDelta = transform.position;
    }
}