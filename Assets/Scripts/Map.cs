using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {

    private Dictionary<string, GameObject> TileSet = new Dictionary<string, GameObject>();
    private int[] FirstPosition;

	void Start () {
        transform.position = Vector3.zero;

        SetCurrentTile(16816, 10729);
	}

    void SetCurrentTile (int x, int y) {

        if (FirstPosition == null) {
            FirstPosition = new int[] { x, y };
        }

        List<string> tiles = new List<string>();
        for (int i = -1; i < 2; i++) {
            for (int j = -1; j < 2; j++) {
                tiles.Add((x + i).ToString() + "_" + (y + j).ToString());
            }
        }

        // Remove old tiles
        foreach (var item in TileSet) {
            if (tiles.IndexOf(item.Key) == -1) {

                // Destroy the tile
                Destroy(item.Value);

                // Remove it from the active TileSet
                TileSet.Remove(item.Key);
            }
        }

        // Add new tiles
        foreach (string tile in tiles) {
            if (!TileSet.ContainsKey(tile)) {

                // Create the tile object
                GameObject obj = new GameObject("Tile");
                obj.transform.parent = transform;

                /*
                 * Split the position key so we can set it on the
                 * Tile behaviour.
                 *
                 * This is inefficient and should be refactored
                 */
                string[] split = tile.Split('_');
                int tileX = Int32.Parse(split[0]);
                int tileY = Int32.Parse(split[1]);

                /*
                 * Add the tile behaviour and set both the position (x & y)
                 * and the world position.
                 */
                Tile t = obj.AddComponent<Tile>();
                t.Position = new Vector2(tileX, tileY);
                t.WorldPosition = new Vector3(
                    ((tileX - FirstPosition[0]) * 100),
                    0,
                    ((tileY - FirstPosition[1]) * 100)
                );

                // Add the tile to the tileset dictionary
                TileSet.Add(tile, obj);
            }
        }
    }

    public static float[] TileCoordsToWorldCoords (int x, int y) {
        int zoom = 15;

        double n = Math.PI - ((2.0 * Math.PI * y) / Math.Pow(2.0, zoom));

        return new float[] {
            (float)((x / Math.Pow(2.0, zoom) * 360.0) - 180.0),
            (float)(180.0 / Math.PI * Math.Atan(Math.Sinh(n)))
        };
    }

}
