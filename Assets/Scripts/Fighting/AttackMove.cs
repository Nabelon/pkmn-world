using UnityEngine;
using System.Collections;

public abstract class AttackMove : fight.Action{
    
    protected string mName;
    protected int priority = 2;
    protected int movesLeft;
    protected string category;
    abstract public bool attack(fight.Monster attacker, fight.Monster defender);
    public override void doAction()
    {
        if (attack(fight.FightingManager.attacker, fight.FightingManager.defender))
        {
            Debug.Log("fainbted");
        }
    }
    public override int getPriority()
    {
        return priority;
    }
    public string getName() {
        return mName;
    }
}
public class SimpleAttack : AttackMove
{
    public readonly int damage;
    public readonly int hitChance;
    private AttackMove child;
    //hitchance == -1 means always hits
    public SimpleAttack(string name, int damage = 40, int hitChance = 100,int movesLeft = 40, string category = "Physical", AttackMove child = null)
    {
        mName = name;
        this.damage = damage;
        this.hitChance = hitChance;
        this.category = category;
        this.child = child;
        this.movesLeft = movesLeft;
    }
    public override bool attack(fight.Monster attacker, fight.Monster defender) {
        if (child != null)
        {
            child.attack(attacker, defender);
        }
        int atk, def;
        if (category == "Physical")
        {
            atk = attacker.getMAtk();
            def = defender.getMDef();
        }
        else if (category == "Special")
        {
            atk = attacker.getMSpAtk();
            def = defender.getMSpDef();
        }
        else
        {
            Debug.Log("Error: categorie of attack move impossible: " + category);
            return false;
        }
        float modifier = 1.0f; //TODO
        int damageDealt = Mathf.FloorToInt((((2.0f * attacker.level + 10.0f) / 250.0f) * ((float)atk / (float)def) * (float) damage + 2.0f) * modifier);
        return defender.getAttacked(damageDealt, attacker, this);
    }

}