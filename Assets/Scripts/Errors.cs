using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// TODO: Better error handling
/**
 * This class displays errors to the user.
 */
public class Errors : MonoBehaviour
{
	public static string CurrentError = "";

	private Text errorText;

	void Start()
	{
		errorText = GameObject.Find("UI/Canvas/ErrorText").GetComponent<Text>();
	}

	void Update()
	{
		errorText.text = CurrentError;
	}
}
