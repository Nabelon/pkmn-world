using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {
    public static Map map;
    private Dictionary<Vector2, GameObject> TileSet = new Dictionary<Vector2, GameObject>();
    private Vector2? FirstPosition;
    private int lastTileX = 0;
    private int lastTileY = 0;
    private Spawner spawner;
    /*
     * This will load the tiles dynamically
     * according to the player's geolocation
     * in the real world.
     */
    void FixedUpdate()
    {
        UpdateTiles();

    }
    void Awake()
    {
        if (map == null)
        {
            map = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        UpdateTiles();
        GameObject obj = new GameObject("Spawner");
        spawner = obj.AddComponent<Spawner>();
        DontDestroyOnLoad(obj);
        spawner.startRunning();
    }
    void UpdateTiles()
    {
        // make sure location is initialized
        if (LocationController.GetStatus() != LocationServiceStatus.Running) return;

        var lastLoc = LocationController.GetLastData();
        Vector2 tilePos = WorldToTileCoords(lastLoc.latitude, lastLoc.longitude);

        SetCurrentTile((int)tilePos.x, (int)tilePos.y);
    }

    /*
     * Set the current tile and calculate which tiles need
     * to be removed/added.
     */
    void SetCurrentTile(int x, int y) {
        // Don't regenerate tiles if it's the same tile position as before
        if (lastTileX == x && lastTileY == y) return;

        if (FirstPosition == null)
            FirstPosition = new Vector2(x, y);

        // Make a list of all the required tile positions
        List<Vector2> tiles = new List<Vector2>();
        for (int i = -1; i < 2; i++) {
            for (int j = -1; j < 2; j++) {
                tiles.Add(new Vector2(x + i, y + j));
            }
        }

        // Remove old tiles
        LinkedList<KeyValuePair<Vector2, GameObject>> toDestroy = new LinkedList<KeyValuePair<Vector2, GameObject>>();
        foreach (var item in TileSet)
        {
            if (tiles.IndexOf(item.Key) == -1)
            {
                toDestroy.AddLast(item);
            }
        }
        foreach (var item in toDestroy)
        {
            // Destroy the tile
            Destroy(item.Value);

            // Remove it from the active TileSet
            TileSet.Remove(item.Key);
        }

        // Add new tiles
        foreach (Vector2 tile in tiles) {
            if (!TileSet.ContainsKey(tile)) {

                // Create the tile object
                GameObject obj = new GameObject("Tile");
                obj.transform.parent = transform;

                /*
                 * Add the tile behaviour and set both the position (x & y)
                 * and the world position.
                 */
                Tile t = obj.AddComponent<Tile>();
                t.Position = new Vector2(tile.x, tile.y);
                t.WorldPosition = new Vector3(
                    ((tile.x - FirstPosition.Value.x) * 100),
                    0,
                    ((tile.y - FirstPosition.Value.y) * 100)
                );
                // Add the tile to the tileset dictionary
                TileSet.Add(tile, obj);
            }
        }

        // Refresh last tile position
        lastTileX = x;
        lastTileY = y;
    }

    /*
     * Spawns a monster on the map. It will add the monster as a child
     * to the Tile it tries to spawn on.
     */
    public void Spawn (map.Monster monster, float latitude, float longitude) {
        Vector2 tileCoords = Map.WorldToTileCoords(latitude, longitude);

        // Don't spawn if the tile is not in our active set
        if (!TileSet.ContainsKey(tileCoords))
            return;
        // Get the tile from our tileset
        Tile tile = TileSet[tileCoords].GetComponent<Tile>();

        // Attach our monster to the tile
        monster.transform.parent = tile.transform;
        Vector2 position = tile.BoundingBox.Interpolate (latitude, longitude);
        position.x = -position.x;
        monster.transform.position = new Vector3(position.x - tile.WorldPosition.x, 2.0f, position.y + tile.WorldPosition.z);
	}

	/*
     * Transform real world coordinates (lat/long)
     * to the tile (x/y) coordinates.
     * This is the opposite of TileToWorldCoords.
     */
	public static Vector2 WorldToTileCoords(float lat, float lon, int zoom = 15) {
        return new Vector2(
            (int)((lon + 180.0) / 360.0 * (1 << zoom)),
            (int)((1.0 - Math.Log(Math.Tan(lat * Math.PI / 180.0) +
                1.0 / Math.Cos(lat * Math.PI / 180.0)) / Math.PI) / 2.0 * (1 << zoom))
        );
    }

    /*
     * Transform tile (x/y) coordinates to the real world
     * coordinates (lat/long). This will always return the
     * top-left coordinates of a tile.
     */
    public static Vector2 TileToWorldCoords(int x, int y, int zoom = 15) {
        double n = Math.PI - ((2.0 * Math.PI * y) / Math.Pow(2.0, zoom));

        return new Vector2(
            (float)((x / Math.Pow(2.0, zoom) * 360.0) - 180.0),
            (float)(180.0 / Math.PI * Math.Atan(Math.Sinh(n)))
        );
    }

}
