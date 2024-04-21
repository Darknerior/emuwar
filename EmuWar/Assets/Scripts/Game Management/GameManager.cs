using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    [DoNotSerialize] public ObjectPooler Pool;
    [SerializeField] private GameObject bullet, enemy, cagedEmu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private float timeBetweenBaseSpawns;
    private bool gameIsPaused = false;
    public GameObject player;
    public BaseController Base { get; private set; }
    public void Awake()
    {
        Pool = new();
        Instance = this;
    }
    public delegate void GameIsPaused(bool isPaused);
    private event GameIsPaused OnPaused;

    public void Start()
    {
        Base = new BaseController(timeBetweenBaseSpawns);
        // Find the player GameObject from the scene
        if (player == null) player = GameObject.FindGameObjectWithTag("Player");
        // Get all instantiated NPC prefabs in the scene
        var npcs = FindObjectsOfType<EmuNPC>();
        var enemies = FindObjectsOfType<Enemy>();

        // Set the player reference for each NPC prefab
        foreach (var npc in npcs) {
            npc.player = player;
        }
        
        foreach (var npc in enemies) {
            npc.player = player;
        }
        
        Pool.CreateNewPool(ObjectList.BULLET, bullet);
        Pool.CreateNewPool(ObjectList.ENEMY,enemy,5);
        Pool.CreateNewPool(ObjectList.CAGEDEMU,cagedEmu);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Escape)  || Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Backspace))
        {
            PauseGame();
        }
    }

    /// <summary>
    /// Pause or unpause the game. Assign this to any button or UIElement you want to control the pause state.
    /// </summary>
    public void PauseGame()
    {
        gameIsPaused = !gameIsPaused;
        ChangeFocus();
        Time.timeScale = gameIsPaused ? 0f : 1f; 
        pauseMenu.SetActive(gameIsPaused);
        OnPaused?.Invoke(gameIsPaused);
    }

    /// <summary>
    /// Sets the cursor to visible if the game is paused.
    /// </summary>
    private void ChangeFocus()
    {
        Cursor.lockState = gameIsPaused ? CursorLockMode.Confined : CursorLockMode.Locked;
    }

    public void BeginRoutine(IEnumerator routine) => StartCoroutine(routine);
    
    public void EndRoutine(IEnumerator routine) => StopCoroutine(routine);

    public void Subscribe(GameIsPaused action) => OnPaused += action;
    public void UnSubscribe(GameIsPaused action) => OnPaused -= action;

}
