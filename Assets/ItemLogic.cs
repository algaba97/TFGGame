using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLogic : MonoBehaviour
{
    public EntityStats stats;

    public float lifeSpan;


    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        print("enter");
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if(player != null){
            player.ModifyStats(stats);
        }
        Destroy(gameObject);
    }


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine(){
        yield return new WaitForSeconds(lifeSpan);
        Destroy(gameObject);
    }


}
