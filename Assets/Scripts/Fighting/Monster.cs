using UnityEngine;
using System.Collections;
namespace fight
{
    public class Monster
    {
        public readonly string name,id;
        public readonly int mLevel;
        private int mAtk, mDef, mMaxHp, mCurrHp, mSpDef, mSpAtk, mSpeed;
        public readonly string[] types = new string[2];
        private bag.Monster bagmonster;
        public string[] moves = new string[4];
        public Monster(bag.Monster m) {
            id = m.id;
            name = m.name;
            mLevel = m.mLevel;
            mAtk = m.getMAtk();
            mDef = m.getMDef();
            mMaxHp = m.getMMaxHp();
            mCurrHp = m.mCurrHp;
            mSpDef = m.getMSpDef();
            mSpAtk = m.getMSpAtk();
            mSpeed = m.getMSpeed();
            types = m.types;
            bagmonster = m;
            for (int i = 0; i < 4; i++)
            {
                moves[i] = m.attackMoves[i];
            }
        }
        public AttackMove getMove(int i)
        {
            if (moves[i] != null)
            {
                if (fight.FightingManager.attacker.Equals(this))
                {
                    return AttackMoveFactory.getAttackMove(moves[i], this, fight.FightingManager.defender);
                }
                else
                {
                    return AttackMoveFactory.getAttackMove(moves[i], this, fight.FightingManager.attacker);
                }
            }
            return null;
        }
        //returns true if monster died
        public bool getAttacked(int damage, Monster attacker = null, AttackMove attack = null, bool crit = false, float typeMult = 1.0f) {
            if (crit)
            {
                TextBox.addText("Critical Hit!");
            }
            TextBox.addText(attacker.name + "'s "+attack.getName() + " dealt " + damage + " to " + name);
            if (typeMult > 1.01f)
            {
                TextBox.addText("Thats very effective");
            } else if (typeMult < 0.99f) {
                TextBox.addText("Thats not effective");
            }
            mCurrHp = Mathf.Max(mCurrHp - damage, 0);
            if (mCurrHp == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void addExp(int exp)
        {
            bagmonster.mCurrHp = mCurrHp;
            bagmonster.addExp(exp);
            if (bagmonster.mLevel > mLevel)
            {
                TextBox.addText(name + " leveled up!");
                //TODO: change count of attack Moves
                fight.FightingManager.attacker = new Monster(bagmonster);
            }
            else
            {
                TextBox.addText("Exp left: " + (bagmonster.getExpToNextLevel() - bagmonster.getMExp()));
            }
        }
        //TODO: add modifiers
        public int getMAtk()
        {
            return mAtk;
        }
        public int getMDef()
        {
            return mDef;
        }
        public int getMMaxHp()
        {
            return mMaxHp;
        }
        public int getMCurrHp()
        {
            return mCurrHp;
        }
        public int getMSpDef()
        {
            return mSpDef;
        }
        public int getMSpAtk()
        {
            return mSpAtk;
        }
        public int getMSpeed()
        {
            return mSpeed;
        }
    }
}