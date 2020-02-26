﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public InformationDisplayController informationDisplay;
    public PlayerController player;
    public BaseLogic baseLogic;
    public EnemySpawnerManager spawnerManager;

    public EnemyListController enemyList;

    public List<OnRoundListener> onRoundListeners;

    public List<OnEnemyKillListener> onEnemyKillListeners;

    public AgentPlayer agent;

    private int _roundNumber;

    public int roundNumber
    {
        get { return _roundNumber; }
    }

    private int _pendingEnemies;

    public int pendingEnemies
    {
        get { return _pendingEnemies; }
    }

    private int _killedEnemies;

    public int killedEnemies
    {
        get { return _killedEnemies; }
    }

    public bool GameOver;

    public bool GameStarted;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        onEnemyKillListeners = new List<OnEnemyKillListener>();
        onRoundListeners = new List<OnRoundListener>();
        _roundNumber = 0;
        _killedEnemies = 0;
        GameOver = false;
        agent.Init(this);
        GameStarted = false;
        informationDisplay.DisplayText("Press any key");
    }

    private void Update()
    {
        if (!GameStarted)
        {
            if (player.isMoving)
            {
                StartGame();
            }
        }
        if (GameOver)
        {
            EndGame();
            StartGame();
        }
    }

    private void StartGame()
    {
        GameStarted = true;
        baseLogic.Init(OnDestroyBase);
        player.Init(OnDestroyPlayer);
        enemyList.DestroyAllEnemies();
        _roundNumber = 0;
        _killedEnemies = 0;
        GameOver = false;
        OnRoundStart();
    }

    private void CalculateRound()
    {
        int enemiesToSpawn = roundNumber;
        int enemiesPerWave = 1 + (int)System.Math.Log(roundNumber, 3);
        float waveRate = 5.0f;

        int levelRound = 1 + roundNumber / 5;

        _pendingEnemies = enemiesToSpawn;
        spawnerManager.SetSpawnParameters(enemiesToSpawn, enemiesPerWave, waveRate, levelRound);
    }

    private void OnRoundFinished()
    {
        if (!GameOver)
        {
            foreach (var l in onRoundListeners)
            {
                l.OnRoundOver(_roundNumber);
            }

            OnRoundStart();
        }
    }

    private void OnRoundStart()
    {
        _roundNumber++;
        foreach (var l in onRoundListeners)
        {
            l.OnRoundStart(_roundNumber);
        }
        CalculateRound();
        spawnerManager.StartRound();
        informationDisplay.DisplayText("Round " + _roundNumber, 3.0f);

    }

    public void OnEnemyKill(EnemyController enemy)
    {
        _pendingEnemies--;
        if (_pendingEnemies < 1)
        {
            OnRoundFinished();
        }
        _killedEnemies++;
        if (CheckAvialabilityPowerUp())
        {
            CreatePowerUp(enemy.transform.position);
        }
        foreach (var l in onEnemyKillListeners)
        {
            l.OnEnemyKill(enemy);
        }
    }
    public List<GameObject> PowerUpList;
    private int _modulePowerUp = 3;
    private bool CheckAvialabilityPowerUp()
    {
        if (_killedEnemies % _modulePowerUp == 0)
        {
            return true;
        }
        return false;
    }

    private void CreatePowerUp(Vector3 position)
    {
        Instantiate(PowerUpList[Random.Range(0, 3)], position, Quaternion.identity, transform);
    }

    private void EndGame()
    {
        GameOver = true;
        spawnerManager.StopSpawns();
        agent.Done();
    }

    private void OnDestroyBase(BaseLogic baseLogic)
    {
        EndGame();
        informationDisplay.DisplayText("Base Destroyed \n Game Over \n Press R");
    }

    private void OnDestroyPlayer(PlayerController player)
    {
        EndGame();
        informationDisplay.DisplayText("Base Destroyed \n Game Over \n Press R");

    }

}
