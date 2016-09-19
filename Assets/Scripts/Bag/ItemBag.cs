using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace bag
{
    public enum Item { MonsterBall, MegaBall, BestBall}
    public class ItemBag
    {

        private static ItemBag itemBag;

        public Dictionary<Item, int> itemsDict {get; set;}
        public static ItemBag getBag()
        {
            if (itemBag == null)
            {
                itemBag = new ItemBag();
                itemBag.itemsDict.Add(Item.MonsterBall, 40);
                itemBag.itemsDict.Add(Item.BestBall, 10);
            }
            return itemBag;
        }
        public static ItemBag createBag(Dictionary<Item, int> dict)
        {
            itemBag = new ItemBag(dict);
            return itemBag;
        }
        public ItemBag()
        {
            itemsDict = new Dictionary<Item, int>();
        }
        public ItemBag(Dictionary<Item, int> dict)
        {
            itemsDict = dict;
        }
        public bool removeItem(Item item)
        {
            if (itemsDict.ContainsKey(item))
            {
                itemsDict[item]--;
                if (itemsDict[item] == 0)
                {
                    itemsDict.Remove(item);
                }
                return true;
            }
            return false;
        }
        public int getItemCount(Item item) {
            if (itemsDict.ContainsKey(item))
            {
                return itemsDict[item];
            }
            return 0;
        }
        public void addItem(Item item, int count = 1) {
            if (count < 0)
            {
                throw new UnityException("cant add negative amount of items: " + item.ToString() + ", " + count.ToString());
            }
            if(itemsDict.ContainsKey(item)){
                itemsDict[item] += count;
            }
            else
            {
                itemsDict.Add(item, count);
            }
        }

        public Dictionary<Item, int> getItemsDict()
        {
            return itemsDict;
        }
    }
}