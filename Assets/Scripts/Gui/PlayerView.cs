using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string toShowText = "";
        toShowText += "Name: " + PlayerData.getName() + "\n";
        toShowText += "Level: " + PlayerData.getLevel() + "\n";
        toShowText += "Exp: " + PlayerData.getExp() + " \\ " + PlayerData.expToNextLevel() + "\n";
        toShowText += "Money: " + PlayerData.getMoney() + "\n";
        toShowText += "Seen: " + MonsterDex.getMonsterDex().monsterSeen() + "\n";
        toShowText += "Caught: " + MonsterDex.getMonsterDex().monsterCaught();
        transform.Find("Text").GetComponent<Text>().text = toShowText;
	}
	
	// Update is called once per frame
    void Update()
    {
        string toShowText = "";
        toShowText += "Name: " + PlayerData.getName() + "\n";
        toShowText += "Level: " + PlayerData.getLevel() + "\n";
        toShowText += "Exp: " + PlayerData.getExp() + " \\ " + PlayerData.expToNextLevel() + "\n";
        toShowText += "Money: " + PlayerData.getMoney() + "\n";
        toShowText += "Seen: " + MonsterDex.getMonsterDex().monsterSeen() + "\n";
        toShowText += "Caught: " + MonsterDex.getMonsterDex().monsterCaught();
        transform.Find("Text").GetComponent<Text>().text = toShowText;
	
	}
}
