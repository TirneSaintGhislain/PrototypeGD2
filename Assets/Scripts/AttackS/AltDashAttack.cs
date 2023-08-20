using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AltDashAttack : BaseAttack
{
    [SerializeField]
    private int _dashFrameDuration;
    [SerializeField]
    private float _dashBaseDistance;

    [SerializeField]
    private float _hitStunTime;
    [SerializeField]
    private float _knockBackStrength;

    private bool _canCollide = false;
    private bool _hitAnAttack = false;
    private MovementSystem _movementSystem;
    private EvolutionSystem _evolutionSystem;

    public override void StartAttack(InputAction.CallbackContext context)
    {
        if (context.started & !_attackSystem.ActiveAttack && _attackSystem.AlternateDashAttack)
        {
            _attackSystem._event1 += Dash;
            _attackSystem._event2 += StartCooldown;
            _attackSystem.ActiveAttack = true;
            _attackSystem.CurrentAttackType = _thisAttackType;
            _attackSystem._event1.Invoke();

        }
    }

    protected override void Start()
    {
        base.Start();
        _movementSystem = GetComponent<MovementSystem>();
    }

    protected override void Cleanup()
    {
        base.Cleanup();
        _attackSystem._event1 -= Dash;
        _attackSystem._event2 -= StartCooldown;
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        col.isTrigger = false;
        _attackSystem.EnableMovement();
    }

    private void Dash()
    {
        float dashDistance = _dashBaseDistance / _dashFrameDuration;
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        col.isTrigger = true;
        _canCollide = true;
        StartCoroutine(ExecuteDash(dashDistance, transform.right));
    }

    private IEnumerator ExecuteDash(float dashDistance, Vector3 direction)
    {
        for (int i = 0; i < _dashFrameDuration; i++)
        {
            _movementSystem.Dash(dashDistance, direction);
            yield return null;
        }
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        col.isTrigger = false;
        _canCollide = false;
        _hitAnAttack = false;
        _attackSystem._event2.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<HealthSystem>() != null && collision.gameObject != gameObject && _canCollide &! _hitAnAttack)
        {
            collision.gameObject.GetComponent<HealthSystem>().GetHit(_attackDamage, _hitStunTime, _knockBackStrength);
            _evolutionSystem.SuccesfulHit((int)_thisAttackType);
            _hitAnAttack = true;
        }
    }

    protected override void ActiveEvent()
    {
        
    }

    protected override void ActiveFinishedEvent()
    {
        
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
       
    }
}
