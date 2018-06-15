using UnityEngine;

/// <summary>
/// Скрипт, отображающий анимации и передвигающий юнита
/// </summary>
class UnitViewer : MonoBehaviour
{ 
    public Unit Unit
    {
        set
        {
            unit = value;
            transform.position = new Vector3(unit.Position.x, unit.Position.y, transform.position.z);

            unit.OnAttack += (enemy) =>
            {
                if (animState == AnimState.Death)
                    return;
                animState = AnimState.Attack;
                Direction = (enemy.Position.x - unit.Position.x);
            };
            unit.OnMove += () =>
            {
                if (animState == AnimState.Death)
                    return;
                animState = AnimState.Walk;
                Direction = (unit.Position.x - transform.position.x);
            };
            unit.OnDeath += () => animState = AnimState.Death;
            unit.OnIdle += () =>
            {
                if (animState == AnimState.Death)
                    return;
                animState = AnimState.Idle;
            };
        }
    }

    public float Direction
    {
        set
        {
            var newXScale = value == 0 ? unit.Owner.Direction : Mathf.Sign(value);
            transform.localScale = new Vector3(newXScale * cachedScale.x, cachedScale.y, cachedScale.z);
        }
    }

    public GameObject Prefab
    {
        set
        {
            var prefTransform = value.transform;
            cachedScale = prefTransform.localScale;
            transform.position = prefTransform.position;
        }
    }

    private enum AnimState
    {
        Idle,
        Walk,
        Attack,
        Death
    }

    private AnimState animState = AnimState.Idle;
    private Animator anim;
    private Vector3 cachedScale;
    private Unit unit;

    private void Start() => anim = GetComponent<Animator>();

    private void Update()
    {
        anim.Play(animState.ToString());
        transform.position = new Vector3(unit.Position.x, unit.Position.y, transform.position.z);
    }

    private void EndDeath() => Destroy(gameObject);
}