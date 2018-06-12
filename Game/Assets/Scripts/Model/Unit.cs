using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit : ActiveObject
{
    public event VoidHandler OnReachDestination;
    public event VoidHandler OnIdle;

    private float speed;
    private Vector2 moveDelta;
    private Vector2 blockingPos;
    private Vector2 areaRangeDelta;
    private Vector2 destination;
    private bool moving;
    private bool idle = true;

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
        OnDeath += () =>
        {
            if (Game.UnitBlockedPositions.Contains(blockingPos - areaRangeDelta))
                Game.UnitBlockedPositions.Remove(blockingPos - areaRangeDelta);
            if (Game.UnitPositionQueues.ContainsKey(blockingPos) && Game.UnitPositionQueues[blockingPos].Contains(this))
                Game.UnitPositionQueues[blockingPos] = new Queue<Unit>(Game.UnitPositionQueues[blockingPos].Except(new[] { this }));
        };
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
        areaRangeDelta = delta * GameConstants.UnitAreaRange;
        blockingPos = position + areaRangeDelta;
        moving = true;
    }

    public override void OnTurn()
    {
        base.OnTurn();
        if (moving && !fighting)
        {
            Queue<Unit> queue;
            if (!Game.UnitPositionQueues.ContainsKey(blockingPos))
            {
                queue = new Queue<Unit>();
                Game.UnitPositionQueues[blockingPos] = queue;
            }
            else
                queue = Game.UnitPositionQueues[blockingPos];
            if (!queue.Contains(this))
                queue.Enqueue(this);
            if(Game.UnitBlockedPositions.Contains(blockingPos) || queue.Peek() != this)
            {
                OnIdle?.Invoke();
                idle = true;
                Game.UnitBlockedPositions.Add(blockingPos - areaRangeDelta);
                return;
            }
            if (idle)
                Game.UnitBlockedPositions.Remove(blockingPos - areaRangeDelta);
            Position += moveDelta;
            if (Position == blockingPos)
            {
                queue.Dequeue();
                blockingPos += areaRangeDelta;
            }
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