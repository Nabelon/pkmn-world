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
		Vector2 tileCoords = Map.WorldToTileCoords(lastLoc.latitude, lastLoc.longitude);
		Tile tile = GameObject.FindObjectsOfType<Tile>().Where((_tile) =>
		{
			// check if x and y are the same as the tile player is on
			return _tile.Position.x == tileCoords.x && _tile.Position.y == tileCoords.y;
		}).First();

		// calculate our position on the map
		CoordBoundingBox bounds = tile.BoundingBox;
		Vector2 interpolatedPos = bounds.Interpolate(lastLoc.latitude, lastLoc.longitude);
		Vector3 newPos = new Vector3(interpolatedPos.x + tile.WorldPosition.x, transform.position.y, interpolatedPos.y + tile.WorldPosition.z);
		// TODO: hack, issue #17
		newPos.x = -newPos.x;

		// smoothly interpolate between our last position and our current position
		transform.position = Vector3.Lerp(
			prevPos,
			newPos,
			Time.fixedDeltaTime * lerpMultiply
		);

		prevPos = transform.position;
	}
}
