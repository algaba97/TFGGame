using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AttackRangeController : MonoBehaviour
{


    Collider2D _collider;
    


    public float range;
    System.Action<AttackRangeController,GameObject> OnEnterAction;
    System.Action<AttackRangeController,GameObject> OnExitAction;

    public void SetOnEnterAction(System.Action<AttackRangeController,GameObject> action){
        OnEnterAction = action;
    }

    public void SetOnExitAction(System.Action<AttackRangeController,GameObject> action){
        OnExitAction = action;
    }



    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if(OnEnterAction!=null){
            OnEnterAction(this,other.gameObject);

        }

    }



    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerExit2D(Collider2D other)
    {
        if(OnExitAction != null)
            OnExitAction(this,other.gameObject);
       
    }

    public void SetRange(float newRange){
        range = newRange;
        //transform.localScale = new Vector3(range, range, 1);

    }

    void Start(){
        _collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(range, range, 1);

    }
}
