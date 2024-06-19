using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler
{
    public RectTransform disc;
    public RectTransform dot;

    Canvas canvas;
    CanvasGroup canvasGroup;
    Vector3[] corners = new Vector3[4];

    bool isPressed;
    Vector2 pressWorldPos;
    Touch? trackedTouch = null;
    bool isTouchBased => MInput.inputType == InputType.Touch;

    public event Action<JoystickEvent, Vector2> OnEvent;
    public Vector2 value { get; private set; }
    public bool hasInput => isPressed;

    Vector2 startAnchoredPosition;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponentInParent<CanvasGroup>();

        startAnchoredPosition = disc.anchoredPosition;
    }

    public void Init()
    {

    }

    private void Update()
    {
        if (isPressed)
        {
            bool released = false;
            Vector2 pointerPos = Vector2.zero;

            if (isTouchBased)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.touches[i];

                    if (touch.fingerId == trackedTouch.Value.fingerId)
                    {
                        pointerPos = touch.position;

                        if (touch.phase == TouchPhase.Ended)
                        {
                            released = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                pointerPos = Input.mousePosition;

                if (Input.GetMouseButtonUp(0))
                {
                    released = true;
                }
            }

            disc.position = pressWorldPos;
            disc.GetWorldCorners(corners);

            Vector2 center = (corners[0] + corners[2]) * 0.5f;
            float radius = (corners[2].x - corners[0].x) * 0.5f;

            Vector2 dd = (pointerPos - center);
            Vector2 clampedPos = center + dd.normalized * Mathf.Clamp(dd.magnitude, 0f, radius);
            Vector2 normalizedPos = dd.normalized * Mathf.Clamp01(dd.magnitude / radius);

            value = normalizedPos;

            dot.position = clampedPos;

            if (released)
            {
                isPressed = false;
                value = Vector2.zero;

                //disc.gameObject.SetActive(false);
                //dot.gameObject.SetActive(false);
                ResetPosition();

                OnEvent?.Invoke(JoystickEvent.End, normalizedPos);
            }
            else
            {
                OnEvent?.Invoke(JoystickEvent.Update, normalizedPos);
            }
        }
    }

    public void Show(bool show)
    {
        disc.gameObject.SetActive(show);
        dot.gameObject.SetActive(show);
    }

    void ResetPosition()
    {
        disc.anchoredPosition = startAnchoredPosition;
        dot.position = disc.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Press");

        pressWorldPos = eventData.position;
        disc.position = pressWorldPos;

        if (isTouchBased)
        {
            MInput.GetTouchClosestToPosition(eventData.position, out var newTouch);
            trackedTouch = newTouch;
        }

        isPressed = true;

        disc.gameObject.SetActive(true);
        dot.gameObject.SetActive(true);

        value = Vector2.zero;
        OnEvent?.Invoke(JoystickEvent.Start, Vector2.zero);
    }

    void OnDisable()
    {
        isPressed = false;
    }

    public void SetInteractable(bool interactable)
    {
        canvasGroup.blocksRaycasts = interactable;
    }

    public enum JoystickEvent
    {
        None,
        Start,
        Update,
        End
    }

}
