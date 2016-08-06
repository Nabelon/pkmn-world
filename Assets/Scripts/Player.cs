using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{
	public float lerpMultiply = 10;

	private Vector3 prevPos;

	void Start()
	{
		prevPos = transform.position;
	}

	void FixedUpdate()
	{
		UpdatePosition();
	}

	void UpdatePosition()
	{
		// rotate player with gyroscope
		if (Input.gyro.enabled)
			transform.Rotate(0, -Input.gyro.rotationRateUnbiased.z, 0);

		// make sure location is initialized
		if (LocationController.GetStatus() != LocationServiceStatus.Running) return;

		var lastLoc = LocationController.GetLastData();
		// get the tile we're on
		int[] tileCoords = Map.WorldToTileCoords(lastLoc.latitude, lastLoc.longitude);
		Tile tile = GameObject.FindObjectsOfType<Tile>().Where((_tile) =>
		{
			return _tile.Position.x == tileCoords[0] && _tile.Position.y == tileCoords[1];
		}).First();

		// calculate our position on the map
		CoordBoundingBox bounds = tile.BoundingBox;
		Vector2 interpolatedPosition = bounds.Interpolate (lastLoc.longitude, lastLoc.latitude);
		Vector3 newPosition = new Vector3 (interpolatedPosition.x, transform.position.y, interpolatedPosition.y);

		// smoothly interpolate between our last position and our current position
		transform.position = Vector3.Lerp(
			prevPos,
			newPosition,
			Time.fixedDeltaTime * lerpMultiply
		);

		prevPos = transform.position;
	}
}
