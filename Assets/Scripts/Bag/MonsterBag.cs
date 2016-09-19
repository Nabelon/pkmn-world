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
        private saveload.Monster[] team1;

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
            teamIsFull = true;
            monsterBox.Add(m);
            return false;
        }
        public bool receive(int pos)
        {
            if (monsterBox.Count < pos)
            {
                Debug.Log("Cant receive that");
                return false;
            }
            if (monsterBox[pos] == null || team[5] != null) return false;
            for (int i = 0; i < 6; i++)
            {
                if (team[i] == null)
                {
                    team[i] = monsterBox[pos];
                    monsterBox.RemoveAt(pos);
                    return true;
                }
            }
            return false; //shouldnt get here
        }
        public bool receive(Monster m)
        {
            int pos = -1;
            for (int i = 0; pos == -1 && i < monsterBox.Count; i++)
            {
                if (monsterBox[i].Equals(m))
                {
                    pos = i;
                }
            }
            if (pos == -1) return false;
            if (monsterBox[pos] == null || team[5] != null) return false;
            for (int i = 0; i < 6; i++)
            {
                if (team[i] == null)
                {
                    team[i] = monsterBox[pos];
                    monsterBox.RemoveAt(pos);
                    return true;
                }
            }
            return false; //shouldnt get here
        }

        public bool discard(int pos)
        {
            if (team[pos] == null || team[1] == null) return false;
            team[pos].heal(1000);
            monsterBox.Add(team[pos]);
            team[pos] = null;
            for (int i = pos; i < 5; i++)
            {
                team[i] = team[i + 1];
            }
            team[5] = null;
            return true;
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

        public MonsterBag(saveload.Monster[] team, List<saveload.Monster> monsterBox)
        {
            this.monsterBox = new List<Monster>();
            foreach (saveload.Monster m in monsterBox)
            {
                this.monsterBox.Add(m.getBagMonster());
            }
            this.team = new Monster[6];
            for (int i = 0; i < 6; i++)
            {
                if (team[i] != null)
                {
                    this.team[i] = team[i].getBagMonster();
                }
            }
        }
        public static MonsterBag getBag() {
            if (bag == null)
            {
                bag = new MonsterBag();
                bag.team[0] = new Monster("25", 5);
                bag.team[0].attackMoves[1] = "Bite";
            }
            return bag;
        }

        public static MonsterBag createBag(saveload.Monster[] team, List<saveload.Monster> monsterBox)
        {
            bag = new MonsterBag(team, monsterBox);
            return bag;
        }

        public void swapMonsters(int posA, int posB)
        {
            int tmpPosA = 0;
            int tmpPosB = 0;
            for (int i = posA; i >= 0; i--)
            {
                if (team[i] != null)
                {
                    tmpPosA = i;
                    break;
                }
            }
            for (int i = posB; i >= 0; i--)
            {
                if (team[i] != null)
                {
                    tmpPosB = i;
                    break;
                }
            }
            if (tmpPosA == tmpPosB) return;
            Monster tmpMonster = team[tmpPosA];
            team[tmpPosA] = team[tmpPosB];
            team[tmpPosB] = tmpMonster;
        }
    }
}