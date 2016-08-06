using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Tile : MonoBehaviour
{

    // X/Y position of the tile
    public Vector2 Position { get; set; }

    public Vector3 WorldPosition { get; set; }

    // Bounding box (lat/long coords)
    public CoordBoundingBox BoundingBox { get; set; }

    // Tile size in meters
    public int TileSize = 100;

    /*
     * Initialises the tile
     */
    void Start ()
    {

        BoundingBox = new CoordBoundingBox ((int)Position.x, (int)Position.y);

        // TODO: loading indicator

        // Start loading data
        StartCoroutine (Create (Position));
    }

    /*
     * Load the tile data and start creating all of the
     * tile layers.
     *
     * This needs to be managed in a separate manager component
     */
    IEnumerator Create (Vector2 position)
    {

        // Placeholder URL stuff
        string url = "http://vector.mapzen.com/osm/water,earth,roads/15/";
        string tileUrl = position.x + "/" + position.y;

        // Create the request and wait for a response
        WWW request = new WWW (url + tileUrl + ".json");
        yield return request;

        // Parse response into the SimpleJSON format
        JSONNode response = JSON.Parse (request.text);

        // Add water to the tile
        AddLayer<Ground> ("Ground", response ["earth"], 0);
        AddLayer<Water> ("Water", response ["water"], 1);
        AddLayer<Road> ("Roads", response ["roads"], 2);

        transform.position = WorldPosition;
    }

    void Update ()
    {
        transform.position = Vector3.zero;
    }

    /*
     * Create a tile layer.
     *
     * Inject the tile behaviour with a name and data to add
     * the tile layer to the tile object
     */
    void AddLayer<T> (string name, JSONNode Data, int level) where T: GenericTileLayer
    {
        GameObject obj = new GameObject (name);
        obj.transform.parent = transform;
        obj.transform.localPosition = Vector3.zero;

        T behaviour = obj.AddComponent<T> ();
        obj.AddComponent<MeshRenderer> ();
        obj.AddComponent<MeshFilter> ();
        obj.AddComponent<MeshCollider> ();
        behaviour.Data = Data;

        // Move up ever so slightly, this seems stupid as fuck
        obj.transform.position += new Vector3 (0, 0.01f * level, 0);
    }

}
