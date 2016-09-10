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
        private List<Monster> monsterList;
        public Bag(bag.MonsterBag mBag) {
            monsterList = new List<Monster>();
            List<bag.Monster> l = mBag.getMonsters();
            foreach (bag.Monster m in l)
            {
                monsterList.Add(new Monster(m));
            }
        }
        public void recreate() {
            bag.MonsterBag.getBag().getMonsters().RemoveAt(0);
            foreach (Monster m in monsterList)
            {
                bag.MonsterBag.getBag().addMonster(m.getBagMonster());
            }
        }
    }
    [Serializable]
    public class Monster
    {
        private string id;
        private int mExp, mLevel, ivAtk, ivDef, ivHp, ivSpDef, ivSpAtk, ivSpeed, mCurrHp;
        public string[] moves = new string[4];
        public Monster(bag.Monster m)
        {
            id = m.id;
            mLevel = m.mLevel;
            ivAtk = m.ivAtk;
            ivDef = m.ivDef;
            ivHp = m.ivHp;
            mCurrHp = m.mCurrHp;
            ivSpDef = m.ivSpDef;
            ivSpAtk = m.ivSpAtk;
            ivSpeed = m.ivSpeed;
            mExp = m.mExp;
            moves = m.attackMoves;
        }
        public bag.Monster getBagMonster()
        {
            bag.Monster b = new bag.Monster(id, mLevel, ivHp, ivAtk, ivDef, ivSpAtk, ivSpDef, ivSpeed);
            b.attackMoves = moves;
            b.mExp = mExp;
            return b;
        }
    } 
    
}