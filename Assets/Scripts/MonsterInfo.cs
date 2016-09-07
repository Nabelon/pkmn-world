using UnityEngine;
using System.Collections;
using SimpleJSON;
public class MonsterInfo {
    private static MonsterInfo monsterInfo;
    public JSONNode info;
    public JSONNode spawns;
    public static MonsterInfo getMonsterInfo()
    {
        if (monsterInfo == null) monsterInfo = new MonsterInfo();
        return monsterInfo;
    }
    private MonsterInfo()
    {
        info = JSON.Parse(Resources.Load<TextAsset>("MonsterData/monster").ToString());
        spawns = JSON.Parse(Resources.Load<TextAsset>("MonsterData/encounters").ToString());
    }
}
