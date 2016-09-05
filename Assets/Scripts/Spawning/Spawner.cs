using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Spawner : MonoBehaviour {
    private bool running = false;
    LanduseManager.LanduseManager landuseManager;
    private int numberOfSpawns = 0;
    public int minNumberOfSpawns = 10;
	// Use this for initialization
	void Start () {
        landuseManager = new LanduseManager.LanduseManager();
	}
	
	// Update is called once per frame
	void Update () {
        if (!running) return;
        if(numberOfSpawns <=  minNumberOfSpawns)
        {
            var lastLoc = LocationController.GetLastData();
            if (spawn(lastLoc.latitude + Random.Range(-0.003f, 0.003f), lastLoc.longitude + Random.Range(-0.003f, 0.003f))) numberOfSpawns++;
        }
	}
    public void startRunning()
    {
        running = true;

    }
    public bool spawn(float lat, float lng)
    {
        // get the tile we will spawn
        Vector2 tileCoords = Map.WorldToTileCoords(lat, lng);
        Tile tile = GameObject.FindObjectsOfType<Tile>().Where((_tile) =>
        {
            // check if x and y are the same as the tile player is on
            return _tile.Position.x == tileCoords.x && _tile.Position.y == tileCoords.y;
        }).First();
        // calculate position on the map
        CoordBoundingBox bounds = new CoordBoundingBox((int) tileCoords.x, (int)tileCoords.y);
        Vector2 interpolatedPos = bounds.Interpolate(lat, lng);
        Vector3 pos = new Vector3(interpolatedPos.x + tile.WorldPosition.x, 0.002f, interpolatedPos.y + tile.WorldPosition.z);
        // TODO: hack, issue #17
        pos.x = -pos.x;
        GameObject monster = getMonster(lat, lng);
        monster.transform.parent = GameObject.Find("Spawner").transform;
        monster.transform.position = pos;
        return true;
    }
    private GameObject getMonster(float lat, float lng)
    {
        MonsterInfo info = MonsterInfo.getMonsterInfo();
        List<string> landuses = landuseManager.getLanduse(lat, lng);
        List<string> monsterIds = new List<string>();
        WeatherControler weatherCon = WeatherControler.getWeatherControler();
        string time = weatherCon.getTime();
        string weather = weatherCon.getWeather(lat,lng);
        Debug.Log(time + "  " + weather);
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
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        MapMonster monster = obj.AddComponent<MapMonster>();
        monster.initiate(monsterIds[(int)Random.Range(0, monsterIds.Count)]);
        obj.GetComponent<Renderer>().material.color = monster.color;
        obj.tag = "MapMonster";
        
        return obj;
    }
}
