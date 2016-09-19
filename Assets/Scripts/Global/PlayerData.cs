using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {

    static int level = 1;
    static int exp = 0;
    static int money = 1000;
    static string name = "unknown";
    public static void recreate(saveload.Player p) {
        level = p.level;
        exp = p.exp;
        money = p.money;
        name = p.name;
    }
    public static void addExp(int exp1) {
        exp += exp1;
        while(exp >= expToNextLevel()){
            exp -= expToNextLevel();
            level++;
        }
    }
    public static int expToNextLevel() {
        int expNext = (int) Mathf.Pow(2, level - 1) * 1000;
        return expNext;
    }
    public static void addMoney(int amount)
    {
        money += amount;
    }
    public static int getLevel()
    {
        return level;
    }
    public static int getMoney()
    {
        return money;
    }
    public static int getExp() {
        return exp;
    }
    public static bool takeMoney(int amount)
    {
        if (money - amount >= 0)
        {
            money -= amount;
            return true;
        }
        return false;
    }
    public static string getName()
    {
        return name;
    }
}
