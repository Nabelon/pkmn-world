using UnityEngine;
using System.Collections;

public class HealTeam : MonoBehaviour {

	// Use this for initialization
    public void healTeam()
    {
        bag.Monster[] team = bag.MonsterBag.getBag().getTeam();
        for (int i = 0; i < 6 && team[i] != null; i++ )
        {
            team[i].heal(1000);
        }
    }
}
