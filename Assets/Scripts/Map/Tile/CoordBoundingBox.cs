using UnityEngine;

public class CoordBoundingBox
{

	public readonly float[] topLeft;

	public readonly float[] topRight;

	public readonly float[] bottomLeft;

	public readonly float[] bottomRight;

	public CoordBoundingBox (int x, int y)
	{
		topLeft = Map.TileToWorldCoords(x, y);
		topRight = Map.TileToWorldCoords(x + 1, y);
		bottomRight = Map.TileToWorldCoords(x + 1, y + 1);
		bottomLeft = Map.TileToWorldCoords(x, y + 1);
	}

	public Vector2 Interpolate(float latitude, float longitude) {
		return new Vector2 (
			((latitude - topLeft[0]) / (topRight[0] - topLeft[0])) * 100,
			((longitude - topLeft[1]) / (bottomLeft[1] - topLeft[1])) * 100
		);
	}
}
