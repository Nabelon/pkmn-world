using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class MeshFactory
{

    /*
     * Create a polygonal mesh, used for structures such as water
     */
    public static Mesh CreatePolygonMesh (JSONNode coords, CoordBoundingBox bounds)
    {

        // Keep normals and vertices in positions in 3d space
        List<Vector3> normals = new List<Vector3> ();
        List<Vector3> vertices = new List<Vector3> ();

        // The triangulator can only handle 2d shapes so simulate one
        List<Vector2> triangulation = new List<Vector2> ();

        for (int i = 0; i < coords.Count - 1; i++) {

            // Store the vertex for the triangulator
            Vector2 vertex = bounds.Interpolate (float.Parse (coords [i] [0].Value), float.Parse (coords [i] [1].Value));
            triangulation.Add (vertex);

            // Add the vertex plus a y position to allow for 3d space
            vertices.Add (new Vector3 (vertex.x, 0, vertex.y));

            // Normals should always point up on this flat surface
            normals.Add (Vector3.up);
        }

        // Iject triangulation points and create a new base mesh
        Triangulator t = new Triangulator (triangulation.ToArray ());
        Mesh m = new Mesh ();

        // Store vertices and get triangles from the triangulator
        m.vertices = vertices.ToArray ();
        m.triangles = t.Triangulate ();
        m.normals = normals.ToArray ();

        // Recalculate mesh properties
        m.RecalculateBounds ();
        m.RecalculateNormals ();

        return m;
    }

    /*
     * Create a combined line mesh from a multi line one.
     * This needs the parent transform to work for now...
     */
    public static Mesh CreateMultiLineMesh (JSONNode coords, CoordBoundingBox bounds, Transform t)
    {
        List<CombineInstance> combine = new List<CombineInstance> ();

        for (int i = 0; i < coords.Count; i++) {

            // Create a single line mesh
            Mesh line = CreateLineMesh (coords [i], bounds);

            // Add the single line to the combine
            CombineInstance c = new CombineInstance ();
            c.mesh = line;
            c.transform = Matrix4x4.identity;
            combine.Add (c);
        }

        // Join all created single line meshes into a new mesh
        Mesh m = new Mesh ();
        m.CombineMeshes (combine.ToArray (), true);

        return m;
    }

    /*
     * Create a line mesh.
     *
     * It makes my eyes hurt...
     */
    public static Mesh CreateLineMesh (JSONNode coords, CoordBoundingBox bounds)
    {

        float width = 0.5f;

        List<Vector3> vertices = new List<Vector3> ();
        List<Vector3> normals = new List<Vector3> ();
        List<Vector2> triangulation = new List<Vector2> ();

        for (int i = 0; i < coords.Count; i++) {
			
            Vector2 currentInterpolated = bounds.Interpolate (float.Parse (coords [i] [0].Value), float.Parse (coords [i] [1].Value));
            Vector3 current = new Vector3 (currentInterpolated.x, 0, currentInterpolated.y);

            Vector2 referenceInterpolated = bounds.Interpolate (float.Parse (coords [i + (i != coords.Count - 1 ? 1 : -1)] [0].Value), float.Parse (coords [i + (i != coords.Count - 1 ? 1 : -1)] [1].Value));
            Vector3 reference = new Vector3 (referenceInterpolated.x, 0, referenceInterpolated.y);
            
            if (i != coords.Count - 1) {
                Vector3 direction = new Vector3 (-(reference.z - current.z), 0, reference.x - current.x).normalized;

                vertices.Add (current + direction * width);
                normals.Add (Vector3.up);
                triangulation.Add (new Vector2 ((current + direction * width).x, (current + direction * width).z));

                vertices.Add (current + direction * -width);
                normals.Add (Vector3.up);
                triangulation.Add (new Vector2 ((current + direction * -width).x, (current + direction * -width).z));
            } else {
                Vector3 direction = new Vector3 (-(current.z - reference.z), 0, current.x - reference.x).normalized;

                vertices.Add (current + direction * -width);
                normals.Add (Vector3.up);
                triangulation.Add (new Vector2 ((current + direction * -width).x, (current + direction * -width).z));

                vertices.Add (current + direction * width);
                normals.Add (Vector3.up);
                triangulation.Add (new Vector2 ((current + direction * width).x, (current + direction * width).z));
            }

        }

        Mesh m = new Mesh ();
        m.vertices = vertices.ToArray ();
        m.normals = normals.ToArray ();
        Triangulator t = new Triangulator (triangulation.ToArray ());
        m.triangles = t.Triangulate ();
        m.RecalculateNormals ();
        m.RecalculateBounds ();

        return m;
    }

}
