using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackSystem : MonoBehaviour
{
    //[SerializeField]
    //private InputAction _lightAttack;
    //[SerializeField]
    //private InputAction _heavyAttack;
    //[SerializeField]
    //private InputAction _dashAttack;
    [SerializeField]
    public LayerMask _hurtboxLayer;

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

    //This bool decides if the Player can attack or not
    private bool _canAttack = true;


    public void OnStrongAttack(InputAction.CallbackContext context)
    {
        if (context.started && _canAttack)
        {
            FireHeavyAttack();
            Debug.Log("Initiated Heavy attack " + name);

            //Start the attack cooldown
            StartCoroutine(GetComponent<HitstunSystem>().StartAttackCooldown());
        }
    }
    public void OnDashAttack(InputAction.CallbackContext context)
    {
        if (context.started && _canAttack)
        {
            FireDashAttack();
            Debug.Log("Initiated Dash attack " + name);

            //Start the attack cooldown
            StartCoroutine(GetComponent<HitstunSystem>().StartAttackCooldown());
        }
    }

    
    //void Update()
    //{
    //    if (!_disableAttack)
    //        CheckAttackInput();

    //    if (_disableAttack)
    //    {
    //        AttackExecution();
    //    }

    //    //Update the CanAttack Bool
    //    _canAttack = GetComponent<HitstunSystem>()._canAttack;
    //}
    void AttackExecution()
    {
        if (!_inStartupFrames & !_inActiveFrames & !_inCooldownFrames)
        {
            _disableAttack = false;
            _frameCounter = 0;
            return;
        }
        if (_inStartupFrames)
        {
            if (IsStartupElapsed())
            {
                _inStartupFrames = false;
                _inActiveFrames = true;
                _frameCounter = 0;
            }
        }
        else if (_inActiveFrames)
        {
            if (!IsActiveElapsed())
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
            if (IsCooldownElapsed())
            {
                _inCooldownFrames = false;
            }
        }
        _frameCounter++;
    }

    void HitDetection()
    {
        if (_isLightAttack)
        {
            ExecuteLightAttack();
        }
        if (_isHeavyAttack)
        {
            ExecuteHeavyAttack();
        }
        if (_isDashAttack)
        {
            ExecuteDashAttack();
        }
    }

    void ExecuteLightAttack()
    {
        Vector3 hitboxSize = new Vector3(0.5f, 0.5f, 2);
        Vector3 overlapBoxPosition = transform.position + transform.forward;
        Collider[] colliders = Physics.OverlapBox(overlapBoxPosition, hitboxSize/*, Quaternion.identity, _hurtboxLayer*/);
        if (colliders.Length > 0)
        {
            //Debug.Log("Hit");
            foreach (var collider in colliders)
            {
                //Checks to see if one of the colliders we hit was a player
                //And then check if the player isn't us
                if (collider.GetComponent<HealthSystem>() != null && collider.gameObject != gameObject)
                {
                    //The 1 represents the damage of the attack, ideally this will change to a variable
                    collider.GetComponent<HealthSystem>().GetHit(1);
                    //0 because 0 is the LightAttack index (Heavy is 1 and Dash is 2)
                    _evolutionSystem.SuccesfulHit(0);
                }
            }

        }
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
        //This now gets done in events

        //if (_lightAttack.WasPressedThisFrame())
        //{
        //    FireLightAttack();
        //}
        //if (_heavyAttack.IsPressed())
        //{
        //    FireHeavyAttack();
        //}
        //if (_dashAttack.IsPressed())
        //{
        //    FireDashAttack();
        //}
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


    //So preferably the system fires each event in succession once the previous one has ended
    //Is there a way to assign this in proper order?

    //Have a function that draws the hitboxes depending on the parameters given by each attack event

    public delegate void FirstEvent();
    public delegate void Event2();
    public delegate void Event3();
    public delegate void Event4();
    public delegate void Event5();
    public FirstEvent _event1;
    public Event2 _event2;
    public Event3 _event3;
    public Event4 _event4;
    public Event5 _event5;

    private Vector3 _gizmos;

    public bool ActiveAttack { get => _activeAttack; set => _activeAttack = value; }

    private bool _activeAttack;

    private bool _event1started = false;
    private bool _event1ended = false;
    private bool _event2ended = false;
    private bool _event3ended = false;
    private bool _event4ended = false;

    [SerializeField]
    private Transform _pointer;

    void Awake()
    {
        _evolutionSystem = GetComponent<EvolutionSystem>();
        Application.targetFrameRate = 60;

    }
    private void Update()
    {
    }
    private void OnDrawGizmos()
    {
        if (ActiveAttack)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.clear;

        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Gizmos.DrawCube(_pointer.position, _gizmos);
    }

    public void HitDetection(float x, float y, float z)
    {
        _gizmos = new Vector3(x, y, z);
        Vector3 hitbox = new Vector3(x, y, z);
        _pointer.localPosition = new Vector3(hitbox.x / 2, 0, 0);
        //hitboxPosition = new Vector3(hitboxPosition.x + x / 2, hitboxPosition.y, hitboxPosition.z);
        Collider[] colliders = Physics.OverlapBox(_pointer.position, hitbox / 2, transform.rotation);

        //Vector3 hitboxSize = new Vector3(0.5f, 0.5f, 2);
        //Vector3 overlapBoxPosition = transform.position + transform.forward;
        //Collider[] colliders = Physics.OverlapBox(overlapBoxPosition, hitboxSize/*, Quaternion.identity, _hurtboxLayer*/);
        if (colliders.Length > 0)
        {
            //Debug.Log("Hit");
            foreach (var collider in colliders)
            {
                //Checks to see if one of the colliders we hit was a player
                //And then check if the player isn't us
                if (collider.GetComponent<HealthSystem>() != null && collider.gameObject != gameObject)
                {
                    //The 1 represents the damage of the attack, ideally this will change to a variable
                    collider.GetComponent<HealthSystem>().GetHit(1);
                    //0 because 0 is the LightAttack index (Heavy is 1 and Dash is 2)
                    _evolutionSystem.SuccesfulHit(0);
                }
            }

        }
    }

    public void HitDetection(float radius)
    {
        Vector3 position = new Vector3(transform.position.x + radius / 2, transform.position.y, transform.position.z);
        Collider[] colliders = Physics.OverlapSphere(position, radius);

        if (colliders.Length > 0)
        {
            foreach (var collider in colliders)
            {
                if (collider.GetComponent<HealthSystem>() != null && collider.gameObject != gameObject)
                {
                    collider.GetComponent<HealthSystem>().GetHit(1);
                    _evolutionSystem.SuccesfulHit(0);
                }
            }
        }
    }
}


