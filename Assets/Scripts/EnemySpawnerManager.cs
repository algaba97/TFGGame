using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    private EnemySpawnLogic[] _spawners;
    public Transform basePosition;

    public Transform playerPosition;
    public Transform enemyList;

    public GameManager gameManager;

    public int _enemiesToSpawn;

    public int _enemiesPerWave;

    public float _waveRate;

    public int _levelRound;

    public GameObject[] enemiesPrefab;

    public bool _canSpawn;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        _spawners = GetComponentsInChildren<EnemySpawnLogic>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        IntializeSpawners();
        _canSpawn = true;
    }

    public void StopSpawns(){
        _canSpawn = false;
    }

    private void Update() {
    }

    private void SpawnRandom(){
        _spawners[ Random.Range(0,_spawners.Length)].SpawnEnemy(enemiesPrefab[ Random.Range(0,enemiesPrefab.Length)],_levelRound);
        
    }

    private void IntializeSpawners(){
        int spawnerID = 0;
        foreach(EnemySpawnLogic spawn in _spawners){
            spawn.Init(spawnerID,enemyList,basePosition,playerPosition,gameManager.OnEnemyKill);
            spawnerID++;
        }
    }

    public void SetSpawnParameters(int enemiesToSpaw, int enemiesPerWave, float waveRate,int level = 0){
        _enemiesToSpawn = enemiesToSpaw;
        _enemiesPerWave = enemiesPerWave;
        _waveRate = waveRate;
        _levelRound = level;
    }
    public void StartRound(){
        StartCoroutine(SpawnEnemiesCoroutine());
    }

    private IEnumerator CreateWaveCoroutine(){
        for (int i = 0; i < _enemiesPerWave; i++)
        {
            if(_enemiesToSpawn > 0){
                SpawnRandom();
                _enemiesToSpawn--;
            }
            else
                break;
            yield return new WaitForEndOfFrame();

        }
    }
    private IEnumerator SpawnEnemiesCoroutine(){
        while(_enemiesToSpawn > 0 && _canSpawn){
            StartCoroutine(CreateWaveCoroutine());
            yield return new WaitForSeconds(_waveRate);
        }
    }
}
