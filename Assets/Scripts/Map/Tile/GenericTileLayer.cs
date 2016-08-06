using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class GenericTileLayer : MonoBehaviour
{

    public JSONNode Data  { get; set; }

    /*
     * Initialises a generic tile, creates meshes
     * from previously set data.
     */
    public virtual void Start ()
    {

        // Get renderer from our gameobject
        // MeshRenderer renderer = GetComponent<MeshRenderer>();
        MeshFilter filter = GetComponent<MeshFilter> ();

        // Setup combiner and get tile object
        Tile tile = transform.parent.gameObject.GetComponent<Tile> ();
        List<CombineInstance> combine = new List<CombineInstance> ();

        // Loop through All features
        JSONNode features = Data ["features"];
        for (int i = 0; i < features.Count; i++) {

            JSONNode feature = features [i];

            /*
             * This if structure lives here to determine what kind of
             * geometry types we are able to render. This is not
             * maintainable and needs to change.
             */
            if (
                feature ["geometry"] ["type"].Value == "Polygon" ||
                feature ["geometry"] ["type"].Value == "LineString" ||
                feature ["geometry"] ["type"].Value == "MultiLineString") {
                Mesh m;

                // Grab a mesh from the factory
                if (feature ["geometry"] ["type"].Value == "Polygon") {
                    m = MeshFactory.CreatePolygonMesh (
                        feature ["geometry"] ["coordinates"] [0],
                        tile.BoundingBox
                    );
                } else if (feature ["geometry"] ["type"].Value == "LineString") {
                    m = MeshFactory.CreateLineMesh (
                        feature ["geometry"] ["coordinates"],
                        tile.BoundingBox
                    );
                } else {

                    // The multiline mesh needs the transform to work
                    m = MeshFactory.CreateMultiLineMesh (
                        feature ["geometry"] ["coordinates"],
                        tile.BoundingBox,
                        transform
                    );
                }

                /*
                 * Create a combine instance, this will be used after
                 * creating all individual through the Triangulator.
                 *
                 * This is done because of a limitation of the triangulator.
                 * It can only render one entire polygon and not multiple,
                 * I used it because I was lazy, should probably be rewritten
                 * into an own triangulator to allow for better optimisation.
                 */
                CombineInstance c = new CombineInstance ();
                c.mesh = m;
                c.transform = transform.localToWorldMatrix;
                combine.Add (c);
            }

        }

        /*
         * Create a new parent mesh which will be used to combine all
         * previously generated polygonal meshes.
         */
        filter.mesh = new Mesh ();
        filter.mesh.CombineMeshes (combine.ToArray (), true);
        filter.mesh.Optimize ();

        // Update our mesh collider with the new mesh
        GetComponent<MeshCollider> ().sharedMesh = filter.mesh;
    }

}
