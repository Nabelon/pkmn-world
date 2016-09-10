using UnityEngine;
using System.Collections;

public class LoadGame : MonoBehaviour {
    bool gameLoaded = false;
	// Use this for initialization
    void Awake()
    {
        if (!gameLoaded)
        {
            saveload.SaveLoad.load();
            gameLoaded = true;
        }
    }
}
