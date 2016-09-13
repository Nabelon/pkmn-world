using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace bag
{
    public class MonsterBag
    {
        
        private List<Monster> monsterBox = new List<Monster>();
        private Monster[] team = new Monster[6];
        private static MonsterBag bag;
        private bool teamIsFull = false;

        //true if monster was added to team
        public bool addMonster(Monster m)
        {
            if (!teamIsFull)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (team[i] == null)
                    {
                        team[i] = m;
                        return true;
                    }
                }
            }
            monsterBox.Add(m);
            return false;
        }
        public Monster[] getTeam() {
            return team;
        }
        private void addToPanel(Monster m, GameObject monsterPanel)
        {
            GameObject b = (GameObject)Object.Instantiate(Resources.Load("Prefabs/MonsterButton"));
            b.transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = m.name + "\nLevel: " + m.mLevel;
            Sprite sprite = (Sprite)Resources.Load<Sprite>("MonsterData/icons/" + m.id);
            b.transform.FindChild("Image").GetComponent<UnityEngine.UI.Image>().overrideSprite = sprite;
            b.transform.SetParent(monsterPanel.transform);

        }
        public List<Monster> getBox()
        {
            return monsterBox;
        }
        private MonsterBag()
        {
            monsterBox = new List<Monster>();
        }
        public static MonsterBag getBag() {
            if (bag == null)
            {
                bag = new MonsterBag();
                bag.team[0] = new Monster("4", 10);
                bag.team[0].attackMoves[1] = "Ember";
            }
            return bag;
        }
    }
    public class Monster
    {
        public readonly string name, nature;
        public readonly string id;
        public string[] attackMoves = new string[4];
        public string[] types;
        private int mAtk, mDef, mMaxHp,  mSpDef, mSpAtk, mExp, mSpeed;
        public int mCurrHp;
        public int ivHp, ivAtk, ivDef, ivSpAtk, ivSpDef, ivSpeed, xpToNextLevel;
        public int mLevel;
        public Monster(string id, int level = 5, string nature = "Jolly", int ivHp = 0,int ivAtk = 0, int ivDef = 0, int ivSpAtk = 0, int ivSpDef = 0, int ivSpeed = 0, int currExp = 0)
        {
            types = MonsterInfo.getMonsterInfo().getTypes(id);
            this.id = id;
            this.nature = nature;
            name = MonsterInfo.getMonsterInfo().getName(id);
            SimpleJSON.JSONNode baseStats = MonsterInfo.getMonsterInfo().getBaseStatsJson(id);
            mLevel = level;
            mExp = 0;
            this.mExp = currExp;
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
            SimpleJSON.JSONNode baseStats = MonsterInfo.getMonsterInfo().getBaseStatsJson(id);      //increase stats
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
            int rarity = MonsterInfo.getMonsterInfo().getRarity(id);
            return (int)(100 + mLevel * (2500/(Mathf.Pow((float)rarity ,2.0f) + 25)) + powAdd);
        }

        public int getMAtk()
        {
            return Mathf.FloorToInt(mAtk * MonsterInfo.getMonsterInfo().getNatureMult(nature, "atk"));
        }
        public int getMDef()
        {
            return Mathf.FloorToInt(mDef * MonsterInfo.getMonsterInfo().getNatureMult(nature, "def"));
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
            return Mathf.FloorToInt(mSpDef * MonsterInfo.getMonsterInfo().getNatureMult(nature, "spDef"));
        }
        public int getMSpAtk()
        {
            return Mathf.FloorToInt(mSpAtk * MonsterInfo.getMonsterInfo().getNatureMult(nature, "spAtk"));
        }
        public int getMSpeed()
        {
            return Mathf.FloorToInt(mSpeed * MonsterInfo.getMonsterInfo().getNatureMult(nature, "speed"));
        }
        public int getMExp()
        {
            return mExp;
        }
    }
}