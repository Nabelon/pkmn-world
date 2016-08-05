using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public float lerpMultiply = 2;

	public float lastLat;
	public float lastLon;
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
		if (Input.location.status != LocationServiceStatus.Running) return;

		// raycast to get the tile under us
		LocationInfo lastLoc = Input.location.lastData;
		RaycastHit hitInfo;
		if (Physics.Raycast(transform.position, Vector3.down, out hitInfo))
		{
			// this should be our tile
			Tile tile = hitInfo.transform.parent.gameObject.GetComponent<Tile>();
			if (tile == null) return;

			// calculate our position in the current tiles bounding box
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
				Vector3.Distance(prevPos, newPos) * lerpMultiply
			);
		}

		prevPos = transform.position;
	}
}
