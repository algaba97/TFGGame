using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : Hittable
{
    public EntityStats stats;

    public EntityStats intialstats;    

    private Vector3 intialPos;

    Rigidbody2D rigidBody;
    private LifeShapeController _lifeShapeController;

    AttackRangeController rangeController;

    GunController _gun;

    SortedDictionary<long, EnemyController> enemyDictionary;

    private System.Action<PlayerController> _onDestroyPlayerAction=null;

    private bool _alive;
    
    private bool _isMoving;
    public bool isMoving
    {
        get
        {
            return _isMoving;
        }
        set
        {
            _isMoving = value;
        }
    }

    private bool _canShoot = true;

    public bool canShoot
    {
        get
        {
            return _canShoot;
        }
    }

    private void OnShoot(GunController gun)
    {
        StartCoroutine(ReloadCouritine());
    }

    private IEnumerator ReloadCouritine()
    {
        _canShoot = false;
        yield return new WaitForSeconds(stats.fireRate.ValueFloat);
        _canShoot = true;
        CheckAndShoot();
    }

    private void CheckAndShoot()
    {
        if (CanShoot())
            ShootFirstEnemy();
    }

    private bool CanShoot()
    {
        return _canShoot && enemyDictionary.Count != 0;
    }

    private void ShootFirstEnemy()
    {
        _gun.Shoot(30.0f, GetFirstEnemy().transform.position, stats.strengh.Value,OnShoot);

    }




    public void OnRangeEnter(AttackRangeController rangeController, GameObject gO)
    {
        EnemyController enemy = gO.GetComponent<EnemyController>();
        if (enemy != null)
        {
            AddEnemy(enemy);
            CheckAndShoot();
        }
    }

    public void OnRangeExit(AttackRangeController rangeController, GameObject gO)
    {
        EnemyController enemy = gO.GetComponent<EnemyController>();
        if (enemy != null)
        {
            RemoveEnemy(enemy);
        }
    }

    private EnemyController GetFirstEnemy()
    {
        foreach (long key in enemyDictionary.Keys)
        {
            return enemyDictionary[key];
        }
        return null;
    }

    private void AddEnemy(EnemyController enemy)
    {
        if (!enemyDictionary.ContainsKey(enemy.enemyID))
        {
            enemyDictionary.Add(enemy.enemyID, enemy);
        }
    }

    private void RemoveEnemy(EnemyController enemy)
    {
        if (enemyDictionary.ContainsKey(enemy.enemyID))
            enemyDictionary.Remove(enemy.enemyID);
    }


    public override void Hit(){
        stats.currentLifes--;
        _lifeShapeController.UpdateShadow(stats.maxLifes,stats.currentLifes);
        if(stats.currentLifes < 1 ){
            if(_onDestroyPlayerAction !=null){
                _onDestroyPlayerAction(this);
            }
        }
    }

    public void Init(System.Action<PlayerController> OnDestroyPlayerAction = null){
        _onDestroyPlayerAction = OnDestroyPlayerAction;
        stats = intialstats;
        _lifeShapeController.UpdateShadow(stats.maxLifes,stats.currentLifes);
        _alive =true;
        enemyDictionary.Clear();
        transform.position = intialPos;
    }

    public void ResetPlayer(){
        stats = intialstats;
    }

    // Start is called before the first frame update
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        intialstats = stats;
        intialPos = transform.position *1;
        enemyDictionary = new SortedDictionary<long, EnemyController>();
        rigidBody = GetComponent<Rigidbody2D>();
        rangeController = GetComponentInChildren<AttackRangeController>();

        _lifeShapeController = GetComponentInChildren<LifeShapeController>();

        _lifeShapeController.Init();
        _lifeShapeController.UpdateShadow(stats.maxLifes,stats.currentLifes);

        rangeController.SetOnEnterAction(OnRangeEnter);
        rangeController.SetOnExitAction(OnRangeExit);
        _gun = GetComponent<GunController>();
        _alive =true;
    }
    public void Move(Utils.DirectionEnumerator dir)
    {
        rigidBody.velocity = Utils.DirectionToVector(dir) * stats.speed.ValueFloat;
        if ((byte)dir != 0x0)
            isMoving = true;
        else
            isMoving = false;



    }

    public void ModifyStats(EntityStats sum){
        stats = stats +sum;
        ApplyNewStats();
        

    }

    private void ApplyNewStats(){
        rangeController.SetRange(stats.range.ValueFloat);
    }
}
