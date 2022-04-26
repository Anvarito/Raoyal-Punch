using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Fighter
{
    private Vector3 _currentLookVector;
    private Vector3 _currentVelocity;
    void Start()
    {
        HitPointsCurrent = HitPointsMax;
        HP_bar.ResetValue(HitPointsMax.ToString());
        _currentLookVector = transform.forward;
    }

    protected override void Update()
    {
        base.Update();
        Rotation();
    }

    private void Rotation()
    {
        Vector3 rotationTarget = (Opponent.transform.position - transform.position).normalized;
        rotationTarget = new Vector3(rotationTarget.x, 0, rotationTarget.z);
        _currentLookVector = Vector3.SmoothDamp(_currentLookVector, rotationTarget, ref _currentVelocity, AnimSmoothTime);

        transform.rotation = Quaternion.LookRotation(_currentLookVector);
    }

}
