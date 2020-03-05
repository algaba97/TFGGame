using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AttackRangeController : MonoBehaviour
{


    Collider2D _collider;

    public List<OnRangeListener> onRangeListeners = new List<OnRangeListener>();
    


    public float range;

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        foreach (var l in onRangeListeners)
        {
            l.OnRangeEnter(other.gameObject);
        }

    }



    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerExit2D(Collider2D other)
    {
        foreach (var l in onRangeListeners)
        {
            l.OnRangeExit(other.gameObject);
        }
       
    }

    public void SetRange(float newRange){
        range = newRange;
        //transform.localScale = new Vector3(range, range, 1);
        transform.localScale = new Vector3(range, range, 1);


    }

    void Start(){
        _collider = GetComponent<Collider2D>();
        SetRange(range);
    }

    
}
