﻿using Vector2 = UnityEngine.Vector2;

public class Tower : ActiveObject
{
    public Tower(Vector2 pos, Player owner) : 
        base(
            pos, 
            owner, 
            GameConstants.TowerHealth, 
            GameConstants.TowerTurnsToAttack, 
            GameConstants.TowerRange, 
            GameConstants.TowerDamage
        )
    { }
}