using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Spawner : MonoBehaviour {
    private bool running = false;
    LanduseManager.LanduseManager landuseManager;
    public int numberOfSpawns = 0;
    public int minNumberOfSpawns = 20;
	// Use this for initialization
	void Start () {
        landuseManager = new LanduseManager.LanduseManager();
	}
    public void monsterDespawned()
    {
        numberOfSpawns--;
    }
	// Update is called once per frame
	void Update () {
        if (!running) return;
        if(numberOfSpawns <=  minNumberOfSpawns)
        {
            var lastLoc = LocationController.GetLastData();
            if (spawn(lastLoc.latitude + Random.Range(-0.006f, 0.006f), lastLoc.longitude + Random.Range(-0.006f, 0.006f))) numberOfSpawns++;
        }
	}
    public void startRunning()
    {
        running = true;

    }
    public bool spawn(float lat, float lng)
    {
        map.Monster monster = getMonster(lat, lng);
        if (monster == null) return false;
        GameObject.FindObjectOfType<Map>().Spawn(monster, lat, lng);
        return true;
    }
    private map.Monster getMonster(float lat, float lng)
    {

        MonsterInfo info = MonsterInfo.getMonsterInfo();
        List<string> landuses = landuseManager.getLanduse(lat, lng);
        List<string> monsterIds = new List<string>();
        WeatherControler weatherCon = WeatherControler.getWeatherControler();
        string time = weatherCon.getTime();
        string weather = weatherCon.getWeather(lat,lng);
        if (weather == null || landuses == null) return null;

        foreach (string landuse in landuses)
        {
            for (int i = 0; i < info.spawns[0][landuse][time][weather].Count; i++)
            {
                monsterIds.Add(info.spawns[0][landuse][time][weather][i]);
            }
        }
        if (monsterIds.Count < 3)
        {
            for (int i = 0; i < info.spawns[1][time][weather].Count; i++)
            {
                monsterIds.Add(info.spawns[1][time][weather][i]);
            }
        }
        GameObject.Find("MonsterDex").transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = "wow";
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject.Find("MonsterDex").transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = "SPHERE SUCKSSSSSSS";
        obj.AddComponent<Animation>();
        obj.AddComponent<MeshCollider>();
        map.Monster monster = obj.AddComponent<map.Monster>();
        monster.initiate(monsterIds[(int)Random.Range(0, monsterIds.Count)]);
        obj.GetComponent<Renderer>().material.color = monster.color;
        
        return monster;
    }
}
