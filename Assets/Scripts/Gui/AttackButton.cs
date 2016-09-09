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
            transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = "null";
        }
        else
        {

            transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = move.getName();
        }
    }
}
