using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace fight
{
    public class FightingManager : MonoBehaviour
    {
        public static fight.Monster attacker, defender;
        public static Action attackerAction, defenderAction;
        // Use this for initialization
        void Start()
        {
            Image attackerImage = GameObject.Find("AttackerImage").GetComponent<Image>();
            Sprite sprite = (Sprite)Resources.Load<Sprite>("MonsterData/icons/back/" + attacker.id);
            attackerImage.overrideSprite = sprite;

            Image defenderImage = GameObject.Find("DefenderImage").GetComponent<Image>();
            sprite = (Sprite)Resources.Load<Sprite>("MonsterData/icons/" + defender.id);
            defenderImage.overrideSprite = sprite;
            defenderAction = attacker.moves[0];
            for (int i = 0; i < 4; i++)
            {
                GameObject.Find("Move" + (i+1).ToString()).GetComponent<AttackButton>().setMove(attacker.moves[i]);
            }
            GameObject.Find("MovesPanel").SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (attackerAction == null || defenderAction == null)
            {
                return;
            }
            if (attackerAction.getPriority() < defenderAction.getPriority())
            {
                defenderAction.doAction();
                checkFainted();
                attackerAction.doAction();

            }
            else
            {
                attackerAction.doAction();
                checkFainted();
                defenderAction.doAction();
            }
            checkFainted();
            attackerAction = null;
        }
        private void checkFainted() {
            if (defender.getMCurrHp() < 1)
            {
                Debug.Log("Won");
                UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
            }
            if (attacker.getMCurrHp() < 1)
            {
                Debug.Log("Lost");
                UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
            }
        }
    }
    public abstract class Action
    {
        public abstract int getPriority();
        public abstract void doAction();
    }
}