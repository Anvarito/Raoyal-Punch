using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private HitPointBar HP_bar;
    [SerializeField] private float HitPointsMax;
    [SerializeField] private float HitPointsCurrent;
    
    void Start()
    {
        HitPointsCurrent = HitPointsMax;
        HP_bar.ResetValue(HitPointsMax.ToString());
    }

    void Update()
    {
        
    }

    public void TakeHit(float hit)
    {
        HitPointsCurrent -= hit;
        float alpha = HitPointsCurrent / HitPointsMax;
        HP_bar.ChangeValue(HitPointsCurrent.ToString(), alpha);
    }
}
