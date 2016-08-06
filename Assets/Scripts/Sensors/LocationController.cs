using UnityEngine;
using System.Collections;

public class LocationController : MonoBehaviour
{
	[Tooltip("Maximum amount of seconds to wait for location service to initialize.")]
	public int maxInitWait = 15;
	public float desiredAccuracyInMeters = 5;
	public float updateDistanceInMeters = 5;

	IEnumerator Start()
	{
		// check if user has allowed location
		if (!Input.location.isEnabledByUser)
			yield break;

		// start location service
		Input.location.Start(desiredAccuracyInMeters, updateDistanceInMeters);

		// wait until service initializes
		int waitingTime = maxInitWait;
		while (Input.location.status == LocationServiceStatus.Initializing && waitingTime > 0)
		{
			yield return new WaitForSeconds(1);
			waitingTime--;
		}

		// if service didn't initialize in time
		if (waitingTime < 1)
		{
			Errors.CurrentError = "Location didn't init in time";
			yield break;
		}

		// if connection has failed
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			Errors.CurrentError = "Location failed to connect";
			yield break;
		}
	}
}

