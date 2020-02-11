using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rigidBody;

    private int damage;

    public void Init(float speed, Vector3 target,int dam = 1){
        rigidBody = GetComponent<Rigidbody2D>();
        target =  target- transform.position ;
        target = target.normalized;
        rigidBody.velocity = target * speed;
        damage = dam;
    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        Hittable _hittable = other.gameObject.GetComponent<Hittable>();
        if(_hittable != null)
            while(damage-->0){
                _hittable.Hit();
            }
        Destroy(gameObject);

    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0,10);
    }
}
