using System;
using System.Linq;
using Vector2 = UnityEngine.Vector2;

/// <summary>
/// Основной класс для всех активных объектов в игре
/// </summary>
public abstract class ActiveObject
{
    public Vector2 Position
    {
        get { return position; }
        set
        {
            position = value;
            OnMove?.Invoke();
        }
    }

    public readonly Player Owner;

    public delegate void VoidHandler();
    public delegate void EnemyHandler(ActiveObject enemy);

    public event VoidHandler OnDeath;
    public event VoidHandler OnMove;
    public event VoidHandler OnHPChanged;
    public event EnemyHandler OnAttack;

    public int HP
    {
        get
        {
            return hp;
        }
        
        private set
        {
            OnHPChanged?.Invoke();
            hp = value;
        }
    }
    protected Game Game => Owner.Game;

    private readonly int turnsToAttack;
    private readonly int damage;
    private readonly float range;

    private int turnCount = 0;
    private int hp;
    protected Vector2 position;
    protected bool fighting;

    protected ActiveObject(
        Vector2 pos, 
        Player owner, 
        int health, 
        int turnsToAttack,
        float range,
        int damage)
    {
        Position = pos;
        Owner = owner;
        HP = health;
        this.turnsToAttack = turnsToAttack;
        this.range = range;
        this.damage = damage;
    }

    public virtual void OnTurn()
    {
        if(!fighting)
        {
            Attack();
            return;
        }

        if(++turnCount == turnsToAttack)
        {
            Attack();
            turnCount = 0;
        }
    }

    protected virtual void Attack()
    {
        var enemy = Owner.Enemies.
            Select(e => Tuple.Create((e.Position - Position).magnitude, e)).
            Where(e => e.Item1 <= range).
            OrderBy(e => e.Item1).
            Select(e => e.Item2).
            FirstOrDefault();

        if (enemy != null)
        {
            enemy.DealDamage(damage);
            if(HP >= 0)
                OnAttack?.Invoke(enemy);
            fighting = true;
        }
        else
            fighting = false;
    }

    private void DealDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Owner.DeleteOwnership(this);
            OnDeath?.Invoke();
        }
    }
}