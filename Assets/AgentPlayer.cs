using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
public class AgentPlayer : Agent
{
    public PlayerController player;
    public BaseLogic baseLogic;

    public override void AgentReset()
    {
        
    }

    public override void AgentAction(float[] vectorAction)
    {
         byte dir = 0x0;
        if (vectorAction[0]>0.5)
        {
            dir += (byte)Utils.DirectionEnumerator.NORTH;
        }
        if (vectorAction[0]<-0.5)
        {
            dir += (byte)Utils.DirectionEnumerator.SOUTH;
        }
        if (vectorAction[1]<-0.5)
        {
            dir += (byte)Utils.DirectionEnumerator.EAST;
        }
        if (vectorAction[1]>0.5)
        {
            dir += (byte)Utils.DirectionEnumerator.WEST;
        }
        if(player != null)
            player.Move((Utils.DirectionEnumerator)dir);

    }
    public override void CollectObservations()
    {
        // Target and Agent positions
        AddVectorObs(player.transform.localPosition);
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
        if(!player.alive){
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

}