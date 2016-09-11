﻿using UnityEngine;
using System.Collections;

public class AttackMoveFactory {

    public static AttackMove getAttackMove(string name, fight.Monster user, fight.Monster target = null)
    {
        
        if (name == "Ember")
        {
            return new SimpleAttack(name, user, target,"fire",40,100,40,0.5f,"Special");

        }
            return new SimpleAttack(name, user, target);
    }
}
