using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackSystem : MonoBehaviour
{
    [SerializeField]
    private InputAction _lightAttack;
    [SerializeField]
    private InputAction _heavyAttack;
    [SerializeField]
    private InputAction _dashAttack;
    [SerializeField]
    private LayerMask _hurtboxLayer;

    //Evolution System Variables
    public float HeavyArea { set => _heavyArea = value; }
    public float DashDamage { set => _dashDamage = value; }
    public int LightHits { set => _lightHits = value; }
    
    private EvolutionSystem _evolutionSystem;

    //temporarily making these public
    public float _heavyArea;
    public int _lightHits;
    public float _dashDamage;

    private bool _disableAttack = false;

    bool _isLightAttack = false;
    bool _isHeavyAttack = false;
    bool _isDashAttack = false;

    bool _inStartupFrames = false;
    bool _inActiveFrames = false;
    bool _inCooldownFrames = false;

    int _startupFrames;
    int _activeFrames;
    int _cooldownFrames;

    int _frameCounter;

    void Awake()
    {
        _lightAttack.Enable();
        _heavyAttack.Enable();
        _dashAttack.Enable();

        _evolutionSystem = GetComponent<EvolutionSystem>();
    }
    void Update()
    {
        if(!_disableAttack)
        CheckAttackInput();

        if(_disableAttack)
        {
            AttackExecution();
        }
    }

    void AttackExecution()
    {
        if(!_inStartupFrames &! _inActiveFrames &! _inCooldownFrames)
        {
            _disableAttack = false;
            _frameCounter = 0;
            return;
        }
        if(_inStartupFrames)
        {
            if(IsStartupElapsed())
            {
                _inStartupFrames = false;
                _inActiveFrames = true;
                _frameCounter = 0;
            }
        }
        else if(_inActiveFrames)
        {
            if(!IsActiveElapsed())
            {
                HitDetection();
            }
            else
            {
                _inActiveFrames = false;
                _inCooldownFrames = true;
            }
        }
        else if (_inCooldownFrames)
        {
            if(IsCooldownElapsed())
            {
                _inCooldownFrames = false;
            }
        }
        _frameCounter++;
    }

    void HitDetection()
    {
        if(_isLightAttack)
        {
            ExecuteLightAttack();
        }
        if(_isHeavyAttack)
        {
            ExecuteHeavyAttack();
        }
        if(_isDashAttack)
        {
            ExecuteDashAttack();
        }
    }

    void ExecuteLightAttack()
    {
        Vector3 hitboxSize = new Vector3(0.5f, 0.5f, 2);
        Vector3 overlapBoxPosition = transform.position + transform.forward; 
        Collider[] colliders = Physics.OverlapBox(overlapBoxPosition, hitboxSize, Quaternion.identity, _hurtboxLayer);
        if (colliders.Length > 0)
        {
            //Debug.Log("Hit");
            //0 because 0 is the LightAttack index (Heavy is 1 and Dash is 2)
            _evolutionSystem.SuccesfulHit(0);
        }
    }

    private void OnDrawGizmos()
    {
        if (_inActiveFrames)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.clear;

        Gizmos.matrix = Matrix4x4.TRS(transform.position + transform.forward, transform.rotation, transform.localScale);
        Vector3 hitboxSize = new Vector3(0.5f, 0.5f, 2);
        Gizmos.DrawCube(Vector3.zero, new Vector3(hitboxSize.x * 2, hitboxSize.y * 2, hitboxSize.z * 2));
    }

    void ExecuteHeavyAttack()
    {

    }

    void ExecuteDashAttack()
    {

    }

    bool IsStartupElapsed()
    {
        if (_frameCounter > _startupFrames)
        {
            return true;
        }
        else
            return false;
    }

    bool IsActiveElapsed()
    {
        if (_frameCounter > _activeFrames)
            return true;
        else
            return false;
    }

    bool IsCooldownElapsed()
    {
        if (_frameCounter > _cooldownFrames)
            return true;
        else
            return false;
    }

    void CheckAttackInput()
    {
        if (_lightAttack.WasPressedThisFrame())
        {
            FireLightAttack();
        }
        if (_heavyAttack.IsPressed())
        {
            FireHeavyAttack();
        }
        if (_dashAttack.IsPressed())
        {
            FireDashAttack();
        }

    }

    void FireLightAttack()
    {
        int startupFrames = 20;
        int activeFrames = 7;
        int cooldownFrames = 10;
        AttackFrameTimer(startupFrames, activeFrames, cooldownFrames);
        _isHeavyAttack = false;
        _isDashAttack = false;
        _isLightAttack = true;


        //Vector3 hitboxSize = new Vector3(1, 1, 4);
        //Collider[] colliders = Physics.OverlapBox(transform.position, hitboxSize, Quaternion.identity, _hurtboxLayer);
        //if(colliders.Length > 0)
        //{
        //    Debug.Log("Hit");
        //}
    }

    void FireHeavyAttack()
    {

    }

    void FireDashAttack()
    {

    }

    void AttackFrameTimer(int startupFrames, int activeFrames, int cooldownFrames)
    {
        _disableAttack = true;
        _inStartupFrames = true;
        _startupFrames = startupFrames;
        _activeFrames = activeFrames;
        _cooldownFrames = cooldownFrames;
    }
}
