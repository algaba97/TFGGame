using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerController player;
    private void Awake() {
        player = GetComponent<PlayerController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        byte dir = 0x0;
        if (Input.GetKey("w"))
        {
            dir += (byte)Utils.DirectionEnumerator.NORTH;
        }
        if (Input.GetKey("s"))
        {
            dir += (byte)Utils.DirectionEnumerator.SOUTH;
        }
        if (Input.GetKey("d"))
        {
            dir += (byte)Utils.DirectionEnumerator.EAST;
        }
        if (Input.GetKey("a"))
        {
            dir += (byte)Utils.DirectionEnumerator.WEST;
        }
        if(player != null)
            player.Move((Utils.DirectionEnumerator)dir);
    }
}
