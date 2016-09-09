using UnityEngine;
using System.Collections;

public class ImportantObjects : MonoBehaviour {
    public GameObject monsterPanel = null;
    public GameObject monstersButton = null;
    public GameObject monsterDexButton = null;
    public FightingSceneInfo fightingSceneInfo = null;
    public MapSceneInfo mapSceneInfo = null;
    public void leaveMapView()
    {
        monstersButton.SetActive(false);
        monsterDexButton.SetActive(false);

    }
    public void enterMapView() {
        monstersButton.SetActive(true);
        monsterDexButton.SetActive(true);
    }
}
