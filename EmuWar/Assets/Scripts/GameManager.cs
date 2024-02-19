using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    [DoNotSerialize] public ObjectPooler<GameObject> Pool;
    [SerializeField] private GameObject bullet;
    private bool gameIsPaused = false;

    public void Awake()
    {
        Pool = new();
        Instance = this;
    }

    public void Start()
    {
        Pool.CreateNewPool(ObjectList.BULLET, bullet);
    }

    /// <summary>
    /// Pause or unpause the game. Assign this to any button or UIElement you want to control the pause state.
    /// </summary>
    public void PauseGame() 
    {
        gameIsPaused = !gameIsPaused;
        ChangeFocus();
        Time.timeScale = gameIsPaused ?  0.0f : 1.0f;
    }

    /// <summary>
    /// Sets the cursor to visible if the game is paused.
    /// </summary>
    private void ChangeFocus()
    {
        Cursor.lockState = gameIsPaused ? CursorLockMode.Confined : CursorLockMode.Locked; 
    }


}
