using UnityEngine;
using System.Collections;

public class SaveButton : MonoBehaviour {

    public void clicked()
    {
        saveload.SaveLoad.save();
    }
}
