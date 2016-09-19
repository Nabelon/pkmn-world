using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GridLayoutScale : MonoBehaviour {
    public int horizontalElements = 2;
    public int verticalElements = 3;
	// Use this for initialization
	void Awake () {

        GridLayoutGroup grid = gameObject.GetComponent<GridLayoutGroup>();
        RectTransform rect = grid.GetComponent<RectTransform>();
        int x = (int)(rect.rect.width - grid.spacing.x * (horizontalElements - 1))/horizontalElements - grid.padding.left - grid.padding.right;
        int y = (int)(rect.rect.height - grid.spacing.y * (verticalElements-1))/verticalElements - grid.padding.top - grid.padding.bottom;
        grid.cellSize = new Vector2(x, y);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
