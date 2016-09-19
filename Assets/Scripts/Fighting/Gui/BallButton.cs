using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BallButton : MonoBehaviour {
    public bag.Item ballItem;
	// Use this for initialization
	void Start () {
        int count = bag.ItemBag.getBag().getItemCount(ballItem);
        if (count > 0)
        {
            transform.FindChild("Text").GetComponent<Text>().text = ballItem.ToString() + count.ToString();
        }
        else
        {
            gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
        int count = bag.ItemBag.getBag().getItemCount(ballItem);
        if (count > 0)
        {
            transform.FindChild("Text").GetComponent<Text>().text = ballItem.ToString() + " (" + count.ToString() + ")";
        }
        else
        {
            gameObject.SetActive(false);
        }
	}
    public void clicked() {
        fight.FightingManager.attackerAction = new fight.ThrowBall(ballItem);
    }
}
