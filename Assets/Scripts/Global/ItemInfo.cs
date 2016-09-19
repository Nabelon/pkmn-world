using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemInfo {
    private Dictionary<bag.Item, int> prices; //-1 == not buyable
    private static ItemInfo itemInfo;
    private ItemInfo()
    {
        prices = new Dictionary<bag.Item, int>();
        prices.Add(bag.Item.MonsterBall, 300);
        prices.Add(bag.Item.MegaBall, 700);
        prices.Add(bag.Item.BestBall, 1300);
    }
    public static ItemInfo getItemInfo() {
        if(itemInfo == null) {
            itemInfo = new ItemInfo();
        }
        return itemInfo;
    }
    public int getPrice(bag.Item item) {
        if (prices.ContainsKey(item))
        {
            return prices[item];
        }
        return -1;
    }
}
