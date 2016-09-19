using UnityEngine;
using System.Collections;
namespace map
{
    public class Thief : MonoBehaviour
    {
        public string name;
        public bag.Monster[] team = new bag.Monster[6];
        public void initiate(string[] monsters, int avrLevel)
        {
            team[0] = new bag.Monster(monsters[0], avrLevel);
        }
        void Update()
        {
            Player p = GameObject.FindObjectOfType<Player>();
            float distance = Mathf.Sqrt(Mathf.Pow(p.transform.position.x - transform.position.x, 2.0f) + Mathf.Pow(p.transform.position.z - transform.position.z, 2.0f));
            if (distance > 90.0f)
            {
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
                    if (hit.transform.IsChildOf(transform) && distance < 5)
                    {
                        fight.FightingManager.addTeam("attacker", bag.MonsterBag.getBag().getTeam());
                        fight.FightingManager.addTeam("defender", team);
                        fight.FightingManager.setTrainerFight(true);
                        Destroy(gameObject);
                        UnityEngine.SceneManagement.SceneManager.LoadScene("FightingScene");
                    }
                }
            }
        }
        void OnDestroy()
        {
            Spawner s = GameObject.FindObjectOfType<Spawner>();
            if (s != null)
            {
                s.monsterDespawned();
            }
        }

    }
}