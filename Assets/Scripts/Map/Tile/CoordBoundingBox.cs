using UnityEngine;

public class CoordBoundingBox
{

    public readonly Vector2 topLeft;

    public readonly Vector2 topRight;

    public readonly Vector2 bottomLeft;

    public readonly Vector2 bottomRight;

    public CoordBoundingBox (int x, int y)
    {
        topLeft = Map.TileToWorldCoords (x, y);
        topRight = Map.TileToWorldCoords (x + 1, y);
        bottomRight = Map.TileToWorldCoords (x + 1, y + 1);
        bottomLeft = Map.TileToWorldCoords (x, y + 1);
    }

    public Vector2 Interpolate (float latitude, float longitude)
    {
        return new Vector2 (
            ((longitude - topLeft.x) / (topRight.x - topLeft.x)) * 100,
            ((latitude - topLeft.y) / (bottomLeft.y - topLeft.y)) * 100
        );
    }
}
