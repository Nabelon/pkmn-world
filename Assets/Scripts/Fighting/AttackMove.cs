using UnityEngine;
using System.Collections;

public abstract class AttackMove : fight.Action{
    
    protected string mName;
    protected int priority = 2;
    protected int movesLeft;
    protected string category;
    protected fight.Monster user, target;
    protected string type;
    abstract public bool attack();
    public override void doAction()
    {
        if (attack())
        {
            if (fight.FightingManager.attacker.getMCurrHp() == 0)
            {
                TextBox.addText(fight.FightingManager.attacker.name + " fainted.");
            }
            if (fight.FightingManager.defender.getMCurrHp() == 0)
            {
                TextBox.addText(fight.FightingManager.defender.name + " fainted.");
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
    public readonly float critChance;
    private AttackMove child;
    //hitchance == -1 means always hits
    public SimpleAttack(string name, fight.Monster user, fight.Monster target, string type = "normal", int damage = 40, int hitChance = 100,int movesLeft = 40, float critChance = 0.1f, string category = "Physical", AttackMove child = null)
    {
        mName = name;
        this.damage = damage;
        this.hitChance = hitChance;
        this.category = category;
        this.child = child;
        this.movesLeft = movesLeft;
        this.user = user;
        this.target = target;
        this.type = type;
        this.critChance = critChance;
    }
    public override bool attack()
    {
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
        var format = System.Globalization.CultureInfo.InvariantCulture.NumberFormat;
        bool crit = (Random.Range(0.0f, 1.0f) > critChance) ? false : true;
        float type1Mult = (MonsterInfo.getMonsterInfo().typeEffectiveness[type][target.type[0]] == null ? 1.0f : float.Parse(MonsterInfo.getMonsterInfo().typeEffectiveness[type][target.type[0]].ToString().Replace("\"", ""), format));
        float type2Mult = target.type[1] == null ? 1.0f : (MonsterInfo.getMonsterInfo().typeEffectiveness[type][target.type[1]] == null ? 1.0f : float.Parse(MonsterInfo.getMonsterInfo().typeEffectiveness[type][target.type[1]].ToString().Replace("\"", ""), format));
        float modifier = 1.0f * (user.type[0] == type ? 1.5f : 1.0f) * (user.type[1] == type ? 1.5f : 1.0f) * type1Mult * type2Mult * Random.Range(0.8f,1.0f) * (crit ? 2.0f : 1.0f);
        int damageDealt = Mathf.FloorToInt((((2.0f * user.mLevel + 10.0f) / 250.0f) * ((float)atk / (float)def) * (float)damage + 2.0f) * modifier);
        return target.getAttacked(damageDealt, user, this,crit, type1Mult * type2Mult);
    }

}