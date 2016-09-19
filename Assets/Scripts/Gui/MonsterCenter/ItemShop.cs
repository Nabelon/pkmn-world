using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class ItemShop : MonoBehaviour
{
    public void openShop()
    {
        GameObject itemPanel = this.transform.Find("ScrollContent").Find("ItemPanel").gameObject;

        var children = new List<GameObject>();
        foreach (Transform child in itemPanel.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        GuiManager.guiManager.showElement(this.gameObject);
        List<bag.Monster> box = bag.MonsterBag.getBag().getBox();
        foreach (bag.Item item in System.Enum.GetValues(typeof(bag.Item)))
        {
            bag.Item i = item;
            GameObject g = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/MonsterButton"));
            g.transform.SetParent(itemPanel.transform);
            Button b = g.GetComponent<Button>();
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(() => buy(i));
            ItemInfoSetter infoSetter = g.AddComponent<ItemInfoSetter>();
            infoSetter.item = item;
            infoSetter.setImages();

        }
    }
    void buy(bag.Item item)
    {
        int price = ItemInfo.getItemInfo().getPrice(item);
        Debug.Log(price);
        if (PlayerData.takeMoney(price))
        {

            bag.ItemBag.getBag().addItem(item);
        }
    }
}
