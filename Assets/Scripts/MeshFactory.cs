using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class MeshFactory {

    public static Mesh CreatePolygonMesh(JSONNode coords, float[,] bb) {

        List<Vector3> normals = new List<Vector3>();
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> triangulation = new List<Vector2>();

        for (int j = 0; j < coords.Count - 1; j++) {
            Vector2 vertex = new Vector2(
                ((float.Parse(coords[j][0].Value) - bb[0, 0]) / (bb[1, 0] - bb[0, 0])) * 100,
                ((float.Parse(coords[j][1].Value) - bb[0, 1]) / (bb[3, 1] - bb[0, 1])) * 100
            );

            triangulation.Add(vertex);
            vertices.Add(new Vector3(vertex.x, 0, vertex.y));
            normals.Add(Vector3.up);
        }

        Triangulator t = new Triangulator(triangulation.ToArray());
        Mesh m = new Mesh();
        m.vertices = vertices.ToArray();
        m.triangles = t.Triangulate();
        m.normals = normals.ToArray();
        m.RecalculateBounds();
        m.RecalculateNormals();

        return m;
    }

}
