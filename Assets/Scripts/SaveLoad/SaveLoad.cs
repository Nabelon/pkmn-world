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
        private static MapDataSaver mapDataSaver;
        public static void save()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/bag.dat", FileMode.OpenOrCreate);

            Bag data = new Bag(bag.MonsterBag.getBag(), bag.ItemBag.getBag());
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
        public static string getMapData(string url) {
            if (mapDataSaver == null)
            {
                mapDataSaver = new MapDataSaver();
            }
            return mapDataSaver.getMapData(url);
        }
        public static void saveMapData(string url, string data)
        {
            if (mapDataSaver == null)
            {
                mapDataSaver = new MapDataSaver();
            }
            mapDataSaver.saveMapData(url, data);
        }
    }
    [Serializable]
    public class Player
    {
        public readonly int money;
        public readonly int exp;
        public readonly int level;
        public readonly string name;
        public Player()
        {
            money = PlayerData.getMoney();
            exp = PlayerData.getExp();
            level = PlayerData.getLevel();
            name = PlayerData.getName();
        }
        public void recreate() {
            PlayerData.recreate(this);
        }
    }
    [Serializable]
    public class Bag
    {
        private Player p;
        private Dictionary<bag.Item, int> itemsDict;
        private List<Monster> monsterBox;
        private Monster[] team;
        public Bag(bag.MonsterBag mBag, bag.ItemBag itemBag) {
            monsterBox = new List<Monster>();
            team = new Monster[6];
            for (int i = 0; i < 6; i++)
            {
                bag.Monster m = mBag.getTeam()[i];
                if (m != null)
                {
                    team[i] = new Monster(m);
                }
            }
            List<bag.Monster> l = mBag.getBox();
            itemsDict = itemBag.getItemsDict();
            foreach (bag.Monster m in l)
            {
                monsterBox.Add(new Monster(m));
            }
            p = new Player();
        }
        public void recreate() {
            bag.ItemBag.createBag(itemsDict);
            bag.MonsterBag.createBag(team, monsterBox);
            p.recreate();
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