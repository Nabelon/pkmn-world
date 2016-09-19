using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class TextBox : MonoBehaviour {
    public static LinkedList<TextBoxAction> stack = new LinkedList<TextBoxAction>();
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
    public static void showText(string text)
    {
        textBox.GetComponent<TextBox>().writeText(text);
    }
    public static void addText(string text)
    {
        stack.AddLast(new AddSimpleText(text));
        if (textBox.activeInHierarchy == false)
        {
            textBox.SetActive(true);
            blockUiPanel.SetActive(true);
            textBox.transform.GetComponent<TextBox>().nextText();
        }
    }
    public static void addTextFirst(string text)
    {
        stack.AddFirst(new AddSimpleText(text));
        if (textBox.activeInHierarchy == false)
        {
            textBox.SetActive(true);
            blockUiPanel.SetActive(true);
            textBox.transform.GetComponent<TextBox>().nextText();
        }
    }
    public static void doNextAction()
    {
        textBox.transform.GetComponent<TextBox>().nextText();
    }
    public static void addActionFirst(TextBoxAction a)
    {
        stack.AddFirst(a);
        if (textBox.activeInHierarchy == false)
        {
            textBox.SetActive(true);
            blockUiPanel.SetActive(true);
            textBox.transform.GetComponent<TextBox>().nextText();
        }
    }
    public static void addActionLast(TextBoxAction a)
    {
        stack.AddLast(a);
        if (textBox.activeInHierarchy == false)
        {
            textBox.SetActive(true);
            blockUiPanel.SetActive(true);
            textBox.transform.GetComponent<TextBox>().nextText();
        }
    }
    public void nextText()
    {
        if (stack.Count == 0)
        {
            blockUiPanel.SetActive(false);
            textBox.SetActive(false);
        }
        else
        {
            TextBoxAction a = stack.First.Value;
            stack.RemoveFirst();
            a.doAction(this);
        }
    }
    public void writeText(string text)
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(writeTextCoro(text));
    }
    private IEnumerator writeTextCoro(string text)
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
public abstract class TextBoxAction
{
    protected TextBox textBox;
    public abstract void doAction(TextBox textBox);
}
public class AddSimpleText : TextBoxAction
{

    private string text;
    public AddSimpleText(string text)
    {
        this.textBox = textBox;
        this.text = text;
    }
    public override void doAction(TextBox textBox)
    {
        textBox.writeText(text);
    }

}