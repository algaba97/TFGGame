using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
public class AgentPlayer : Agent, OnRoundListener, OnEnemyKillListener
{
    public PlayerController player;
    public BaseLogic baseLogic;

    protected int currentRound;

    protected bool enemyKilled;

    protected bool roundFinished;

    protected int lastKnowLifes = 3;

    protected int lastKnowBaseLifes= 10;

    public float rayDistance;

    int layer = 1 <<11;




    public void Init(GameManager gameManager){
        gameManager.onEnemyKillListeners.Add(this);
        gameManager.onRoundListeners.Add(this);

    }
    public override void AgentReset()
    {
         lastKnowBaseLifes= 10; lastKnowLifes = 3;
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
        
        if(enemyKilled){
            SetReward(1.0f);      
            enemyKilled = false;  
        }

        if(lastKnowBaseLifes != baseLogic.lifesCounter){
            //SetReward(-1.0f);
            lastKnowBaseLifes = baseLogic.lifesCounter;      
            
        }

        if(lastKnowLifes != player.stats.currentLifes){
            //SetReward(-1.0f);
            lastKnowLifes = player.stats.currentLifes;      
            
        }


        if(roundFinished){
            SetReward(currentRound);      
            roundFinished = false;  
        }

    }
    public override void CollectObservations()
    {
        // Target and Agent positions
        AddVectorObs(player.transform.localPosition);
        AddVectorObs(baseLogic.transform.localPosition);
        AddVectorObs(currentRound);
        AddVectorObs(player.stats.range.ValueFloat);
        AddVectorObs(player.stats.speed.ValueFloat);
        AddVectorObs(player.stats.currentLifes);

        RaycastHit2D hitinfo;
        
        for (int i = 0; i < 36; i++)
        {
            Vector2 aux = Quaternion.AngleAxis(i*(360/36), Vector3.forward) * Vector2.up;
            hitinfo = Physics2D.Raycast(transform.position,aux,rayDistance,layer);
             AddVectorObs(hitinfo.distance);
             
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
        roundNumber = currentRound;
    }

}