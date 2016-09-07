using UnityEngine;
using System.Collections;
using System;

public class LocationController : MonoBehaviour
{
	[Tooltip("Maximum amount of seconds to wait for location service to initialize.")]
	public int maxInitWait = 15;
	public float desiredAccuracyInMeters = 5;
	public float updateDistanceInMeters = 5;

	[Header("Location Mocking")]
	public LocationServiceStatus mockStatus = LocationServiceStatus.Running;
	public float mockLatitude = 47.5657951f;
	public float mockLongitude = -122.2769933f;
    public float speed = 0.0002f;

	private static LocationServiceStatus _mockStatus;
	private static float _mockLatitude;
	private static float _mockLongitude;
	IEnumerator Start()
	{
		// check if user has allowed location
		if (!Input.location.isEnabledByUser)
			yield break;

		// start location service
		Input.location.Start(desiredAccuracyInMeters, updateDistanceInMeters);

		// wait until service initializes
		int waitingTime = maxInitWait;
		while (LocationController.GetStatus() == LocationServiceStatus.Initializing && waitingTime > 0)
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
		if (LocationController.GetStatus() == LocationServiceStatus.Failed)
		{
			Errors.CurrentError = "Location failed to connect";
			yield break;
		}
	}

	void Update()
	{
        mockLongitude += Input.GetAxis("Horizontal") * speed;
        mockLatitude += Input.GetAxis("Vertical") * speed;

        _mockStatus = mockStatus;
		_mockLatitude = mockLatitude;
		_mockLongitude = mockLongitude;
	}

	public static LocationServiceStatus GetStatus()
	{
		return Debug.isDebugBuild ? _mockStatus : Input.location.status;
	}

	public static LocationData GetLastData()
	{
		if (Debug.isDebugBuild)
			return new LocationData(_mockLatitude, _mockLongitude);
		else
			return new LocationData(Input.location.lastData);
	}
}

public struct LocationData
{
	public LocationData(float _latitude, float _longitude)
	{
		altitude = 50;
		horizontalAccuracy = 0;
		latitude = _latitude;
		longitude = _longitude;
		timestamp = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
		verticalAccuracy = 0;
	}

	public LocationData(LocationInfo locInfo)
	{
		altitude = locInfo.altitude;
		horizontalAccuracy = locInfo.horizontalAccuracy;
		latitude = locInfo.latitude;
		longitude = locInfo.longitude;
		timestamp = locInfo.timestamp;
		verticalAccuracy = locInfo.verticalAccuracy;
	}

	/**
	 * Geographical device location altitude.
	 */
	public readonly float altitude;
	
	/**
	 * Horizontal accuracy of the location.
	 */
	public readonly float horizontalAccuracy;
	
	/**
	 * Geographical device location latitude.
	 */
	public readonly float latitude;
	
	/**
	 * Geographical device location latitude.
	 */
	public readonly float longitude;
	
	/**
	 * Timestamp (in seconds since 1970) when location was last time updated.
	 */
	public readonly double timestamp;
	
	/**
	 * Vertical accuracy of the location.
	 */
	public readonly float verticalAccuracy;
}