using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI inst;

    public Joystick joystick;

    public void Init()
    {
        inst = this;


    }
}
