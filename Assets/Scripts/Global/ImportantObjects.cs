using UnityEngine;
using System.Collections;

public class ImportantObjects : MonoBehaviour {
    public GameObject monsterPanel = null;
    public GameObject buttonsPanel = null;
    public FightingSceneInfo fightingSceneInfo = null;
    public MapSceneInfo mapSceneInfo = null;
    public void leaveMapView()
    {
        buttonsPanel.SetActive(false);

    }
    public void enterMapView() {
        buttonsPanel.SetActive(true);
    }
}
