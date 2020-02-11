using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlue : EnemyController
{
    public float advanceTime;
    protected override void GoToTarget(Vector3 destination)
    {
        StartCoroutine(FaceToTargetCoroutine(destination));
    }

    private IEnumerator AdvanceCoroutine(float time, Vector3 destination){

        while(!targetReached && (destination - transform.position).magnitude > 0.1f && time>0.0f ){
            Advance();
            time -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Stop();
        yield return new WaitForSeconds(stats.fireRate.ValueFloat);
        SelectRandomTarget();
        GoToTarget(target.position);
    }

    private IEnumerator FaceToTargetCoroutine(Vector3 destination){
        while(!FaceToTarget(destination)){
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(AdvanceCoroutine(advanceTime,destination));
        
    }

    

    protected override void Start(){
        SelectRandomTarget();
        GoToTarget(target.position);
    }
    
}
