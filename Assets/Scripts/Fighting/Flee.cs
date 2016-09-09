using UnityEngine;
using System.Collections;

public class Flee : MonoBehaviour {
    public void flee() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }
}
