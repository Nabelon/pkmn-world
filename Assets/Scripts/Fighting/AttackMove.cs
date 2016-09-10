using UnityEngine;
using System.Collections;

public abstract class AttackMove : fight.Action{
    
    protected string mName;
    protected int priority = 2;
    protected int movesLeft;
    protected string category;
    protected fight.Monster user, target;
    abstract public bool attack();
    public override void doAction()
    {
        if (attack())
        {
            if (fight.FightingManager.attacker.getMCurrHp() == 0)
            {
                TextBox.addText(fight.FightingManager.attacker.name + "fainted.");
            } else if (fight.FightingManager.defender.getMCurrHp() == 0)
            {
                TextBox.addText(fight.FightingManager.defender.name + "fainted.");
            }
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
    public SimpleAttack(string name, fight.Monster user, fight.Monster target, int damage = 40, int hitChance = 100,int movesLeft = 40, string category = "Physical", AttackMove child = null)
    {
        mName = name;
        this.damage = damage;
        this.hitChance = hitChance;
        this.category = category;
        this.child = child;
        this.movesLeft = movesLeft;
        this.user = user;
        this.target = target;
    }
    public override bool attack() {
        if (child != null)
        {
            child.attack();
        }
        int atk, def;
        if (category == "Physical")
        {
            atk = user.getMAtk();
            def = target.getMDef();
        }
        else if (category == "Special")
        {
            atk = user.getMSpAtk();
            def = target.getMSpDef();
        }
        else
        {
            Debug.Log("Error: categorie of attack move impossible: " + category);
            return false;
        }
        float modifier = 1.0f; //TODO
        int damageDealt = Mathf.FloorToInt((((2.0f * user.mLevel + 10.0f) / 250.0f) * ((float)atk / (float)def) * (float) damage + 2.0f) * modifier);
        return target.getAttacked(damageDealt, user, this);
    }

}