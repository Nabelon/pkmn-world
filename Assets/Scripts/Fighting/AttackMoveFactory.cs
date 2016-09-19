using UnityEngine;
using System.Collections;

public class AttackMoveFactory {

    public static AttackMove getAttackMove(string name, fight.Monster user, fight.Monster target = null)
    {
        SimpleJSON.JSONNode move = MonsterInfo.getMonsterInfo().getMoveJson(name);
        if (move == null)
        {
            return new SimpleAttack("BUGBUGBUG", user, target, "fire", 40, 100, 40, 0.5f, "Special");
        }

        return new SimpleAttack(name, user, target, move["type"].Value, int.Parse(move["dmg"].Value), int.Parse(move["acc"].Value), 40, 0.1f, move["cat"]);


    }
}
