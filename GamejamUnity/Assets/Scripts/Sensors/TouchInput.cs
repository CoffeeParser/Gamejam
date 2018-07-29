using UnityEngine;

namespace MobileSensors
{
    /// <summary>
    /// Class for accessing the touch input of your mobile device.
    /// </summary>
    public class TouchInput : MonoBehaviour
    {

        public Touch[] touches;
        public int touchCount;
        public float dragInterval;
        private float dragTime;

        /// <summary>
        /// Event for touchCountChange
        /// </summary>
        public EventInt OnTouchCountChanged;
        /// <summary>
        /// Event for Touch up
        /// </summary>
        public EventTouch OnTouchUp;
        /// <summary>
        /// Event for Touch Up and retreive Position
        /// </summary>
        public EventScreenPosition OnTapPositionUp;
        /// <summary>
        /// Event for touch down
        /// </summary>
        public EventTouch OnTouchDown;
        /// <summary>
        /// Event for Drag
        /// </summary>
        public EventTouch OnDrag;
        /// <summary>
        /// Event for Move
        /// </summary>
        public EventTouch OnMove;
        /// <summary>
        /// Event for Multi touch move
        /// </summary>
        public EventMultiTouch OnMultiMove;
        /// <summary>
        /// Event for multi touch up
        /// </summary>
        public EventMultiTouch OnMultiMoveUp;

        // Use this for initialization
        void Awake()
        {
            if (OnTouchCountChanged == null)
                OnTouchCountChanged = new EventInt();

            if (OnTouchDown == null)
                OnTouchDown = new EventTouch();

            if (OnTouchUp == null)
                OnTouchUp = new EventTouch();

            if (OnDrag == null)
                OnDrag = new EventTouch();

            if (OnTapPositionUp == null)
                OnTapPositionUp = new EventScreenPosition();

            if (OnMove == null)
                OnMove = new EventTouch();

            if (OnMultiMove == null)
                OnMultiMove = new EventMultiTouch();

            if (OnMultiMoveUp == null)
                OnMultiMoveUp = new EventMultiTouch();
        }

        // Update is called once per frame
        void Update()
        {

            if (touchCount != Input.touchCount)
            {
                touchCount = Input.touchCount;
                OnTouchCountChanged.Invoke(touchCount);
            }

            touches = Input.touches;

            if (touchCount == 1)
            {

                Touch tap = Input.touches[0];

                if (tap.phase == TouchPhase.Began && dragTime < dragInterval)
                {
                    OnTouchDown.Invoke(tap);
                    dragTime = 0;
                }
                if (tap.phase == TouchPhase.Ended)
                {
                    OnTapPositionUp.Invoke(tap.position);
                    OnTouchUp.Invoke(tap);
                    dragTime = 0;
                }
                if (tap.phase == TouchPhase.Moved && dragTime < dragInterval)
                {
                    OnMove.Invoke(tap);
                    dragTime = 0;
                }
                if ((tap.phase == TouchPhase.Moved || tap.phase == TouchPhase.Stationary) && dragTime >= dragInterval)
                {
                    OnDrag.Invoke(tap);
                }

                dragTime += Time.deltaTime;
            }
            else if (touchCount > 1)
            {
                Touch tap = Input.touches[0];
                if (tap.phase == TouchPhase.Moved)
                {
                    OnMultiMove.Invoke(Input.touches);
                }
                if (tap.phase == TouchPhase.Ended)
                {
                    OnMultiMoveUp.Invoke(Input.touches);
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                OnTapPositionUp.Invoke(Input.mousePosition);
            }
        }
    }
}