using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LearnAttack : TextBoxAction
{
    private string attack;
    private bag.Monster monster;
    private GameObject yesNoPanel;
    private Button yesButton;
    private Button noButton;
    public LearnAttack(string attack, bag.Monster monster)
    {
        this.monster = monster;
        this.attack = attack;
    }
    public override void doAction(TextBox textBox)
    {
        for (int i = 0; i < 4; i++)
        {
            if (monster.attackMoves[i] == null)
            {
                monster.attackMoves[i] = attack;
                TextBox.showText(monster.name + " learned " + attack);
                fight.FightingManager.attacker = new fight.Monster(monster);
                GameObject.Find("Move" + (i + 1).ToString()).GetComponent<AttackButton>().setMove(fight.FightingManager.attacker.getMove(i));

                
                return;
            }
        }
        textBox.writeText(monster.name + " wants to learn " + attack + ", but already knows 4 attacks,\n should he forget one?");
        yesNoPanel = GameObject.Find("Canvas").transform.FindChild("YesNoPanel").gameObject;
        yesNoPanel.SetActive(true);
        yesButton = yesNoPanel.transform.Find("YesButton").GetComponent<Button>();
        noButton = yesNoPanel.transform.Find("NoButton").GetComponent<Button>();
        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(() => yesForgetClicked());
        noButton.onClick.RemoveAllListeners();
        noButton.onClick.AddListener(() => noForgetClicked());
    }
    void yesForgetClicked()
    {
        GameObject.Find("Canvas").transform.FindChild("YesNoPanel").gameObject.SetActive(false);
        GameObject movesPanel = GameObject.Find("BlockUiPanel").transform.FindChild("ForgetMovePanel").gameObject;
        movesPanel.SetActive(true);
        Button b;
        //sont know why for loop doesnt work
        b = GameObject.Find("ForgetMove" + (0 + 1).ToString()).GetComponent<Button>();
        GameObject.Find("ForgetMove" + (0 + 1).ToString()).GetComponent<AttackButton>().setMove(fight.FightingManager.attacker.getMove(0));
        b.onClick.RemoveAllListeners();
        b.onClick.AddListener(() => forgetMove(0));

        b = GameObject.Find("ForgetMove" + (1 + 1).ToString()).GetComponent<Button>();
        GameObject.Find("ForgetMove" + (1 + 1).ToString()).GetComponent<AttackButton>().setMove(fight.FightingManager.attacker.getMove(1));
        b.onClick.RemoveAllListeners();
        b.onClick.AddListener(() => forgetMove(1));

        b = GameObject.Find("ForgetMove" + (2 + 1).ToString()).GetComponent<Button>();
        GameObject.Find("ForgetMove" + (2 + 1).ToString()).GetComponent<AttackButton>().setMove(fight.FightingManager.attacker.getMove(2));
        b.onClick.RemoveAllListeners();
        b.onClick.AddListener(() => forgetMove(2));

        b = GameObject.Find("ForgetMove" + (3 + 1).ToString()).GetComponent<Button>();
        GameObject.Find("ForgetMove" + (3 + 1).ToString()).GetComponent<AttackButton>().setMove(fight.FightingManager.attacker.getMove(3));
        b.onClick.RemoveAllListeners();
        b.onClick.AddListener(() => forgetMove(3));
        
        b = movesPanel.transform.Find("BackButton").GetComponent<Button>();
        b.onClick.RemoveAllListeners();
        b.onClick.AddListener(() => retryLearn());
    }
    void retryLearn()
    {
        GameObject.Find("BlockUiPanel").transform.FindChild("ForgetMovePanel").gameObject.SetActive(false);
        yesNoPanel.SetActive(false);
        TextBox.addActionFirst(new LearnAttack(attack, monster));
        TextBox.doNextAction();
        
    }
    void forgetMove(int movePos)
    {
        Debug.Log("Forget move on pos:" + movePos.ToString());
        TextBox.addActionFirst(new LearnAttack(attack, monster));
        TextBox.showText(monster.name + " forgot " + monster.attackMoves[movePos]);
        monster.attackMoves[movePos] = null;
        GameObject movesPanel = GameObject.Find("BlockUiPanel").transform.FindChild("ForgetMovePanel").gameObject;
        movesPanel.SetActive(false);

    }
    void noForgetClicked()
    {
        TextBox.showText("Are you sure?");
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(() => yesSureClicked());
        yesButton.onClick.AddListener(() => yesNoPanel.SetActive(false));
        noButton.onClick.AddListener(() => retryLearn());
    }
    void yesSureClicked()
    {
        TextBox.showText(monster.name + " didnt learn " + attack);
    }
}
