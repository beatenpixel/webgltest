using UnityEngine;

public class App : MonoBehaviour
{
    public Game game;

    private void Awake()
    {
        game.Init();
    }

    private void Update()
    {
        game.Tick();
    }

    private void OnApplicationQuit()
    {
        game.Stop();
    }

    private void OnApplicationFocus(bool focus)
    {
        
    }

}
