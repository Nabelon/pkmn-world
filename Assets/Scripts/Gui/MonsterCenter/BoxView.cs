using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class BoxView : MonoBehaviour {
    public GameObject boxGui;
    public void openBox()
    {
        GameObject monsterPanel = boxGui.transform.Find("ScrollContent").Find("MonsterPanel").gameObject;

        var children = new List<GameObject>();
        foreach (Transform child in monsterPanel.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        GuiManager.guiManager.showElement(boxGui);
        List<bag.Monster> box = bag.MonsterBag.getBag().getBox();
        for (int i = 0; i < box.Count; i++)
        {
            GameObject g = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/MonsterButton"));
            g.transform.SetParent(monsterPanel.transform);
            Button b = g.GetComponent<Button>();
            b.onClick.RemoveAllListeners();
            bag.Monster m = bag.MonsterBag.getBag().getBox()[i];
            b.onClick.AddListener(() => receive(m,g));
            boxview.InfoSetter infoSetter = g.AddComponent<boxview.InfoSetter>();
            infoSetter.posBox = i;
            infoSetter.setImages();
        }
    }
    void receive(bag.Monster m, GameObject button)
    {
        if (bag.MonsterBag.getBag().receive(m))
        {
            GameObject.Destroy(button);
        }
    }
}
