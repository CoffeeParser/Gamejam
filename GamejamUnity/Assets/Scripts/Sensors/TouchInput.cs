using UnityEngine;

public class TouchInput : MonoBehaviour {

    public Touch[] touches;
    public int touchCount;
    public float dragInterval;
    private float dragTime;

    public EventInt OnTouchCountChanged;
    public EventTouch OnTouchUp;
    public EventScreenPosition OnTapPositionUp;
    public EventTouch OnTouchDown;
    public EventTouch OnDrag;
    public EventTouch OnMove;

    // Use this for initialization
    void Awake () {
        if(OnTouchCountChanged == null)
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
    }
	
	// Update is called once per frame
	void Update () {

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
            if (tap.phase == TouchPhase.Moved)
            {
                OnMove.Invoke(tap);
            }
            if ((tap.phase == TouchPhase.Moved || tap.phase == TouchPhase.Stationary) && dragTime >= dragInterval)
            {
                OnDrag.Invoke(tap);
            }

            dragTime += Time.deltaTime;
        } else if (Input.GetMouseButtonUp(0))
        {
            OnTapPositionUp.Invoke(Input.mousePosition);
        }
	}
}
