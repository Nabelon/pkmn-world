using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace boxview
{
    public class InfoSetter : MonoBehaviour
    {
        public int posBox;
        bag.Monster monster;
        bag.MonsterBag mBag;
        // Use this for initialization
        public void setImages()
        {
            mBag = bag.MonsterBag.getBag();
            bag.Monster m = mBag.getBox()[posBox];
            if (m == null)
            {
                gameObject.SetActive(false);
                return;
            }

            Image monsterImage = transform.Find("MonsterImage").gameObject.GetComponent<Image>();
            Text description = transform.Find("Description").gameObject.GetComponent<Text>();

            description.text = m.name + "\nLevel: " + m.mLevel.ToString();
            Sprite sprite = (Sprite)Resources.Load<Sprite>("MonsterData/icons/" + m.id);
            monsterImage.overrideSprite = sprite;
            monster = m;
        }
        // Update is called once per frame
        void Update()
        {
            /*
            if (mBag.getBox().Count < posBox) { GameObject.Destroy(gameObject); return; }
            if (mBag.getBox()[posBox].Equals(monster)) return;
            if (posBox == 0) { GameObject.Destroy(gameObject); return; }
            if (mBag.getBox()[posBox - 1].Equals(monster))
            {
                posBox--;
                Awake();
                return;
            }
            GameObject.Destroy(gameObject);

       
        }*/
        }
    }
}