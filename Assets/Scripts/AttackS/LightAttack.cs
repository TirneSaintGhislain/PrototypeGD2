using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightAttack : BaseAttack
{
    [SerializeField]
    private float _hitStunTime;
    [SerializeField]
    private float _knockBackStrength;

    [Header("Light Attack Hitbox")]
    [SerializeField]
    private float _x;
    [SerializeField]
    private float _y;
    [SerializeField]
    private float _z;

    [HideInInspector]
    public bool _canEvolve = true;
    int _currentHits = 0;

    public override void StartAttack(InputAction.CallbackContext context)
    {
        if (context.started & !_attackSystem.ActiveAttack)
        {
            _canEvolve = true;
            _attackSystem._event1 += StartStartup;
            _attackSystem._event2 += StartActive;
            _attackSystem._event3 += RepeatAttacks;
            _attackSystem._event4 += StartCooldown;
            _attackSystem.ActiveAttack = true;
            _attackSystem.CurrentAttackType = _thisAttackType;
            _attackSystem.InStartupFrames = true;
            _attackSystem.Gizmos = new Vector3(_x, _y, _z);
            _attackSystem._event1.Invoke();
            
        }
    }

    protected override void Cleanup()
    {
        base.Cleanup();
        _attackSystem._event1 -= StartStartup;
        _attackSystem._event2 -= StartActive;
        _attackSystem._event3 -= RepeatAttacks;
        _attackSystem._event4 -= StartCooldown;
        _currentHits = 0;
        _attackSystem.EnableMovement();
        //Debug.Log("Cleanup was called");
    }

    private void RepeatAttacks()
    {
        _currentHits++;
        if(_currentHits < _attackSystem.LightHits)
        {
            //_startupFinished = false;
            Debug.Log("Repeat count: " + _currentHits);
            _attackSystem._event1?.Invoke();
        }
        else
        {
            _attackSystem._event4.Invoke();
        }
    }

    protected override void ActiveEvent()
    {
        _attackSystem.HitDetection(_x, _y, _z, _hitStunTime, _knockBackStrength, _canEvolve);
        //Debug.Log("Active was called");
    }

    protected override void CooldownEvent()
    {
        //Debug.Log("Cooldown was called");
    }

    protected override void StartupEvent()
    {
        //Debug.Log("Startup was called");
    }
    protected override void StartupFinishedEvent()
    {
        _attackSystem._event2.Invoke();
        _attackSystem.InStartupFrames = false;
        _attackSystem.InActiveFrames = true;
    }
    protected override void ActiveFinishedEvent()
    {
        _attackSystem._event3.Invoke();
        _attackSystem.InActiveFrames = false;
    }
    protected override void CooldownFinishedEvent()
    {
        Cleanup();
    }
}
