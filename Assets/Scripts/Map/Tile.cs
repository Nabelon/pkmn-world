using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Tile : MonoBehaviour {

    // X/Y position of the tile
    public Vector2 Pos { get; set; }

    // Bounding box (lat/long coords)
    public float[,] Box { get; set; }

    // Tile size in meters
    public int TileSize = 100;

    /*
     * Initialises the tile
     */
    void Start () {

        Box = CreateBoundingBox();

        // TODO: loading indicator

        // Start loading data
        StartCoroutine(Create(Pos));
    }

    /*
     * Create the bounding box coords
     */
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

    /*
     * Load the tile data and start creating all of the
     * tile layers.
     *
     * This needs to be managed in a separate manager component
     */
    IEnumerator Create (Vector2 pos) {

        // Placeholder URL stuff
        string url = "http://vector.mapzen.com/osm/water,earth,roads/15/";
        string tileUrl = pos.x + "/" + pos.y;

        // Create the request and wait for a response
        WWW request = new WWW(url + tileUrl + ".json");
        yield return request;

        // Parse response into the SimpleJSON format
        JSONNode response = JSON.Parse(request.text);

        CreateGrassLayer();

        // Add water to the tile
        CreateWaterLayer(response["water"]);
    }

    /*
     * Create the water layer
     *
     * Needs to be refactored into a method which can
     * create any layer based on the GenericTileLayer.
     */
    void CreateWaterLayer(JSONNode Data) {
        GameObject obj = new GameObject("Water");
        Water behaviour = obj.AddComponent<Water>();
        obj.AddComponent<MeshRenderer>();
        obj.AddComponent<MeshFilter>();
        behaviour.Data = Data;
        obj.transform.parent = transform;
    }

    /*
     * Create the base grass layer
     */
    void CreateGrassLayer () {
        GameObject grassObj = new GameObject("Grass");

        // Create a new plane mesh
        Mesh m = new Mesh();
        m.vertices = new Vector3[] {
            new Vector3(0, 0, 0),
            new Vector3(100, 0, 0),
            new Vector3(100, 0, 100),
            new Vector3(0, 0, 100)
        };
        m.triangles = new int[] { 3, 2, 0, 2, 1, 0 };
        m.RecalculateNormals();

        // Add mesh components
        grassObj.AddComponent<MeshRenderer>();
        grassObj.AddComponent<MeshFilter>();

        // Set mesh properties
        grassObj.GetComponent<MeshFilter>().mesh = m;
        grassObj.GetComponent<MeshRenderer>().material = Resources.Load("Materials/Grass", typeof(Material)) as Material;

        // Attach to parent tile
        grassObj.transform.parent = transform;
    }

}
