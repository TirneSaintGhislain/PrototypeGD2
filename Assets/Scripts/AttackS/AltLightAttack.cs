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
        _attackSystem.HitDetection(_baseRange, _hitStunTime, _knockBackStrength, _attackDamage);
        GetComponent<AudioSystem>().PlayLightAttack();
    }

    protected override void ActiveFinishedEvent()
    {
        _attackSystem._event3.Invoke();
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
        _attackSystem._event2.Invoke();
        _attackSystem.InStartupFrames = false;
        _attackSystem.InActiveFrames = true;
    }
}
