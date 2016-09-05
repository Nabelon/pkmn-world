using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace bag
{
    public class MonsterBag
    {
        private static List<Monster> monsterList = new List<Monster>();
        public static void addMonster(Monster m)
        {
            monsterList.Add(m);
        }
        public List<Monster> getMonsters()
        {
            return monsterList;
        } 
    }
    public class Monster
    {
        public readonly string name;
        public readonly string id;
        public Monster(string id)
        {
            this.id = id;
            name = MonsterInfo.getMonsterInfo().info[id]["name"];
        }
    }
}