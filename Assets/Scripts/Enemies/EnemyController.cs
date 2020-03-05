using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Hittable, OnRangeListener
{
    public static long enemyCount = 0;
    private long _enemyID;

    public long enemyID
    {
        get { return _enemyID; }
    }

    public EntityStats stats;
    protected Rigidbody2D rigidBody;

    private LifeShapeController _lifeShape;

    public Transform playerBase;
    public Transform playerPosition;

    public Transform target;

    protected int _spawnOrigin;

    protected bool targetReached;

    protected System.Action<EnemyController> _onDieAction = null;

    public void OnRangeEnter(GameObject gameObject)
    {

        if (gameObject.transform == target)
        {
            targetReached = true;
            Hittable hittable = gameObject.GetComponent<Hittable>();
            if (hittable != null)
                Attack(hittable);
        }

    }



    public void OnRangeExit(GameObject gameObject)
    {
        targetReached = false;

    }

    bool attacking = false;
    protected void Attack(Hittable hittable)
    {
        attacking = true;
        StartCoroutine(AttackCouroutine(hittable));
        StartCoroutine(AttackAnimationCouroutine());
        //Hit();
    }

    protected IEnumerator AttackAnimationCouroutine()
    {
        while (attacking)
        {
            //transform.Rotate(0, 0, 10f);
            yield return new WaitForEndOfFrame();
        }
    }
    protected void Stop(){
        rigidBody.velocity = Vector3.zero;
    }
    protected IEnumerator AttackCouroutine(Hittable hittable)
    {
        hittable.Hit();
        Stop();
        yield return new WaitForSeconds(stats.fireRate.ValueFloat);
        attacking = false;
        if (targetReached)
            Attack(hittable);


    }
    public void Init(Transform _playerBase, Transform _playerPosition, int spawnOrigin, int level = 0,System.Action<EnemyController> onDieAction = null)
    {
        _onDieAction = onDieAction;
        playerBase = _playerBase;
        playerPosition = _playerPosition;
        _spawnOrigin = spawnOrigin;
        SelectTarget();
        attackRange = GetComponentInChildren<AttackRangeController>();
        _lifeShape = GetComponentInChildren<LifeShapeController>();
        _lifeShape.Init();
        _lifeShape.UpdateShadow(stats.maxLifes,stats.currentLifes);


        attackRange.onRangeListeners.Add(this);
        targetReached = false;
        rigidBody = GetComponent<Rigidbody2D>();

        stats.maxLifes += level;
        stats.speed.level += level;
        stats.fireRate.level += level;
        stats.currentLifes = stats.maxLifes;

    }

    public override void Hit()
    {
        stats.currentLifes--;
        _lifeShape.UpdateShadow(stats.maxLifes,stats.currentLifes);

        if (stats.currentLifes < 1)
        {
            if (_onDieAction != null)
            {
                _onDieAction(this);
            }
            Destroy(gameObject);

        }
    }


    /// <summary>
    /// Face to the destination
    /// </summary>
    /// <param name="destination">the destination</param>
    /// <returns>True if is facing the target</returns>
    protected virtual bool FaceToTarget(Vector3 destination){
        Vector3 vectorToTarget = destination - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * stats.speed.ValueFloat);
        
        angle = Vector3.Angle((destination- transform.position), transform.up);
         if(angle<2.5f)
         {
             return true;
         }
        return false;
        
    }

    protected virtual void Advance()
    {
        rigidBody.velocity = transform.up * stats.speed.ValueFloat;

    }

    protected virtual void GoToTarget(Vector3 destination)
    {
        FaceToTarget(destination);
        Advance();

    }

    protected void SelectClosestTarget(){
        Vector3 currentPos = transform.position;
        if ((playerPosition.position - currentPos).magnitude <= (playerBase.position - currentPos).magnitude)
        {
            ChangeTarget(playerPosition);
        }
        else
        {
            ChangeTarget(playerBase);
        }
    }

    protected void SelectRandomTarget(){
        if(Random.Range(0,2)<1){
            ChangeTarget(playerPosition);
        }else{
            ChangeTarget(playerBase);
        }
    }

    protected virtual void SelectTarget()
    {
        Vector3 currentPos = transform.position;
        if ((playerPosition.position - currentPos).magnitude <= (playerBase.position - currentPos).magnitude)
        {
            ChangeTarget(playerPosition);
        }
        else
        {
            ChangeTarget(playerBase);
        }

    }

    public virtual void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }



    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected virtual void Awake()
    {
        _enemyID = enemyCount;
        enemyCount++;


    }
    protected AttackRangeController attackRange;
    protected virtual void Start()
    {
    }

    


}
