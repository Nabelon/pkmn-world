using UnityEngine;
using System.Collections;

public class Water : GenericTileLayer {

    /*
     * Overrides the GenericTileLayer method and calls
     * it from the base.
     *
     * Makes me feel a bit dirty and can be cleaned up
     * considerably without the inheritance part.
     */
    public override void Start() {
        base.Start();

        MeshFilter filter = GetComponent<MeshFilter>();
        Mesh mesh = filter.mesh;
    }

}
