using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour {

    void Update() {

        // Check for left mouse button
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)) {
            Vector2 position;

			if (Input.touchSupported) {

				// Use touch position when supported by the device
				position = Input.GetTouch (0).position;
			} else {

				// Use mouse position as a fallback
				position = Input.mousePosition;
			}

			Ray ray = Camera.main.ScreenPointToRay (position);
			RaycastHit hit;

			// Do a raycast, the max size here needs to be better defined.
			if (Physics.Raycast (ray, out hit, 300.0f)) {
				if (hit.transform == transform) {

					// TODO: Move to the battle scene

				}
			}
        }

    }

}
