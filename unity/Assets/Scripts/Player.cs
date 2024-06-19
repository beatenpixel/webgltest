using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player inst;

    public float moveSpeed = 5f;
    public float rotationOffset = 90f;

    public Transform rootT;
    Vector2 input;

    public void Init()
    {
        inst = this;

        UI.inst.joystick.OnEvent += Joystick_OnEvent;
    }

    private void Joystick_OnEvent(Joystick.JoystickEvent e, Vector2 v)
    {
        input = v;

        if(e == Joystick.JoystickEvent.End)
        {
            input = Vector2.zero;
        }
    }

    public void Tick()
    {
        rootT.position += new Vector3(input.x, 0, input.y) * Time.deltaTime * moveSpeed;

        float angle = Mathf.Atan2(input.y, input.x);

        if (input.sqrMagnitude > Mathf.Epsilon)
        {
            rootT.rotation = Quaternion.Euler(0, rotationOffset - angle * Mathf.Rad2Deg,0);
        }
    }

}
