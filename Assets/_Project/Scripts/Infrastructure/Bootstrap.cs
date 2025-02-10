using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    public Game game;

    private void Awake()
    {
        game = new Game(new Services());
        DontDestroyOnLoad(gameObject);
    }
}
