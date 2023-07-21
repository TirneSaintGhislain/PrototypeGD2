using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DashAttack : BaseAttack
{
    [Header("Data of Dash")]
    [SerializeField]
    int _dashFrameDuration;
    [SerializeField]
    float _dashBaseDistance;

    [Header("Dash Attack Hitbox")]
    [SerializeField]
    int _x;
    [SerializeField]
    int _y;
    [SerializeField]
    int _z;
    public override void StartAttack(InputAction.CallbackContext context)
    {
        if (context.started & !_attackSystem.ActiveAttack)
        {
            _attackSystem._event1 += Dash;
            _attackSystem._event2 += StartStartup;
            _attackSystem._event3 += StartActive;
            _attackSystem._event4 += StartCooldown;
            _attackSystem.ActiveAttack = true;
            _attackSystem._event1.Invoke();
        }
    }

    private void Dash()
    {
        float dashDistance = _dashBaseDistance / _dashFrameDuration;
        StartCoroutine(ExecuteDash(dashDistance));
    }

    IEnumerator ExecuteDash(float dashDistance)
    {
        for(int i = 0; i < _dashFrameDuration; i++)
        {
            //Move player rigidbody in dash direction at given speed
            yield return null;
        }
        _attackSystem._event2.Invoke();
    }

    protected override void Cleanup()
    {
        base.Cleanup();
        _attackSystem._event1 -= Dash;
        _attackSystem._event2 -= StartStartup;
        _attackSystem._event3 -= StartActive;
        _attackSystem._event4 -= StartCooldown;
    }

    protected override void ActiveEvent()
    {
        _attackSystem.HitDetection(_x, _y, _z);
    }

    protected override void ActiveFinishedEvent()
    {
        _attackSystem._event4.Invoke();
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
    }
}
