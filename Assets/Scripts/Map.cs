using UnityEngine;
using System;
using System.Collections;

public class Map : MonoBehaviour {

    /*
     * Initialises the Map behaviour. Now it just loads
     * a static tile (for testing), this needs te be a
     * dynamic tile loading system based on geo position
     * of the player.
     *
     * This should probably be taken care of in a separate
     * manager.
     */
	void Start () {

        GameObject obj = new GameObject("Tile");
        Tile t = obj.AddComponent<Tile>();
        t.Pos = new Vector2(16816, 10729);

        obj.transform.parent = transform;
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
	}

    /*
     * Transform tile (x/y) coordinates to the real world
     * coordinates (lat/long). This will always return the
     * top-left coordinates of a tile.
     */
    public static float[] TileCoordsToWorldCoords (int x, int y) {
        int zoom = 15;

        double n = Math.PI - ((2.0 * Math.PI * y) / Math.Pow(2.0, zoom));

        return new float[] {
            (float)((x / Math.Pow(2.0, zoom) * 360.0) - 180.0),
            (float)(180.0 / Math.PI * Math.Atan(Math.Sinh(n)))
        };
    }

}
