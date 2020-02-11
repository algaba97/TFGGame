using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnLogic : MonoBehaviour
{
    private Transform _enemiesTransform;
    private Transform _basePostion;
    private Transform _playerPosition;

    private System.Action<EnemyController> _onKillAction;
    private int _spawnID;
    // Start is called before the first frame update
    public void Init(int spawnID,Transform enemyTransform, Transform basePosition, Transform playerPosition, System.Action<EnemyController> onKillAction = null){
        _spawnID  = spawnID;
        _basePostion = basePosition;
        _enemiesTransform = enemyTransform;
        _playerPosition = playerPosition;
        _onKillAction = onKillAction;
    }

    public void SpawnEnemy(GameObject enemyToSpawn,int level = 0){
        EnemyController enemy = Instantiate(enemyToSpawn,transform.position,Quaternion.identity,_enemiesTransform).GetComponent<EnemyController>();
        enemy.Init(_basePostion,_playerPosition,_spawnID,level,_onKillAction);

    }
    
}
