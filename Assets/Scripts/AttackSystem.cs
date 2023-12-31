using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackSystem : MonoBehaviour
{
    [SerializeField]
    public LayerMask _hurtboxLayer;

    [SerializeField]
    private bool _attackingFreezesPlayer;

    [SerializeField]
    private bool _attackHalfMovement;

    //Evolution System Variables
    public float HeavyArea { get; set; }
    public float DashDamage { get; set; }
    public int LightHits { get;  set; }

    private EvolutionSystem _evolutionSystem;
    private MovementSystem _movementSystem;
    private AttackValueManager _attackValueManager;

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
    public AttackTypes CurrentAttackType { get => _attackType; set => _attackType = value; }

    public Vector3 Gizmos { set => _gizmos = value; }

    public bool InStartupFrames { get; set; }
    public bool InActiveFrames { get; set; }

    private AttackTypes _attackType;
    private bool _activeAttack;

    private bool _event1started = false;
    private bool _event1ended = false;
    private bool _event2ended = false;
    private bool _event3ended = false;
    private bool _event4ended = false;

    [SerializeField]
    private Transform _pointer;
    [SerializeField]
    private MeshRenderer _pointerMeshRenderer;
    [SerializeField]
    private MeshFilter _pointerMeshFilter;
    [SerializeField]
    private Material[] _materials;
    [SerializeField]
    private Mesh[] _meshes;
    [SerializeField]
    private List<GameObject> _attackVFXs;

    public bool AlternateLightAttack { get; set; }
    public bool AlternateHeavyAttack { get; set; }
    public bool AlternateDashAttack { get; set; }


    void Awake()
    {
        _evolutionSystem = GetComponent<EvolutionSystem>();
        _movementSystem = GetComponent<MovementSystem>();
        Application.targetFrameRate = 60;
        InStartupFrames = false;
        InActiveFrames = false;

        _attackValueManager = GetComponent<AttackValueManager>();
        AttackSelection AS = FindObjectOfType<AttackSelection>();
        AS.SelectAttacks(this, GetComponent<MovementSystem>());
    }
    private void Update()
    {
        DrawHitbox();
        ChangeMovement();
    }

    public void UpdateAttackValues()
    {
        if(AlternateLightAttack)
        {
            GetComponent<AltLightAttack>()._attackDamage = _attackValueManager._lightAttackDamage;
            
        }
        else
        {
            GetComponent<LightAttack>()._attackDamage = _attackValueManager._lightAttackDamage;
        }

        if (AlternateHeavyAttack)
        {
            GetComponent<AltHeavyAttack>()._attackDamage = _attackValueManager._heavyAttackDamage;
        }
        else
        {
            GetComponent<HeavyAttack>()._attackDamage = _attackValueManager._heavyAttackDamage;
        }

        if (AlternateDashAttack)
        {
            GetComponent<AltDashAttack>()._attackDamage = _attackValueManager._dashAttackDamage;
        }
        else
        {
            GetComponent<DashAttack>()._attackDamage = _attackValueManager._dashAttackDamage;
        }
        //GetComponent<LightAttack>()._attackDamage = _attackValueManager._lightAttackDamage;
        //GetComponent<HeavyAttack>()._attackDamage = _attackValueManager._heavyAttackDamage;
        //GetComponent<DashAttack>()._attackDamage = _attackValueManager._dashAttackDamage;

        //GetComponent<LightAttack>()._currentRange = GetComponent<LightAttack>()._baseRange * _attackValueManager._lightAttackRange;
        //GetComponent<HeavyAttack>().Radius = _attackValueManager._heavyAttackRange;
        //GetComponent<DashAttack>()._currentRange = GetComponent<DashAttack>()._baseRange * _attackValueManager._dashAttackRange;

        //Debug.Log(GetComponent<LightAttack>()._currentRange);
    }

    void ChangeMovement()
    {
        if(ActiveAttack && _attackingFreezesPlayer)
        {
            _movementSystem.CanMove = false;
        }
        else if(ActiveAttack && _attackHalfMovement)
        {
            _movementSystem.HalfMovement = true;
        }
        else
        {
            _movementSystem.HalfMovement = false;
        }
    }

    public void EnableMovement()
    {
        _movementSystem.CanMove = true;
    }

    void DrawHitbox()
    {
        if(InStartupFrames)
        {
            Material mat = _materials[(int)_attackType];
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.4f);
            _materials[(int)_attackType].color = mat.color;
            _pointerMeshRenderer.enabled = true;
            _pointerMeshRenderer.material = _materials[(int)_attackType];
            _pointerMeshFilter.mesh = _meshes[(int)_attackType];
            _pointer.localScale = _gizmos;
            if (_attackType >= 0 && (int)_attackType < _attackVFXs.Count)
                _attackVFXs[(int)_attackType].SetActive(true);
        }
        else if (InActiveFrames)
        {
            // Can also be made visible only in the editor:
            // put #if UNITY_EDITOR above the if statement and #endif after the brackets of the else statement
            Material mat = _materials[(int)_attackType];
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.9f);
            _materials[(int)_attackType].color = mat.color;
            _pointerMeshRenderer.enabled = true;
            _pointerMeshRenderer.material = _materials[(int)_attackType];
            _pointerMeshFilter.mesh = _meshes[(int)_attackType];
            _pointer.localScale = _gizmos;
            foreach (var vfx in _attackVFXs)
            {
                vfx.SetActive(false);
            }
        }
        else
        {
            _pointerMeshRenderer.enabled = false;
            foreach (var vfx in _attackVFXs)
            {
                vfx.SetActive(false);
            }
        }
    }

    public void HitDetection(Vector3 size, float stunTime, float knockBackStrength, int attackDamage)
    {
        _gizmos = size;
        Vector3 hitbox = size;
        _pointer.localPosition = new Vector3(hitbox.x / 2, 0, 0);
        Collider[] colliders = Physics.OverlapBox(_pointer.position, hitbox / 2, transform.rotation);

        if (colliders.Length > 0)
        {
            foreach (var collider in colliders)
            {
                //Checks to see if one of the colliders we hit was a player
                //And then check if the player isn't us
                if (collider.GetComponent<HealthSystem>() != null && collider.gameObject != gameObject)
                {
                    //The 1 represents the damage of the attack, ideally this will change to a variable
                    collider.GetComponent<HealthSystem>().GetHit(attackDamage, stunTime, knockBackStrength);
                    //0 because 0 is the LightAttack index (Heavy is 1 and Dash is 2)
                    _evolutionSystem.SuccesfulHit((int)_attackType);
                }
            }

        }
    }

    public void HitDetection(float radius, float hitStunTime, float knockBackStrength, int attackDamage)
    {
        _gizmos = new Vector3(radius, radius, radius);
        _pointer.localPosition = new Vector3(radius / 2, 0, 0);
        //Vector3 position = new Vector3(transform.position.x + radius / 2, transform.position.y, transform.position.z);
        Collider[] colliders = Physics.OverlapSphere(_pointer.position, radius / 2);

        if (colliders.Length > 0)
        {
            foreach (var collider in colliders)
            {
                if (collider.GetComponent<HealthSystem>() != null && collider.gameObject != gameObject)
                {
                    collider.GetComponent<HealthSystem>().GetHit(attackDamage, hitStunTime, knockBackStrength);
                    _evolutionSystem.SuccesfulHit((int)_attackType);
                }
            }
        }
    }
}

public enum AttackTypes
{
    LightAttack, HeavyAttack, DashAttack
}


