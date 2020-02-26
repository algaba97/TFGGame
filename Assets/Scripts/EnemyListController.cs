using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyListController : MonoBehaviour
{
    public void DestroyAllEnemies()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
