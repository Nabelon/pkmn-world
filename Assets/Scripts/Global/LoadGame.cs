using UnityEngine;
using System.Collections;

public class LoadGame : MonoBehaviour {
    static bool gameLoaded = false; //load game once
    public bool loadGame = true;    //for testing
	// Use this for initialization
    void Awake()
    {
        if (!gameLoaded && loadGame)
        {
            saveload.SaveLoad.load();
            gameLoaded = true;
        }
    }
}
