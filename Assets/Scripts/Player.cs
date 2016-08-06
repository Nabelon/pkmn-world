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
		var bb = tile.Box;
		Vector3 newPos = new Vector3(
			((lastLoc.longitude - bb[0, 0]) / (bb[1, 0] - bb[0, 0])) * 100,
			transform.position.y,
			((lastLoc.latitude - bb[0, 1]) / (bb[3, 1] - bb[0, 1])) * 100
		);

		// smoothly interpolate between our last position and our current position
		transform.position = Vector3.Lerp(
			prevPos,
			newPos,
			Time.fixedDeltaTime * lerpMultiply
		);

		prevPos = transform.position;
	}
}
