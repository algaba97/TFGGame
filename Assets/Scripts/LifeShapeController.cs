using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeShapeController : MonoBehaviour
{
    private void Awake()
    {
        UpdateShadow(1.0f,0.0f);
    }
    public void Init()
    {
        UpdateShadow(1.0f,0.0f);
    }

    public void UpdateShadow(float maxSize,float sizeToChange){
        float scale =(maxSize-sizeToChange)/maxSize; 
        transform.localScale = new Vector3(scale,scale,1.0f);
    }
}
