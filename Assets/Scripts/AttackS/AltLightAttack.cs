using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AltLightAttsack : BaseAttack
{
    [SerializeField]
    private float _hitStunTime;
    [SerializeField]
    private float _knockBackStrength;

    [Header("Light Attack Hitbox")]
    [SerializeField]
    public Vector3 _baseRange;
    [HideInInspector]
    public Vector3 _currentRange;

    public override void StartAttack(InputAction.CallbackContext context)
    {
        if (context.started & !_attackSystem.ActiveAttack)
        {
            _attackSystem._event1 += StartStartup;
            _attackSystem._event2 += StartActive;
            _attackSystem._event3 += StartCooldown;
            _attackSystem.ActiveAttack = true;
            _attackSystem.CurrentAttackType = _thisAttackType;
            _attackSystem.InStartupFrames = true;
            _attackSystem.Gizmos = _baseRange;
            _attackSystem._event1.Invoke();

        }
    }

    protected override void Cleanup()
    {
        base.Cleanup();
        _attackSystem._event1 -= StartStartup;
        _attackSystem._event2 -= StartActive;
        _attackSystem._event3 -= StartCooldown;
        _attackSystem.EnableMovement();
        //Debug.Log("Cleanup was called");
    }

    protected override void ActiveEvent()
    {
        throw new System.NotImplementedException();
    }

    protected override void ActiveFinishedEvent()
    {
        throw new System.NotImplementedException();
    }

    protected override void CooldownEvent()
    {
        throw new System.NotImplementedException();
    }

    protected override void CooldownFinishedEvent()
    {
        throw new System.NotImplementedException();
    }

    protected override void StartupEvent()
    {
        throw new System.NotImplementedException();
    }

    protected override void StartupFinishedEvent()
    {
        throw new System.NotImplementedException();
    }
}
