using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLogic : Hittable
{
    private TextMesh lifesCounterText;
    private LifeShapeController _lifeShape;
    public int startLifes;
    private int _lifesCounter;
    public int _maxLifeCounter;
    public int lifesCounter
    {
        get
        {
            return _lifesCounter;
        }
        set
        {
            _lifesCounter = System.Math.Min(System.Math.Max(0, value), _maxLifeCounter);
            UpdateCounter();
            if (_lifesCounter < 1 ){
                if(_OnDestroyBaseAction!=null)
                    _OnDestroyBaseAction(this);
            }


        }
    }

    private System.Action<BaseLogic> _OnDestroyBaseAction=null;


    private void UpdateCounter()
    {
        _lifeShape.UpdateShadow(startLifes, _lifesCounter);
    }


    public override void Hit()
    {
        lifesCounter --;        
    }

    public void Init(System.Action<BaseLogic> OnBaseDestroyAction=null){
        _OnDestroyBaseAction = OnBaseDestroyAction;
        lifesCounter = startLifes;
        _lifeShape.Init();
        _lifeShape.UpdateShadow(startLifes, _lifesCounter);
    }

    private void Start()
    {
        lifesCounterText = GetComponentInChildren<TextMesh>();
        _lifeShape = GetComponentInChildren<LifeShapeController>();
        Init();
        
    }

}
