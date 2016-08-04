using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Tile : MonoBehaviour {

    // X/Y position of the tile
    public Vector2 Position { get; set; }

    public Vector3 WorldPosition { get; set; }

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
        StartCoroutine(Create(Position));
    }

    /*
     * Create the bounding box coords
     */
    private float[,] CreateBoundingBox () {
        float[] topleft = Map.TileCoordsToWorldCoords((int)Position.x, (int)Position.y);
        float[] topright = Map.TileCoordsToWorldCoords((int)Position.x + 1, (int)Position.y);
        float[] bottomright = Map.TileCoordsToWorldCoords((int)Position.x + 1, (int)Position.y + 1);
        float[] bottomleft = Map.TileCoordsToWorldCoords((int)Position.x, (int)Position.y + 1);

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
    IEnumerator Create (Vector2 position) {

        // Placeholder URL stuff
        string url = "http://vector.mapzen.com/osm/water,earth,roads/15/";
        string tileUrl = position.x + "/" + position.y;

        // Create the request and wait for a response
        WWW request = new WWW(url + tileUrl + ".json");
        yield return request;

        // Parse response into the SimpleJSON format
        JSONNode response = JSON.Parse(request.text);

        // Add water to the tile
        CreateLayer<Ground>("Ground", response["earth"], 0);
        CreateLayer<Water>("Water", response["water"], 1);

        transform.position = WorldPosition;
    }

    void Update () {
        transform.position = Vector3.zero;
    }

    /*
     * Create a tile layer.
     *
     * Inject the tile behaviour with a name and data to add
     * the tile layer to the tile object
     */
    void CreateLayer<T> (string name, JSONNode Data, int level) where T: GenericTileLayer {

        GameObject obj = new GameObject(name);
        obj.transform.parent = transform;
        obj.transform.localPosition = Vector3.zero;

        T behaviour = obj.AddComponent<T>();
        obj.AddComponent<MeshRenderer>();
        obj.AddComponent<MeshFilter>();
        behaviour.Data = Data;

        // Move up ever so slightly, this seems stupid as fuck
        obj.transform.position += new Vector3(0, 0.01f * level, 0);
    }

}
