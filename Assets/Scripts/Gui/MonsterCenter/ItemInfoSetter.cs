using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemInfoSetter : MonoBehaviour {

    public bag.Item item;
    // Use this for initialization
    public void setImages()
    {
        Image monsterImage = transform.Find("MonsterImage").gameObject.GetComponent<Image>();
        Text description = transform.Find("Description").gameObject.GetComponent<Text>();

        description.text = item.ToString() + "\n" + ItemInfo.getItemInfo().getPrice(item);
    }
}
