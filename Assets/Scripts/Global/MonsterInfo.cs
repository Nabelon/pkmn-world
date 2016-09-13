using UnityEngine;
using System.Collections;
using SimpleJSON;
public class MonsterInfo {
    private static MonsterInfo monsterInfo;
    private JSONNode info;
    public JSONNode spawns;
    private JSONNode baseStats;
    private JSONNode spawnData;
    private JSONNode natures;
    public JSONNode typeEffectiveness;
    private string[] naturesNames = new string[] {"Rash", "Jolly", "Calm", "Serious","Quirky","Lonely","Bold","Gentle","Naive","Bashful","Modest","Lax","Docile","Sassy","Adamant","Timid","Hasty","Quiet","Naughty","Careful","Brave","Impish","Hardy","Mild","Relaxed"};
       
    public static MonsterInfo getMonsterInfo()
    {
        if (monsterInfo == null)
        {
            monsterInfo = new MonsterInfo();
        }
        return monsterInfo;
    }
    public string getName(string id)
    {
        return info[id]["name"].ToString().Replace("\"","");
    }
    public JSONNode getBaseStatsJson(string id)
    {
        return baseStats[getName(id)]["stats"];
    }
    public int getRarity(string id)
    {
        return int.Parse(spawnData[id]["rarity"].ToString().Replace("\"", ""));
    }
    public string getRandomNature()
    {
        return naturesNames[Random.Range(0, naturesNames.Length)];
    }
    public float getNatureMult(string nature, string stat)
    {
    var format = System.Globalization.CultureInfo.InvariantCulture.NumberFormat;
        return float.Parse(natures[nature][stat].ToString().Replace("\"",""),format);
    }
    public string[] getTypes(string id)
    {
        string[] types = new string[2];
        types[0] = info[id]["type"].ToString().Replace("\"", "");
        return types;
    }
    private MonsterInfo()
    {
        info = JSON.Parse(Resources.Load<TextAsset>("MonsterData/monster").ToString());
        spawns = JSON.Parse(Resources.Load<TextAsset>("MonsterData/encounters").ToString());
        baseStats = JSON.Parse(Resources.Load<TextAsset>("MonsterData/pokemonBaseStats").ToString());
        spawnData = JSON.Parse(Resources.Load<TextAsset>("MonsterData/monsterSpawnData").ToString());
        typeEffectiveness = JSON.Parse(Resources.Load<TextAsset>("MonsterData/typeEffectiveness").ToString());
        natures = JSON.Parse(Resources.Load<TextAsset>("MonsterData/natures").ToString());
    }
}
