using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextBox : MonoBehaviour {
    public static LinkedList<string> stack = new LinkedList<string>();
    private static GameObject textBox;
    private static  GameObject blockUiPanel;
    private System.DateTime lastClick;
    private Coroutine coroutine;
    void Awake()
    {
        textBox = gameObject;
        textBox.SetActive(false);
        blockUiPanel = GameObject.Find("BlockUiPanel");
        blockUiPanel.SetActive(false);
        lastClick = System.DateTime.Now;
    }
    public static void addText(string text)
    {
        if (textBox.activeInHierarchy == false)
        {
            textBox.SetActive(true);
            blockUiPanel.SetActive(true);
        }
        stack.AddLast(text);
        textBox.transform.GetComponent<TextBox>().nextText();
    }
    public void nextText()
    {
        if ((System.DateTime.Now - lastClick).TotalMilliseconds < 500) return;
        lastClick = System.DateTime.Now;
        if (stack.Count == 0)
        {
            blockUiPanel.SetActive(false);
            textBox.SetActive(false);
        }
        else
        {
            if(coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(writeText(stack.First.Value));
            stack.RemoveFirst();
        }
    }
    IEnumerator writeText(string text)
    {
        UnityEngine.UI.Text textB = transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>();
        textB.text = "";
        for (int i = 0; i < text.Length; i++) {
            textB.text += text[i];
            yield return new WaitForSeconds(0.02f);
        }
    }
    public static bool showsText()
    {
        return (textBox.activeSelf);
    }
}
