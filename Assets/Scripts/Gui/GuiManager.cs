using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuiManager : MonoBehaviour {

    public static GuiManager guiManager;
    public GameObject mapGui, boxGui, monsterView, teamView, exitButton;
    private Stack<GameObject> stack = new Stack<GameObject>();
    void Awake()
    {
        boxGui.SetActive(false);
        monsterView.SetActive(false);
        teamView.SetActive(false);
        exitButton.SetActive(false);
        mapGui.SetActive(true);
        stack.Push(mapGui);
        guiManager = this;
    }
    public void showElement(GameObject element)
    {
        stack.Peek().SetActive(false);
        element.SetActive(true);
        stack.Push(element);
        exitButton.SetActive(true);
    }
    public void closeElement()
    {
        stack.Pop().SetActive(false);
        stack.Peek().SetActive(true);
        if (stack.Count == 1)
        {
            exitButton.SetActive(false);
        }
    }
}
