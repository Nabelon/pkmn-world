using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
namespace saveload
{
    public class SaveLoad
    {
        public static void save()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/bag.dat", FileMode.OpenOrCreate);

            Bag data = new Bag(bag.MonsterBag.getBag());
            bf.Serialize(file, data);
            file.Close();
        }
        public static void load()
        {
            if (File.Exists(Application.persistentDataPath + "/bag.dat"))
            {
                Debug.Log("LOADING");
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/bag.dat", FileMode.Open);
                Bag data = (Bag)bf.Deserialize(file);
                file.Close();
                data.recreate();
            }
        }
    }
    [Serializable]
    public class Bag
    {
        private List<Monster> monsterBox;
        private Monster[] team;
        public Bag(bag.MonsterBag mBag) {
            monsterBox = new List<Monster>();
            List<bag.Monster> l = mBag.getBox();
            foreach (bag.Monster m in l)
            {
                monsterBox.Add(new Monster(m));
            }
        }
        public void recreate() {
            bag.MonsterBag mBag = bag.MonsterBag.getBag();
            mBag.getBox().RemoveAt(0);
            foreach (Monster m in monsterBox)
            {
                mBag.addMonster(m.getBagMonster());
            }
            for (int i = 0; i < 6; i++)
            {
                if (team[i] != null)
                {
                    mBag.getTeam()[i] = team[i].getBagMonster();
                }
            }
        }
    }
    [Serializable]
    public class Monster
    {
        private string id, nature;
        private int mExp, mLevel, ivAtk, ivDef, ivHp, ivSpDef, ivSpAtk, ivSpeed, mCurrHp;
        public string[] moves = new string[4];
        public Monster(bag.Monster m)
        {
            id = m.id;
            nature = m.nature;
            mLevel = m.mLevel;
            ivAtk = m.ivAtk;
            ivDef = m.ivDef;
            ivHp = m.ivHp;
            mCurrHp = m.mCurrHp;
            ivSpDef = m.ivSpDef;
            ivSpAtk = m.ivSpAtk;
            ivSpeed = m.ivSpeed;
            mExp = m.getMExp();
            moves = m.attackMoves;
        }
        public bag.Monster getBagMonster()
        {
            bag.Monster b = new bag.Monster(id, mLevel, nature, ivHp, ivAtk, ivDef, ivSpAtk, ivSpDef, ivSpeed, mExp);
            b.attackMoves = moves;
            return b;
        }
    } 
    
}