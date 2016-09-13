using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace fight
{
    public class FightingManager : MonoBehaviour
    {
        public static fight.Monster attacker, defender;
        public static Monster[] attackerTeam, defenderTeam;
        public static Action attackerAction, defenderAction;
        bool fightIsOver = false;
        // Use this for initialization
        void Start()
        {
            Image attackerImage = GameObject.Find("AttackerImage").GetComponent<Image>();
            Sprite sprite = (Sprite)Resources.Load<Sprite>("MonsterData/icons/back/" + attacker.id);
            attackerImage.overrideSprite = sprite;

            Image defenderImage = GameObject.Find("DefenderImage").GetComponent<Image>();
            sprite = (Sprite)Resources.Load<Sprite>("MonsterData/icons/" + defender.id);
            defenderImage.overrideSprite = sprite;
            defenderAction = defender.getMove(0);
            for (int i = 0; i < 4; i++)
            {
                GameObject.Find("Move" + (i + 1).ToString()).GetComponent<AttackButton>().setMove(attacker.getMove(i));
            }
            GameObject.Find("MovesPanel").SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (TextBox.showsText())
            {
                return;
            }

            if (fightIsOver)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
            }
            if (attackerAction == null || defenderAction == null)
            {
                return;
            }
            if (doMoves())
            {
                if (attacker.getMCurrHp() > 0)
                {
                    giveExp(attacker, defender);
                    fightIsOver = true;
                }
            }
            attackerAction = null;
            checkFainted();
        }
        private bool doMoves()
        {
            if (attackerAction.getPriority() < defenderAction.getPriority())
            {
                defenderAction.doAction();
                if (checkFainted()) return true;
                attackerAction.doAction();

            }
            else
            {
                attackerAction.doAction();
                if (checkFainted()) return true;
                defenderAction.doAction();
            }
            if (checkFainted()) return true;
            return false;
        }
        private void giveExp(Monster reciver, Monster lost)
        {
            int giveExp = lost.mLevel * 20 + 100;
            TextBox.addText(reciver.name + " recived " + giveExp + " exp.");
            reciver.addExp(giveExp);
        }
        private bool checkFainted()
        {
            if (defender.getMCurrHp() < 1)
            {
                return true;
            }
            if (attacker.getMCurrHp() < 1)
            {
                return true;
            }
            return false;
        }
        public static bool addTeam(string side, bag.Monster[] team)
        {
            int count = 0;
            if (side == "attacker")
            {
                attackerTeam = new Monster[3];
                for (int i = 0; i < 6 && count < 3; i++)
                {
                    if (team[i] != null && team[i].mCurrHp > 0)
                    {
                        attackerTeam[count++] = new Monster(team[i]);
                    }
                }
                attacker = attackerTeam[0];
            }
            else if (side == "defender")
            {
                defenderTeam = new Monster[3];
                for (int i = 0; i < 6 && count < 3; i++)
                {
                    if (team[i] != null && team[i].mCurrHp > 0)
                    {
                        defenderTeam[count++] = new Monster(team[i]);
                    }
                }
                defender = defenderTeam[0];
            }
            if (count > 0)
            {
                return true;
            }
            return false;
        }

    }
    public abstract class Action
    {
        public abstract int getPriority();
        public abstract void doAction();
    }
}