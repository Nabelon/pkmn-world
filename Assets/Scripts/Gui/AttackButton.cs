using UnityEngine;
using System.Collections;

public class AttackButton : MonoBehaviour {

    private AttackMove move;
    public void pressed()
    {
        fight.FightingManager.attackerAction = move;
    }
    public void setMove(AttackMove move) {
        this.move = move;
        if (move == null)
        {
            transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = "";
            GetComponent<UnityEngine.UI.Image>().color = new Color(255, 255, 255, 0);
        }
        else
        {

            transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = move.getName();
            GetComponent<UnityEngine.UI.Image>().color = new Color(255, 255, 255, 255);
        }
    }
}
