using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FloatingJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private CanvasScaler CanvasScaler;
    [SerializeField] private Image BackgroundImage;
    [SerializeField] private Image JoystickMovingImage;
    [SerializeField] private TimeScaleManager TimeScaleManager;
    
    private Vector3[] FourCornersArray = new Vector3[4];
    private Vector2 BackgroundImageStartPosition;
    private Vector2 BackgroundImageStartAnchoredPosition;

    public Vector2 DirectionVector { get; private set; }

    private void Awake()
    {
        BackgroundImageStartAnchoredPosition = new Vector2(((float)Screen.width / Screen.height * CanvasScaler.referenceResolution.y + BackgroundImage.rectTransform.sizeDelta.x) * 0.5f, BackgroundImage.rectTransform.anchoredPosition.y);
        BackgroundImage.rectTransform.anchoredPosition = BackgroundImageStartAnchoredPosition;
        BackgroundImage.rectTransform.GetWorldCorners(FourCornersArray);
        BackgroundImageStartPosition = FourCornersArray[3];
        BackgroundImage.rectTransform.pivot = new Vector2(1, 0);
        BackgroundImage.rectTransform.position = BackgroundImageStartPosition;
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        if (!TimeScaleManager.IsGameStarted) return;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(BackgroundImage.rectTransform, Input.touches[^1].position, ped.pressEventCamera, out Vector2 localPoint))
        {
            localPoint = new(localPoint.x / BackgroundImage.rectTransform.sizeDelta.x, localPoint.y / BackgroundImage.rectTransform.sizeDelta.y);
            DirectionVector = new Vector3(localPoint.x * 2 + 1, localPoint.y * 2 - 1);
            if (DirectionVector.magnitude > 1.0f)
            {
                DirectionVector = DirectionVector.normalized;
            }
            JoystickMovingImage.rectTransform.anchoredPosition = Vector2.Scale(DirectionVector, BackgroundImage.rectTransform.sizeDelta) / 2;
        }
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        DirectionVector = Vector3.zero;
        JoystickMovingImage.rectTransform.anchoredPosition = Vector3.zero;
        ReturnJoystick();
    }

    public virtual void OnPointerDown(PointerEventData ped) { }

    private void Update()
    {
        if (!TimeScaleManager.IsGameStarted) return;
        if (Input.touchCount > 0)
        {
            Touch[] Touches = Input.touches;
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Touches[i].phase == TouchPhase.Began)
                {
                    Vector2 touchPosition = Touches[i].position / Screen.height * CanvasScaler.referenceResolution.y;
                    Vector2 currentPosition = touchPosition + Vector2.Scale(new Vector2(0.5f, -0.5f), BackgroundImage.rectTransform.sizeDelta);
                    BackgroundImage.rectTransform.anchoredPosition = currentPosition;
                }
            }
        }
    }

    public void ReturnJoystick()
    {
        BackgroundImage.rectTransform.anchoredPosition = BackgroundImageStartAnchoredPosition;
    }
}