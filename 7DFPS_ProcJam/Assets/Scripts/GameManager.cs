﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    public float Max_Map_Bounds = 80.0f; //Bounds on the map on all sides

    private SpaceGenerator spaceGen = null;
    private SpawnManager spawnManager = null;

    private int playerNumKills = 0;
    private int playerHull = 100;
    private int numEnemiesInScene = 0;
    private int prevNumEnemies = 0;

    private bool isGamePaused = false;
    private bool isGameOver = false;

    public delegate void GameEvent();

    public GameEvent PlayerTakeDamage;
    public GameEvent GameRestart;

    public int PlayerNumKills
    {
        get { return playerNumKills; }
        set { playerNumKills = value; }
    }

    public int PlayerHull
    {
        get { return playerHull; }
        set { playerHull = value; }
    }

    public int NumEnemiesInScene
    {
        get { return numEnemiesInScene; }
        set { numEnemiesInScene = value; }
    }

    public bool IsGamePaused
    {
        get { return isGamePaused; }
        set { isGamePaused = value; }
    }

    public bool IsGameOver
    {
        get { return isGameOver; }
        set { isGameOver = value; }
    }

	// Use this for initialization
	void Start () 
    {
        spaceGen = SpaceGenerator.Instance;
        spaceGen.SpaceContainer = GameObject.FindGameObjectWithTag("Space");

        spaceGen.GenerateSpace();

        spawnManager = SpawnManager.Instance;
        spawnManager.GameManager = this;
        spawnManager.EstablishSpawnPoints();
	}
	
	// Update is called once per frame
	void Update () 
    {
        UpdateEnemyCount();

        if (spawnManager != null)
        {
            spawnManager.Update();
        }
	}

    private void UpdateEnemyCount()
    {
        numEnemiesInScene = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if(prevNumEnemies != numEnemiesInScene)
            prevNumEnemies = numEnemiesInScene;
    }

    public void ApplyPlayerDamage(int damageValue)
    {
        if (playerHull > 0)
        {
            playerHull -= damageValue;

            if (PlayerTakeDamage != null)
                PlayerTakeDamage();
        }
    }

    public void RestartGame()
    {
        if (GameRestart != null)
            GameRestart(); //Fire this event to all objects that need to unsub from events.

        Application.LoadLevel(0);
    }
}
