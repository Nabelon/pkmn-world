using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuiManager : MonoBehaviour {

    public static GuiManager guiManager;
    public GameObject mapGui, boxGui, monsterView, teamView, exitButton, monsterCenterView;
    private Stack<GameObject> stack = new Stack<GameObject>();
    private Stack<bool> stackButton = new Stack<bool>();
    void Awake()
    {
        boxGui.SetActive(false);
        monsterView.SetActive(false);
        teamView.SetActive(false);
        exitButton.SetActive(false);
        mapGui.SetActive(true);
        stack.Push(mapGui);
        stackButton.Push(false);
        guiManager = this;
    }
    public void showElement(GameObject element)
    {
        stack.Peek().SetActive(false);
        element.SetActive(true);
        stack.Push(element);
        exitButton.SetActive(true);
        stackButton.Push(true);
    }
    public void showElementNoButton(GameObject element)
    {
        stack.Peek().SetActive(false);
        element.SetActive(true);
        stack.Push(element);
        stackButton.Push(false);
    }
    public void closeElement()
    {
        stack.Pop().SetActive(false);
        stack.Peek().SetActive(true);
        stackButton.Pop();
        exitButton.SetActive(stackButton.Peek());
    }
    public int getStackSize()
    {
        return stack.Count;
    }
}
