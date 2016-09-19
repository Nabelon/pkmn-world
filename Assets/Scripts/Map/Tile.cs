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
        string url = "http://vector.mapzen.com/osm/water,earth,roads,pois/15/";
        string tileUrl = position.x + "/" + position.y;
        string savedAsUrl = "water,earth,roads,pois,15," + position.x + "," + position.y;
        JSONNode response;
        string savedData = saveload.SaveLoad.getMapData(savedAsUrl);
        if (savedData == null)
        {
            // Create the request and wait for a response
            WWW request = new WWW(url + tileUrl + ".json");
            yield return request;
            // Parse response into the SimpleJSON format
            response = JSON.Parse(request.text);
            saveload.SaveLoad.saveMapData(savedAsUrl, request.text);
        }
        else
        {
            yield return new WaitForSeconds(1);
            response = JSON.Parse(savedData);

        }
        // Add water to the tile
        AddLayer<Ground> ("Ground", response ["earth"], 0);
        AddLayer<Water> ("Water", response ["water"], 1);
        AddLayer<Road> ("Roads", response ["roads"], 2);
        transform.position = WorldPosition;
        addPOIs(response["pois"]);
    }
    void Update ()
    {
        transform.position = Vector3.zero;
    }
    private void addPOIs(JSONNode data)
    {
        var format = System.Globalization.CultureInfo.InvariantCulture.NumberFormat;
        JSONNode features = data["features"];
        for (int i = 0; i < features.Count; i++)
        {
            if (features[i]["properties"]["kind"].Value != "place_of_worship" && features[i]["properties"]["kind"].Value != "biergarten" && features[i]["properties"]["kind"].Value != "school")
            {
                continue;
            }
            Vector2 posWc = new Vector2(float.Parse(features[i]["geometry"]["coordinates"][0].Value, format), float.Parse(features[i]["geometry"]["coordinates"][1].Value, format));
            Vector2 posOnTile = BoundingBox.Interpolate(posWc.y, posWc.x);
            GameObject g = (GameObject) GameObject.Instantiate(Resources.Load("Prefabs/MonsterCenter"));
            g.name = features[i]["properties"]["kind"].Value;
            g.transform.parent = transform; 
            g.transform.localPosition = new Vector3( - posOnTile.x - WorldPosition.x, 0.0f, posOnTile.y + WorldPosition.z);

        }
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
