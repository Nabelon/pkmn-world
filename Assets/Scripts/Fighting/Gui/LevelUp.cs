using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelUp : TextBoxAction {
    bag.Monster monster;
    int level;
    public LevelUp(bag.Monster monster, int level)
    {
        this.monster = monster;
        this.level = level;
    }
    public override void doAction(TextBox textBox)
    {
        LinkedList<string> list;
        KeyValuePair<int, string> evolve = MonsterInfo.getMonsterInfo().evolveOnTo(monster.id, level);
        if (level >= evolve.Key)
        {
            list = MonsterInfo.getMonsterInfo().getMovesForLevel(evolve.Value, level);
            foreach (string s in list)
            {
                if(!knowsAttack(s, monster)) TextBox.addActionFirst(new LearnAttack(s, monster));
            }
            TextBox.addActionFirst(new Evolve(monster, evolve.Value));
           
        }
        list = MonsterInfo.getMonsterInfo().getMovesForLevel(monster.id, level);
        foreach (string s in list)
        {
            if (!knowsAttack(s, monster)) TextBox.addActionFirst(new LearnAttack(s, monster));
        }
        TextBox.showText(monster.name +" reached level "+ level.ToString());
    }
    private bool knowsAttack(string a, bag.Monster m)
    {
        for (int i = 0; i < 4; i++)
        {
            if (m.attackMoves[i] == a) return true;
        }
        return false;
    }
}
