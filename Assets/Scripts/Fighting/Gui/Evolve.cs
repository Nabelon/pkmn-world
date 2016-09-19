using UnityEngine;
using System.Collections;

public class Evolve : TextBoxAction {
    bag.Monster monster;
    string evolveTo;
    public Evolve(bag.Monster monster, string evolveTo)
    {
        this.monster = monster;
        this.evolveTo = evolveTo;
    }
    public override void doAction(TextBox textBox)
    {
        string oldName = monster.name;
        monster.evolveTo(evolveTo);
        textBox.writeText(oldName + " evolve to " + monster.name);
        fight.FightingManager.attacker = new fight.Monster(monster);
        fight.FightingManager.reload();
    }
}
