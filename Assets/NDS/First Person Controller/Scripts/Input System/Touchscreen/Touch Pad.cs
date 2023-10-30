/*Copyright © Non-Dynamic Studio*/
/*2023*/

using Unity.Burst;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NDS.InputSystem.DragControls
{
    [BurstCompile]
    public class TouchPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        #region Variables
        [HideInInspector] public Vector2 TouchDist;
        [HideInInspector] public Vector2 PointerOld;
        [HideInInspector] public int PointerId;
        [HideInInspector] public bool Pressed;
        #endregion
        #region MonoBehaviour Callbacks
        // Update is called once per frame
        private void Update()
        {
            HandleTouch();
        }
        public void OnPointerDown(PointerEventData eventData)
        {

            HandlePointerDown(eventData);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            HandlePointerUp();
        }
        #endregion
        #region Private Methods
        private void HandlePointerUp()
        {
            Pressed = false;
        }
        private void HandleTouch()
        {
            if (Pressed)
            {
                if (PointerId >= 0 && PointerId < Input.touches.Length)
                {
                    TouchDist = Input.touches[PointerId].position - PointerOld;
                    PointerOld = Input.touches[PointerId].position;
                }
                else
                {
                    TouchDist = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - PointerOld;
                    PointerOld = Input.mousePosition;
                }
            }
            else
            {
                TouchDist = new Vector2();
            }
        }
        private void HandlePointerDown(PointerEventData eventData)
        {
            Pressed = true;
            PointerId = eventData.pointerId;
            PointerOld = eventData.position;
        }
        #endregion
    }

}
