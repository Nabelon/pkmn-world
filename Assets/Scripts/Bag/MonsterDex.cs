using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterDex {
    private static MonsterDex monsterDex;
    private int seen = 0;
    private int caught = 0;
    public static MonsterDex getMonsterDex()
    {
        if (monsterDex == null)
        {
            monsterDex = new MonsterDex();
        }
        return monsterDex;
    }
    //public static MonsterDex loadMonsterDex();
    private Dictionary<string, bool> dex = new Dictionary<string, bool>(); //false = seen, true = caugth, no Key = never seen
    public bool wasSeen(string id) {
        return dex.ContainsKey(id);
    }
    public bool wasCaught(string id) {
        if(dex.ContainsKey(id) && dex[id]) {
            return true;
        }
        return false;
    }
    public void addToSeen(string id) {
        if(!dex.ContainsKey(id)) {
            dex.Add(id, false);
            seen++;
        }
    }
    public void addToCaugth(string id) {
        if(!dex.ContainsKey(id)) {
            dex.Add(id, true);
            caught++;
        }
        else if (!dex[id])
        {
            dex[id] = true;
            caught++;
        }
    }
    public int monsterSeen()
    {
        return seen;
    }
    public int monsterCaught()
    {
        return caught;
    }

}
