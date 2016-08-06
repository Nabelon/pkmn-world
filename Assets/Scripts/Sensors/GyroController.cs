using UnityEngine;
using System.Collections;

public class GyroController : MonoBehaviour
{
	void Start()
	{
		if (!SystemInfo.supportsGyroscope)
			return;

		Input.gyro.enabled = true;
	}
}
