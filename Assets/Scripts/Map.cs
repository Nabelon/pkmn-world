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
                // TODO: remove item from scene
                TileSet.Remove(item.Key);
            }
        }

        // Add new tiles
        foreach (string tile in tiles) {
            if (!TileSet.ContainsKey(tile)) {

                GameObject obj = new GameObject("Tile");
                Tile t = obj.AddComponent<Tile>();

                string[] split = tile.Split('_');
                int tileX = Int32.Parse(split[0]);
                int tileY = Int32.Parse(split[1]);
                t.Pos = new Vector2(tileX, tileY);

                obj.transform.parent = transform;

                t.WorldPosition = new Vector3(
                    ((tileX - FirstPosition[0]) * 100),
                    0,
                    ((tileY - FirstPosition[1]) * 100)
                );

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
