using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	private Vector3 offset;
	private Transform player;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		offset = player.transform.position - transform.position;
	}

	void LateUpdate()
	{
		float desiredAngle = player.transform.eulerAngles.y;
		Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);

		transform.position = player.transform.position - (rotation * offset);
		transform.LookAt(player.transform.position);
	}
}
