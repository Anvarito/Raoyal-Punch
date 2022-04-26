using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Fighter : MonoBehaviour
{
    [SerializeField] protected Fighter Opponent;

    [Header("Animation")]
    [SerializeField] protected float AnimSmoothTime;
    protected Animator _animator;
    protected int _fightAnimID;

    [Header("Hit points")]
    [SerializeField] protected HitPointBar HP_bar;
    [SerializeField] protected float HitPointsMax;
    protected float HitPointsCurrent;

    [Header("Punch")]
    [SerializeField] protected float AttackDistance;
    [SerializeField] protected float HitPower = 10;
    private bool opponentTakeHit = false;

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _fightAnimID = Animator.StringToHash("fight");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        AttackChecker();
    }

    private void TakeHit(float hit)
    {
        HitPointsCurrent -= hit;
        float alpha = HitPointsCurrent / HitPointsMax;
        HP_bar.ChangeValue(HitPointsCurrent.ToString(), alpha);
    }

    private void AttackChecker()
    {
        var distance = Vector3.Distance(transform.position, Opponent.transform.position);
        bool isVisibleRange = Vector3.Dot(transform.forward, (Opponent.transform.position - transform.position).normalized) > 0.2f;

        opponentTakeHit = distance < AttackDistance && isVisibleRange;

        _animator.SetBool(_fightAnimID, opponentTakeHit);
    }

    //Called in event from animation
    public void Punch()
    {
        if (!opponentTakeHit)
        {
            return;
        }

        Opponent.TakeHit(HitPower);
    }
}
