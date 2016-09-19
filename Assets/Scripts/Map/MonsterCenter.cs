using UnityEngine;
using System.Collections;

public class MonsterCenter : MonoBehaviour
{
   
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Check for left mouse button
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Vector2 position;

            if (Input.touchSupported)
            {
                // Use touch position when supported by the device
                position = Input.GetTouch(0).position;
            }
            else
            {

                // Use mouse position as a fallback
                position = Input.mousePosition;
            }

            Ray ray = Camera.main.ScreenPointToRay(position);
            RaycastHit hit;

            // Do a raycast, the max size here needs to be better defined.
            if (Physics.Raycast(ray, out hit, 300.0f))
            {
                Player p = GameObject.FindObjectOfType<Player>();
                float distance = Mathf.Sqrt(Mathf.Pow(p.transform.position.x - transform.position.x, 2.0f) + Mathf.Pow(p.transform.position.z - transform.position.z, 2.0f));
                if (hit.transform.IsChildOf(transform) && distance < 10)
                {
                    GuiManager.guiManager.showElementNoButton(GuiManager.guiManager.monsterCenterView);
                }
            }
        }
    }
}

