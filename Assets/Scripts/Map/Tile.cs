using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Tile : MonoBehaviour {

    public Vector2 Pos { get; set; }

    public float[,] Box { get; set; }

    public int TileSize = 100;

    void Start () {

        Box = CreateBoundingBox();

        // TODO: loading indicator

        // Start loading data
        StartCoroutine(Create(Pos));
    }

    private float[,] CreateBoundingBox () {
        float[] topleft = Map.TileCoordsToWorldCoords((int)Pos.x, (int)Pos.y);
        float[] topright = Map.TileCoordsToWorldCoords((int)Pos.x + 1, (int)Pos.y);
        float[] bottomright = Map.TileCoordsToWorldCoords((int)Pos.x + 1, (int)Pos.y + 1);
        float[] bottomleft = Map.TileCoordsToWorldCoords((int)Pos.x, (int)Pos.y + 1);

        return new float[,] {
            { topleft[0], topleft[1] },
            { topright[0], topright[1] },
            { bottomright[0], bottomright[1] },
            { bottomleft[0], bottomleft[1] }
        };
    }

    IEnumerator Create (Vector2 pos) {
        string url = "http://vector.mapzen.com/osm/water,earth,roads/15/";
        string tileUrl = pos.x + "/" + pos.y;

        // Create the request and wait for a response
        WWW request = new WWW(url + tileUrl + ".json");
        yield return request;

        // Parse response into the SimpleJSON format
        JSONNode response = JSON.Parse(request.text);

        // Add water to the tile
        CreateWaterLayer(response["water"]);
    }

    void CreateWaterLayer(JSONNode waterData) {
        GameObject waterObj = new GameObject("Water");
        Water waterBehaviour = waterObj.AddComponent<Water>();
        waterObj.AddComponent<MeshRenderer>();
        waterObj.AddComponent<MeshFilter>();
        waterBehaviour.Data = waterData;
        waterObj.transform.parent = transform;
    }

}
