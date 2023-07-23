using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeavyAttack : BaseAttack
{
    [Header("Heavy Attack Base Radius")]
    [SerializeField]
    private float _baseRadius;

    public float Radius { get; set; }

    private void Start()
    {
        Radius = _baseRadius;
    }

    public override void StartAttack(InputAction.CallbackContext context)
    {
        if (context.started & !_attackSystem.ActiveAttack)
        {
            _attackSystem._event1 += StartStartup;
            _attackSystem._event2 += StartActive;
            _attackSystem._event3 += StartCooldown;
            _attackSystem.ActiveAttack = true;
            _attackSystem.CurrentAttackType = _thisAttackType;
            _attackSystem._event1.Invoke();
        }
    }

    protected override void Cleanup()
    {
        base.Cleanup();
        _attackSystem._event1 -= StartStartup;
        _attackSystem._event2 -= StartActive;
        _attackSystem._event3 -= StartCooldown;
    }

    protected override void ActiveEvent()
    {
        _attackSystem.HitDetection(Radius);
    }

    protected override void ActiveFinishedEvent()
    {
        _attackSystem._event3.Invoke();
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
        _attackSystem._event2.Invoke();
    }
}
