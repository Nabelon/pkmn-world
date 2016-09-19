using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DiscardMonster : MonoBehaviour {

    public void openDiscardView(GameObject discardView)
    {
        GuiManager.guiManager.showElement(discardView);
        for (int i = 0; i < 6; i++)
        {
            int j = i + 1;
            Button b = discardView.transform.Find("TeamPanel").Find("Monster" + (i + 1).ToString()).GetComponent<Button>();
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(() => discard(j - 1));
        }
    }
    public void discard(int pos)
    {
        if (!bag.MonsterBag.getBag().discard(pos))
        {
            //some animation to show it failed
            Debug.Log("Failed to discard monster at pos " + pos.ToString());
        }
    }
}
