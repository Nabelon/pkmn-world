using UnityEngine;
using System.Collections;
namespace fight
{
    public class Monster
    {
        public readonly string name,id;
        public readonly int level;
        private int mAtk, mDef, mMaxHp, mCurrHp, mSpDef, mSpAtk, mSpeed;

        public AttackMove[] moves = new AttackMove[4];
        public Monster(bag.Monster m) {
            id = m.id;
            name = m.name;
            mAtk = m.mAtk;
            mDef = m.mDef;
            mMaxHp = m.mMaxHp;
            mCurrHp = m.mCurrHp;
            mSpDef = m.mSpDef;
            mSpAtk = m.mSpAtk;
            mSpeed = m.mSpeed;
            for (int i = 0; i < 4; i++)
            {
                if (m.attackMoves[i] != null)
                {
                    moves[i] = AttackMoveFactory.getAttackMove(m.attackMoves[i]);
                }
            }
        }
        //returns true if monster died
        public bool getAttacked(int damage, Monster attacker = null, AttackMove attack = null) {
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