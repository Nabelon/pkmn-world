using UnityEngine;
using System.Collections;
namespace map
{
    public class Monster : MonoBehaviour
    {
        public string monsterName;
        public string id;
        public Color color;
        public void initiate(string id)
        {
            this.id = id;
            monsterName = MonsterInfo.getMonsterInfo().info[id]["name"];
            string type = MonsterInfo.getMonsterInfo().info[id]["type"];
            switch (type)
            {
                case "fire":
                    color = new Color(255, 0, 0); break;
                case "water":
                    color = new Color(0, 0, 255); break;
                case "grass":
                    color = new Color(0, 255, 0); break;
                case "normal":
                    color = new Color(255, 255, 255); break;
                default:
                    color = new Color(0, 0, 0);
                    break;
            }
        }
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
                    if (hit.transform == transform && distance < 20)
                    {

                        Debug.Log("hit");

                    }
                }
            }

        }

    }
}