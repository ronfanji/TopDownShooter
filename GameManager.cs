using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public bool isPlaying = false;
    public UnityEvent onPlay = new UnityEvent();
    public UnityEvent onGameOver = new UnityEvent();

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {

        }
    }

    public void StartGame()
    {
        onPlay.Invoke();
        isPlaying = true;
    }
    public void GameOver()
    {
        onGameOver.Invoke();
        isPlaying = false;
    }
}
