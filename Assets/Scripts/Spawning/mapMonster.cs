using UnityEngine;
using System.Collections;

public class MapMonster : MonoBehaviour {
    public string monsterName;
    public string id;
    public Color color;
    public void initiate(string id)
    {
        this.id = id;
        monsterName = MonsterInfo.getMonsterInfo().info[id]["name"];
        string type = MonsterInfo.getMonsterInfo().info[id]["type"];
        switch (type)
        {
            case "fire":
                color = new Color(255, 0, 0); break;
            case "water":
                color = new Color(0, 0, 255); break;
            case "grass":
                color = new Color(0, 255, 0); break;
            case "normal":
                color = new Color(255, 255, 255); break;
            default:
                color = new Color(0, 0, 0);
                break;
        }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
