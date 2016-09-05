using UnityEngine;
using System.Collections;
using SimpleJSON;
public class MonsterInfo {
    private string MONSTERFILE = "\\Resources\\MonsterData\\monster.json";
    private string MONSTERSPAWNFILE = "\\Resources\\MonsterData\\encounters.json";
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
        info = JSON.Parse(System.IO.File.ReadAllText(Application.dataPath + MONSTERFILE));
        spawns = JSON.Parse(System.IO.File.ReadAllText(Application.dataPath + MONSTERSPAWNFILE));
    }
}
