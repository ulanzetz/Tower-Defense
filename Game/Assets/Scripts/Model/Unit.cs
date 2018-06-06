using System;
using System.Linq;
using UnityEngine;

public class Unit : ActiveObject
{
    public event VoidHandler OnReachDestination;
    public event VoidHandler OnIdle;

    private float speed;
    private Vector2 moveDelta;
    private Vector2 destination;
    private bool moving;

    public Unit(Vector2 pos, Player owner, float speed) : 
        base(
            pos, 
            owner, 
            GameConstants.UnitHealth,
            GameConstants.UnitTurnsToAttack, 
            GameConstants.UnitRange,
            GameConstants.UnitDamage
        )
    {
        this.speed = speed;
    }

    public void Move(Vector2 destination)
    {
        if (moving || fighting)
            return;
        this.destination = destination;
        var delta = (destination - Position).normalized;
        if (delta != Vector2.up && delta != Vector2.down && delta != Vector2.left && delta != Vector2.right)
            throw new ArgumentException($"Incorrent destination to move unit: {destination}");
        if (!Game.Map.RoadNodes.Contains(destination))
            throw new InvalidOperationException("You can't move unit to not-road node");
        moveDelta = delta * speed;
        moving = true;
    }

    public override void OnTurn()
    {
        base.OnTurn();
        if (moving && !fighting)
        {
            var newPos = Position + moveDelta;
            if (Game.Units.
                Where(u => u != this).
                Select(u => u.Position).
                Where(p => p != Game.Map.Bounds.LeftDown && p != Game.Map.Bounds.RightDown).
                Where(p => (p - newPos).magnitude <= GameConstants.UnitAreaRange).
                Any())
            {
                OnIdle?.Invoke();
                return;
            }      

            Position = newPos;
            if (Position == destination)
            {
                moving = false;
                OnReachDestination?.Invoke();
                if (Position == Game.Map.Bounds.LeftDown && Owner == Game.RightPlayer ||
                    Position == Game.Map.Bounds.RightDown && Owner == Game.LeftPlayer)
                    Game.EndGame(Owner);
            }
        }
    }
}