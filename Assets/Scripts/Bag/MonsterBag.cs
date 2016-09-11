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
        }
        private void addToPanel(Monster m)
        {
            GameObject b = (GameObject)Object.Instantiate(Resources.Load("Prefabs/MonsterButton"));
            b.transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = m.name + "\nLevel: " + m.mLevel;
            Sprite sprite = (Sprite)Resources.Load<Sprite>("MonsterData/icons/" + m.id);
            b.transform.FindChild("Image").GetComponent<UnityEngine.UI.Image>().overrideSprite = sprite;
            b.transform.SetParent(monsterPanel.transform);

        }
        public void setMonsterPanel(GameObject panel)
        {
            monsterPanel = panel;
            foreach (Monster m in monsterList) {
                addToPanel(m);
            }
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
                bag.addMonster(new Monster("4", 10));
                bag.getMonsters()[0].attackMoves[1] = "Ember";
            }
            return bag;
        }
    }
    public class Monster
    {
        public readonly string name;
        public readonly string id;
        public string[] attackMoves = new string[4];
        public string[] type = new string[2];
        public int mAtk, mDef, mMaxHp,  mSpDef, mSpAtk, mExp, mSpeed;
        public int mCurrHp;
        public int ivHp, ivAtk, ivDef, ivSpAtk, ivSpDef, ivSpeed, xpToNextLevel;
        public int mLevel;
        public Monster(string id, int level = 5, int ivHp = 0,int ivAtk = 0, int ivDef = 0, int ivSpAtk = 0, int ivSpDef = 0, int ivSpeed = 0)
        {
            type[0] = MonsterInfo.getMonsterInfo().info[id]["type"].ToString().Replace("\"","");
            this.id = id;
            name = MonsterInfo.getMonsterInfo().info[id]["name"];
            SimpleJSON.JSONNode baseStats = MonsterInfo.getMonsterInfo().baseStats[name]["stats"];
            mLevel = level;
            mExp = 0;
            this.ivAtk = ivAtk; this.ivDef = ivDef; this.ivHp = ivHp; this.ivSpDef = ivSpDef; this.ivSpeed = ivSpeed; this.ivSpAtk = ivSpAtk; //set stats
            this.mAtk = Mathf.FloorToInt(Mathf.Floor((System.Int32.Parse(baseStats["attack"].ToString().Replace("\"","")) + ivAtk) * mLevel / 100.0f + 5.0f));
            this.mDef = Mathf.FloorToInt(Mathf.Floor((System.Int32.Parse(baseStats["defense"].ToString().Replace("\"", "")) + ivDef) * mLevel / 100.0f + 5.0f));
            this.mSpAtk = Mathf.FloorToInt(Mathf.Floor((System.Int32.Parse(baseStats["special_attack"].ToString().Replace("\"", "")) + ivSpAtk) * mLevel / 100.0f + 5.0f));
            this.mSpDef = Mathf.FloorToInt(Mathf.Floor((System.Int32.Parse(baseStats["special_defense"].ToString().Replace("\"", "")) + ivSpDef) * mLevel / 100.0f + 5.0f));
            this.mSpeed = Mathf.FloorToInt(Mathf.Floor((System.Int32.Parse(baseStats["speed"].ToString().Replace("\"", "")) + ivSpeed) * mLevel / 100.0f + 5.0f));
            this.mMaxHp = Mathf.FloorToInt((2 * System.Int32.Parse(baseStats["hp"].ToString().Replace("\"", "")) + ivHp) * level / 100.0f + level + 10.0f);
            mCurrHp = mMaxHp;
            attackMoves[0] = "Tackle";
        }
        public void addExp(int exp) {
            int newExp = mExp + exp;
            int nextLevelExp = getExpToNextLevel();
            while(newExp > nextLevelExp && mLevel < 100) {
                newExp -= nextLevelExp;
                addExpAnimation(nextLevelExp);
                levelUp();
                nextLevelExp = getExpToNextLevel();
            }
            if (mLevel > 99) return;
            mExp = newExp;
        }
        private void addExpAnimation(int exp) {
            //TODO
        }
        private void levelUp() {
            //TODO: Animation
            mLevel++;
            SimpleJSON.JSONNode baseStats = MonsterInfo.getMonsterInfo().baseStats[name]["stats"];      //increase stats
            mAtk = Mathf.FloorToInt(Mathf.Floor((System.Int32.Parse(baseStats["attack"].ToString().Replace("\"","")) + ivAtk) * mLevel / 100.0f + 5.0f));
            mDef = Mathf.FloorToInt(Mathf.Floor((System.Int32.Parse(baseStats["defense"].ToString().Replace("\"", "")) + ivDef) * mLevel / 100.0f + 5.0f));
            mSpAtk = Mathf.FloorToInt(Mathf.Floor((System.Int32.Parse(baseStats["special_attack"].ToString().Replace("\"", "")) + ivSpAtk) * mLevel / 100.0f + 5.0f));
            mSpDef = Mathf.FloorToInt(Mathf.Floor((System.Int32.Parse(baseStats["special_defense"].ToString().Replace("\"", "")) + ivSpDef) * mLevel / 100.0f + 5.0f));
            mSpeed = Mathf.FloorToInt(Mathf.Floor((System.Int32.Parse(baseStats["speed"].ToString().Replace("\"", "")) + ivSpeed) * mLevel / 100.0f + 5.0f));
            int newMaxHp = Mathf.FloorToInt((2 * System.Int32.Parse(baseStats["hp"].ToString().Replace("\"", "")) + ivHp) * mLevel / 100.0f + mLevel + 10.0f);
            mCurrHp += newMaxHp - mMaxHp;
            mMaxHp = newMaxHp;
        }
        public int getExpToNextLevel() {
            int powAdd = (int)Mathf.Pow((float)mLevel, 2.0f);
            int rarity = System.Int32.Parse(MonsterInfo.getMonsterInfo().spawnData[id]["rarity"].ToString().Replace("\"", ""));
            return (int)(100 + mLevel * (2500/(Mathf.Pow((float)rarity ,2.0f) + 25)) + powAdd);
        }
    }
}