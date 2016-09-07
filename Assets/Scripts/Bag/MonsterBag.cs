using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace bag
{
    public class MonsterBag
    {
        
        private List<Monster> monsterList = new List<Monster>();
        private GameObject monsterPanel;
        private static MonsterBag bag;
        public void addMonster(Monster m)
        {
            monsterList.Add(m);
            GameObject b = (GameObject)Object.Instantiate (Resources.Load("Prefabs/MonsterButton"));
            b.transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = m.name;
            Sprite sprite = (Sprite)Resources.Load<Sprite>("MonsterData/icons/" + m.id);
            b.transform.FindChild("Image").GetComponent<UnityEngine.UI.Image>().overrideSprite = sprite;
            b.transform.SetParent(monsterPanel.transform);

        }
        public List<Monster> getMonsters()
        {
            return monsterList;
        }
        private MonsterBag()
        {
            monsterList = new List<Monster>();
            monsterPanel = GameObject.Find("GameObjectList").GetComponent<ImportantObjects>().monsterPanel;
        }
        public static MonsterBag getBag() {
            if (bag == null)
            {
                bag = new MonsterBag();
            }
            return bag;
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