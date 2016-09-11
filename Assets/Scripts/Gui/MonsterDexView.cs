using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterDexView : MonoBehaviour {

	static bool wasDestroyed = false;
    static HashSet<string> idsSeen;
    static HashSet<string> idsCaught;
    void Start()
    {
    }
    
    public void open()
    {
        if (wasDestroyed) {
            bag.MonsterBag.getBag().setMonsterPanel(gameObject);
            wasDestroyed = false;
        }
        gameObject.SetActive(true);
    }
    void onDestroy()
    {
        wasDestroyed = true;
    }
    void setUp()
    {
        GameObject panel = gameObject.transform.FindChild("ScrollContent").FindChild("MonsterPanel").gameObject;
        int maxId = 0;
        foreach (string id in idsSeen)
        {
            maxId = Mathf.Max(maxId, int.Parse(id));
        }
        for (int i = 1; i < maxId; i++)
        {
        }
    }
}

