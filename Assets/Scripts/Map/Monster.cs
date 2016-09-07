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
                    color = new Color(255, 82, 0); break;
                case "water":
                    color = new Color(0, 115, 255); break;
                case "grass":
                    color = new Color(91, 255, 0); break;
                case "normal":
                    color = new Color(255, 255, 255); break;
                case "fighting":
                    color = new Color(132, 20, 20); break;
                case "flying":
                    color = new Color(187, 161, 204); break;
                case "poison":
                    color = new Color(125, 38, 182); break;
                case "electric":
                    color = new Color(182, 171, 38); break;
                case "ground":
                    color = new Color(118, 113, 52); break;
                case "psychic":
                    color = new Color(238, 40, 245); break;
                case "rock":
                    color = new Color(86, 80, 4); break;
                case "ice":
                    color = new Color(125, 229, 229); break;
                case "bug":
                    color = new Color(111, 159, 68); break;
                case "dragon":
                    color = new Color(55, 36, 111); break;
                case "ghost":
                    color = new Color(91, 71, 156); break;
                case "dark":
                    color = new Color(71, 13, 13); break;
                case "steel":
                    color = new Color(94, 94, 94); break;
                case "fairy":
                    color = new Color(202, 122, 223); break;
                default:
                    color = new Color(0, 0, 0);
                    break;
            }
        }
        void Update()
        {
            Player p = GameObject.FindObjectOfType<Player>();
            float distance = Mathf.Sqrt(Mathf.Pow(p.transform.position.x - transform.position.x, 2.0f) + Mathf.Pow(p.transform.position.z - transform.position.z, 2.0f));
            if (distance > 90.0f)
            {
                Debug.Log("destroyed");
                GameObject.FindObjectOfType<Spawner>().monsterDespawned();
                Destroy(gameObject);
            }
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
                    if (hit.transform == transform && distance < 5)
                    {
                        bag.MonsterBag.getBag().addMonster(new bag.Monster(id));
                        GameObject.FindObjectOfType<Spawner>().monsterDespawned();
                        Destroy(gameObject);

                    }
                }
            }

        }

    }
}