using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
public class AgentPlayer : Agent, OnRoundListener, OnEnemyKillListener,OnShootListener, OnRangeListener
{
    public PlayerController player;
    public BaseLogic baseLogic;

    protected int currentRound;

    protected bool enemyKilled;

    protected bool roundFinished;

    protected int lastKnowLifes = 3;

    protected int lastKnowBaseLifes= 10;

    public float rayDistance;

    protected bool shooted = false;

    protected int enemiesOnRange=0;


    private int lastEnemiesOnRange=0;
    int layer = 1 << 11;


    public void OnRangeEnter(GameObject gO)
    {
        if (gO.tag == "Enemy")
        {
            enemiesOnRange ++;
        }
    }

    public void OnRangeExit( GameObject gO)
    {
        
        if (gO.tag == "Enemy")
        {
           enemiesOnRange --;
        }
    }

    public void Init(GameManager gameManager){
        gameManager.onEnemyKillListeners.Add(this);
        gameManager.onRoundListeners.Add(this);
        player.onShootListeners.Add(this);
        player.rangeController.onRangeListeners.Add(this);

    }
    public override void AgentReset()
    {
         lastKnowBaseLifes= 10; 
         lastKnowLifes = 3;
         enemyKilled = false;
         shooted = false;
         enemiesOnRange = lastEnemiesOnRange = 0;
         currentRound =0;
    }

    public override void AgentAction(float[] vectorAction)
    {
        
        byte dir = 0x0;
        if (vectorAction[0] > 0.5)
        {
            dir += (byte)Utils.DirectionEnumerator.NORTH;
        }
        if (vectorAction[0] < -0.5)
        {
            dir += (byte)Utils.DirectionEnumerator.SOUTH;
        }
        if (vectorAction[1] < -0.5)
        {
            dir += (byte)Utils.DirectionEnumerator.EAST;
        }
        if (vectorAction[1] > 0.5)
        {
            dir += (byte)Utils.DirectionEnumerator.WEST;
        }
        if (player != null)
            player.Move((Utils.DirectionEnumerator)dir);
        
        //SetReward(0.05f*currentRound);

        if(enemyKilled){
            AddReward(1.0f);      
            enemyKilled = false;  
        }

        //AddReward(enemiesOnRange*0.10f);
        

        if(lastKnowBaseLifes > baseLogic.lifesCounter){
            AddReward(-0.3f);
            lastKnowBaseLifes = baseLogic.lifesCounter;
        }

        if(lastKnowLifes > player.stats.currentLifes){
            AddReward(-1.0f);
            lastKnowLifes = player.stats.currentLifes;      
        }
        if(shooted){
            shooted = false;
            //AddReward(0.5f);
        }
    }

    Queue<Vector2> auxQ = new Queue<Vector2>();
    public override void CollectObservations()
    {
        // Target and Agent positions
        AddVectorObs(player.velocity);
        AddVectorObs(player.transform.localPosition);
        AddVectorObs(baseLogic.transform.position-player.transform.position);

        RaycastHit2D hitinfo;
        int j = 0;
        Vector2 player2dPos = new Vector2(player.transform.localPosition.x,player.transform.localPosition.z);
        for (int i = 0; i < 72; i++)
        {
            Vector2 aux = Quaternion.AngleAxis(i*5.0f, Vector3.forward) * Vector2.up;
            hitinfo = Physics2D.Raycast(player.transform.position,aux,rayDistance,layer);
            if(hitinfo.distance>0.5f){ 
                Vector2 hitPos2D = new Vector2(hitinfo.transform.localPosition.x,hitinfo.transform.localPosition.z);
                auxQ.Enqueue(aux*hitinfo.distance);
            }
            else{
                j++;
            }
            //Debug.DrawRay(baseLogic.transform.position, aux * hitinfo.distance, Color.white); 
        }
        int addedObs = 20;
        
        while(auxQ.Count > 0 && addedObs >0){
            AddVectorObs(auxQ.Dequeue());
            addedObs--;
        }
        while(j>0&& addedObs >0){
            j--;
            addedObs--;
            AddVectorObs(new Vector2(0.0f,0.0f));

        }
    }
    public override float[] Heuristic()
    {
        var action = new float[2];
        if (Input.GetKey("w"))
        {
            action[0] += 1;
        }
        if (Input.GetKey("s"))
        {
            action[0] += -1;

        }
        if (Input.GetKey("d"))
        {
            action[1] += -1;

        }
        if (Input.GetKey("a"))
        {
            action[1] += 1;

        }
        return action;
    }


    public void FixedUpdate()
    {
        if (!player.alive || baseLogic.lifesCounter == 0)
        {
            Done();
            return;
        }
        WaitTimeInference();

    }

    public float timeBetweenDecisionsAtInference;
    float m_TimeSinceDecision;

    void WaitTimeInference()
    {

        if (m_TimeSinceDecision >= timeBetweenDecisionsAtInference)
        {
            m_TimeSinceDecision = 0f;
            RequestDecision();
        }
        else
        {
            m_TimeSinceDecision += Time.fixedDeltaTime;
        }
    }
    public void OnEnemyKill(EnemyController enemy)
    {
        enemyKilled = true;
        
    }

    public void OnRoundOver(int roundNumber){
        roundFinished = true;        
        
    }
    public void OnRoundStart(int roundNumber){
        currentRound = roundNumber;
        Debug.Log("round: "+currentRound);
    }

    public void OnShoot(){
        shooted = true;
    }

}