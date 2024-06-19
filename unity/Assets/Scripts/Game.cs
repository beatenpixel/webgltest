using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game inst;

    public UI ui;

    public Player player;

    public void Init()
    {
        inst = this;

        ui.Init();

        player.Init();        
    }

    public void Tick()
    {
        player.Tick();
    }

    public void Stop()
    {

    }

}
