using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementSystem : MonoBehaviour
{
    private Vector2 _movement;
    Vector2 _rotation;

    [SerializeField]
    private float _speed;

    private Rigidbody _rb;

    public bool CanMove { get => _canMove; set => _canMove = value; }

    private bool _canMove = true;
    private HitstunSystem _hitstunSystem;

    public void OnMove(InputAction.CallbackContext context)
    {
        _movement = context.ReadValue<Vector2>();
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        _rotation = context.ReadValue<Vector2>();
    }

    private void Awake()
    {
        //References the hitstun system so we can make the Player immobile when they're stunned
        _hitstunSystem = GetComponent<HitstunSystem>();

        _rb = GetComponent<Rigidbody>();
        //_movement.Enable();
    }

    private void Update()
    {
        _canMove = !_hitstunSystem._isStunned;

        if (_canMove)
            CheckIfMove();
        if(!RotationDeadAngles())
        {
            DoRotation();
        }
        
    }

    bool RotationDeadAngles()
    {
        if(_rotation.x > 0.1f || _rotation.x < -0.1f || _rotation.y > 0.1f || _rotation.y < -0.1f)
        {
            return false;
        }
        return true;
    }

    void DoRotation()
    {
        Vector3 rotationTarget = new Vector3(_rotation.x, 0, _rotation.y);
        Quaternion toRotation = Quaternion.LookRotation(-rotationTarget, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 360);
    }

    private void CheckIfMove()
    {
        Vector2 movementVector = _movement;
        if(movementVector.x > 0 || movementVector.x < 0 || movementVector.y > 0 || movementVector.y <0)
        {
            float xFactor = movementVector.x;
            float yFactor = -movementVector.y;
            _rb.velocity = new Vector3(yFactor, 0, xFactor) * _speed;
        }
        else
        {
            _rb.velocity = new Vector3(0, 0, 0);
        }
        
    }
}
