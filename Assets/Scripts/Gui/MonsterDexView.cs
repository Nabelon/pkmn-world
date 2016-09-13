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
            //bag.MonsterBag.getBag().setMonsterPanel(gameObject);
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
        GameObject panel = gameObject.transform.FindChild("ScrollContent").FindChild("DexPanel").gameObject;
        int maxId = 0;
        foreach (string id in idsSeen)
        {
            maxId = Mathf.Max(maxId, int.Parse(id));
        }
        MonsterInfo mi = MonsterInfo.getMonsterInfo();
        for (int i = 1; i < maxId; i++)
        {
            string idStr = i.ToString();
            GameObject b = (GameObject)Object.Instantiate(Resources.Load("Prefabs/MonsterButton"));
            if (idsCaught.Contains(idStr))
            {
                b.transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = mi.getName(idStr);
                Sprite sprite = (Sprite)Resources.Load<Sprite>("MonsterData/icons/" + idStr);
                b.transform.FindChild("Image").GetComponent<UnityEngine.UI.Image>().overrideSprite = sprite;
            }
            else if (idsSeen.Contains(idStr))
            {
                /*b.transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = mi.getName(idStr);
                Sprite sprite = (Sprite)Resources.Load<Sprite>("MonsterData/icons/" + idStr);
                b.transform.FindChild("Image").GetComponent<UnityEngine.UI.Image>().overrideSprite = sprite;
                */
            }
            else
            {
                b.transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = "???";
                Sprite sprite = (Sprite)Resources.Load<Sprite>("MonsterData/icons/shiny/0");
                b.transform.FindChild("Image").GetComponent<UnityEngine.UI.Image>().overrideSprite = sprite;

            }
                b.transform.SetParent(panel.transform);
        }
    }
}

