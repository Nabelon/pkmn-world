using UnityEngine;
using System;
using System.Collections;

public class Map : MonoBehaviour {

	void Start () {

        GameObject obj = new GameObject("Tile");
        Tile t = obj.AddComponent<Tile>();
        t.Pos = new Vector2(16816, 10729);

        obj.transform.parent = transform;
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
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
