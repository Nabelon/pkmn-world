using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace teamView
{
    public class InfoSetter : MonoBehaviour
    {
        public int posTeam;

        // Use this for initialization
        void Awake()
        {
            bag.Monster m = bag.MonsterBag.getBag().getTeam()[posTeam];
            if (m == null)
            {
                gameObject.SetActive(false);
                return;
            }
                
            Image monsterImage = transform.Find("MonsterImage").gameObject.GetComponent<Image>();
            Text description = transform.Find("Description").gameObject.GetComponent<Text>();
            Image hpBar = transform.Find("HpBar").gameObject.GetComponent<Image>();

            description.text = m.name + "\nId: " + m.id + "\nLevel: " + m.mLevel.ToString() + "\nNature: " + m.nature;
            Sprite sprite = (Sprite)Resources.Load<Sprite>("MonsterData/icons/" + m.id);
            monsterImage.overrideSprite = sprite;

            hpBar.fillAmount = m.mCurrHp / (float)m.getMMaxHp();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}