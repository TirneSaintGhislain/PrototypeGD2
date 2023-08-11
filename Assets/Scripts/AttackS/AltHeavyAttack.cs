using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AltHeavyAttack : BaseAttack
{
    [SerializeField]
    private float _hitStunTime;
    [SerializeField]
    private float _knockBackStrength;
    //[HideInInspector]
    //public float _baseRadius;

    //public float Radius { get; set; }

    [Header("Light Attack Hitbox")]
    [SerializeField]
    public Vector3 _baseRange;
    [HideInInspector]
    public Vector3 _currentRange;

    //protected override void Start()
    //{
    //    base.Start();
    //    Radius = _baseRadius;
    //}
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
    }

    protected override void ActiveEvent()
    {
        _attackSystem.HitDetection(_baseRange, _hitStunTime, _knockBackStrength, _attackDamage);
        GetComponent<AudioSystem>().PlayHeavyAttack();
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
