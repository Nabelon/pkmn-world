using UnityEngine;
using System.Collections;

public class AttackMoveFactory {

    public static AttackMove getAttackMove(string name)
    {
        return new SimpleAttack(name);
    }
}
