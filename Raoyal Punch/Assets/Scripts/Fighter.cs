using System;
using UnityEngine;



[RequireComponent(typeof(Animator))]
public abstract class Fighter : MonoBehaviour
{
    [SerializeField] protected Fighter Opponent;
    [SerializeField] protected float VisibleTreshold;

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
    private bool _enemyInPunchZone = false;

    [Header("Ragdoll")]
    //[SerializeField] protected Collider MainCollider;
    [SerializeField] protected Rigidbody HeadRigidbody;
    [SerializeField] protected Rigidbody[] rigidbodies;
    [SerializeField] protected Collider[] colliders;
    [SerializeField] protected Transform Armature;
    [SerializeField] protected Transform Hips;

    protected Vector3[] bonesCapturePos = new Vector3[11];
    protected Quaternion[] bonesCaptureRot = new Quaternion[11];
    

    public static Action<Fighter> OnFighterDefeat;
    protected bool _isfighterDown = false;
    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        AssignAnimationToHash();
    }

    protected virtual void AssignAnimationToHash()
    {
        _fightAnimID = Animator.StringToHash("fight");
    }
    protected virtual void Start()
    {
        HitPointsCurrent = HitPointsMax;
        HP_bar.SetValue(HitPointsMax.ToString());

        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = true;
            colliders[i].enabled = false;
        }
    }

    public virtual void ResetGame()
    {
        ResetRagdoll();
        HP_bar.SetValue(HitPointsMax.ToString());
    }

    protected virtual void Update()
    {
        if (Game.GetGameState() == EGameState.inFight && !_isfighterDown)
        {
            AttackChecker();
            Movement();
        }
    }

    protected abstract void Movement();
    protected void TakeHit(float hit)
    {
        HitPointsCurrent -= hit;
        HitPointsCurrent = Mathf.Clamp(HitPointsCurrent, 0, HitPointsMax);
        float value = HitPointsCurrent / HitPointsMax;
        HP_bar.ChangeValue(HitPointsCurrent.ToString(), value);

        if (HitPointsCurrent == 0 && !_isfighterDown)
        {
            OnFighterDefeat?.Invoke(this);
            EnableRagdoll();
            HeadRigidbody.AddForce((transform.position - Opponent.transform.position).normalized * 100, ForceMode.Impulse);
        }
    }

    protected virtual void EnableRagdoll()
    {
        _isfighterDown = true;
        _animator.SetBool(_fightAnimID, false);
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = false;
            rigidbodies[i].useGravity = true;
            colliders[i].enabled = true;
            bonesCapturePos[i] = rigidbodies[i].transform.localPosition;
            bonesCaptureRot[i] = rigidbodies[i].transform.localRotation;
        }
        _animator.enabled = false;
        Hips.transform.parent = null;
    }

    private void ResetRagdoll()
    {
        _isfighterDown = false;
        _animator.SetBool(_fightAnimID, false);
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = true;
            rigidbodies[i].useGravity = false;
            colliders[i].enabled = false;
        }
        _animator.enabled = true;
        Hips.transform.parent = Armature.transform;
        Hips.transform.position = Vector3.zero;
    }

    private void AttackChecker()
    {
        var distance = GetDistantToOpponent();
        bool isVisibleRange = IsOpponentInVisible();

        _enemyInPunchZone = distance < AttackDistance && isVisibleRange;

        _animator.SetBool(_fightAnimID, _enemyInPunchZone);
    }

    protected float GetDistantToOpponent()
    {
        float distance = Vector3.Distance(transform.position, Opponent.transform.position);
        return distance;
    }

    protected bool IsOpponentInVisible()
    {
        return Vector3.Dot(transform.forward, (Opponent.transform.position - transform.position).normalized) > VisibleTreshold;
    }

    //Called in event from animation
    public void Punch()
    {
        if (!_enemyInPunchZone || Game.GetGameState() != EGameState.inFight)
        {
            return;
        }

        Opponent.TakeHit(HitPower);
    }

    public virtual void FighterWin()
    {
        _animator.SetBool(_fightAnimID, false);
    }
}
