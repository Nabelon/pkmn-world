using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour {
    public string who; //"defender" or "attacker"
    private fight.Monster monster;
    private Image image;
	// Use this for initialization
	void Start () {
        if (who == "defender")
        {
            monster = fight.FightingManager.defender;
        }
        else
        {
            monster = fight.FightingManager.attacker;
        }
        image = GetComponent<Image>();
        image.fillAmount = monster.getMCurrHp() / monster.getMMaxHp();

	}
	
	// Update is called once per frame
	void Update () {

        image.fillAmount = monster.getMCurrHp() / (float) monster.getMMaxHp();
	}
}
