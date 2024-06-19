using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class MInput : MonoBehaviour
{
    public static MInput inst;

    public static InputType inputType { get; private set; }

    private static List<RaycastResult> raycastResults = new List<RaycastResult>();
    public static float imguiRectSetTime;
    public static Rect? imguiRect;

    public void Init()
    {
        inst = this;

        InvokeRepeating("SlowTick", 0f, 1f);
    }

    public void Tick()
    {

    }

    void SlowTick()
    {
        DetermineInputType();
    }

    void DetermineInputType()
    {
#if UNITY_ANDROID || UNITY_IPHONE
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isRemoteConnected) {
            inputType = InputType.Touch;
        } else {
            inputType = InputType.KeyboardMouse;
        }
#else
            inputType = InputType.Touch;
#endif
#else
        inputType = InputType.KeyboardMouse;
#endif
    }

    public static bool GetTouchByFingerId(int fingerID, out Touch touch)
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.touches[i].fingerId == fingerID)
            {
                touch = Input.touches[i];
                return true;
            }
        }

        touch = default;
        return false;
    }

    public static bool GetTouchClosestToPosition(Vector2 position, out Touch touch)
    {
        if (Input.touchCount > 0)
        {
            touch = Input.touches.OrderBy(x => (position - x.position).magnitude).FirstOrDefault();
            return true;
        }
        else
        {
            touch = default;
            return false;
        }
    }

    public static bool IsPointerOverUIObject()
    {
        return IsPointerOverUIObject(Input.mousePosition);
    }

    public static bool IsPointerOverUIObject(Vector2 position)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = position;
        EventSystem.current.RaycastAll(eventDataCurrentPosition, raycastResults);

        return raycastResults.Count > 0;
    }

    public static bool IsVerySmallScreenDistance(Vector2 start, Vector2 end)
    {
        Vector2 dd = (end - start);
        float minDimension = Mathf.Min(Screen.width, Screen.height);
        return dd.magnitude < minDimension * 0.01f;
    }

}

public enum InputType
{
    KeyboardMouse,
    Touch,
}