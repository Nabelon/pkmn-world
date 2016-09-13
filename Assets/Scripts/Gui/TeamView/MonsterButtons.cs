using UnityEngine;
using System.Collections;
namespace teamView
{
    public class MonsterButtons : MonoBehaviour
    {
        public GameObject MonsterView;
        // Use this for initialization
        public void clicked()
        {
            GuiManager.guiManager.showElement(MonsterView);
        }
    }
}