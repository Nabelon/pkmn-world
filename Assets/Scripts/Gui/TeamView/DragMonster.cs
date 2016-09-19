using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace teamView
{
    public class DragMonster : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {

        private static int posStartDrag;
        private static GameObject draggedImage;

        public void OnBeginDrag(PointerEventData eventData)
        {
            posStartDrag = gameObject.GetComponent<InfoSetter>().posTeam;
            draggedImage = new GameObject("DraggedMonster");
            draggedImage.transform.SetParent(GuiManager.guiManager.transform);
            draggedImage.transform.localScale = new Vector3(3, 3, 3);
            Image i = draggedImage.AddComponent<Image>();
            Sprite sprite = (Sprite)Resources.Load<Sprite>("MonsterData/icons/" + bag.MonsterBag.getBag().getTeam()[posStartDrag].id);
            //draggedImage.GetComponent<CanvasGroup>().blocksRaycasts = false;
            i.overrideSprite = sprite;
            i.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
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
                draggedImage.transform.position = position;
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
          GameObject.Destroy(draggedImage);
        }

        public void OnDrop(PointerEventData eventData)
        {
            int newPos = gameObject.GetComponent<InfoSetter>().posTeam;
            Debug.Log("droped");
            bag.MonsterBag.getBag().swapMonsters(newPos, posStartDrag);
        }
    }
}