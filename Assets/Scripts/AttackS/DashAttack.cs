using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DashAttack : BaseAttack
{
    [Header("Data of Dash")]
    [SerializeField]
    private int _dashFrameDuration;
    [SerializeField]
    private float _dashBaseDistance;
    [SerializeField]
    private float _hitStunTime;
    [SerializeField]
    private float _knockBackStrength;
    [SerializeField]
    private bool _noFollowUp = false;

    [Header("Dash Attack Hitbox")]
    [SerializeField]
    public Vector3 _baseRange;
    [HideInInspector]
    public Vector3 _currentRange;

    private MovementSystem _movementSystem;
    public override void StartAttack(InputAction.CallbackContext context)
    {
        if (context.started & !_attackSystem.ActiveAttack &! _attackSystem.AlternateDashAttack)
        {
            _attackSystem._event1 += Dash;
            _attackSystem._event2 += StartStartup;
            _attackSystem._event3 += StartActive;
            _attackSystem._event4 += StartCooldown;
            _attackSystem.ActiveAttack = true;
            _attackSystem.CurrentAttackType = _thisAttackType;
            _attackSystem.Gizmos = _baseRange;
            _attackSystem._event1.Invoke();
        }
    }

    protected override void Start()
    {
        base.Start();
        _movementSystem = GetComponent<MovementSystem>();
    }

    private void Dash()
    {
        float dashDistance = _dashBaseDistance / _dashFrameDuration;
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        col.isTrigger = true;
        StartCoroutine(ExecuteDash(dashDistance, transform.right));
    }

    IEnumerator ExecuteDash(float dashDistance, Vector3 direction)
    {
        for(int i = 0; i < _dashFrameDuration; i++)
        {
            _movementSystem.Dash(dashDistance, direction);
            yield return null;
        }
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        col.isTrigger = false;
        if(!_noFollowUp)
        {
            _attackSystem.InStartupFrames = true;
            _attackSystem._event2.Invoke();
        }
        else
        {
            Cleanup();
        }
        
    }

    protected override void Cleanup()
    {
        base.Cleanup();
        _attackSystem._event1 -= Dash;
        _attackSystem._event2 -= StartStartup;
        _attackSystem._event3 -= StartActive;
        _attackSystem._event4 -= StartCooldown;
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        col.isTrigger = false;
        _attackSystem.EnableMovement();
    }

    protected override void ActiveEvent()
    {
        _attackSystem.HitDetection(_baseRange, _hitStunTime, _knockBackStrength, _attackDamage);
        GetComponent<AudioSystem>().PlayDashAttack();
    }

    protected override void ActiveFinishedEvent()
    {
        _attackSystem._event4.Invoke();
        _attackSystem.InActiveFrames = false;
    }

    protected override void CooldownEvent()
    {

    }

    protected override void CooldownFinishedEvent()
    {
        Cleanup();
    }

    protected override void StartupEvent()
    {
        
    }

    protected override void StartupFinishedEvent()
    {
        _attackSystem._event3.Invoke();
        _attackSystem.InStartupFrames = false;
        _attackSystem.InActiveFrames = true;
    }
}
