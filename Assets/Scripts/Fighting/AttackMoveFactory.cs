using UnityEngine;
using System.Collections;

public class AttackMoveFactory {

    public static AttackMove getAttackMove(string name, fight.Monster user, fight.Monster target = null)
    {
        return new SimpleAttack(name, user, target);
    }
}
