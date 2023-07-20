using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementSystem : MonoBehaviour
{
    private Vector2 _movement;

    [SerializeField]
    private float _speed;

    private Transform _transform;

    private bool _canMove = true;
    private HitstunSystem _hitstunSystem;

    public void OnMove(InputAction.CallbackContext context)
    {
        _movement = context.ReadValue<Vector2>();
    }

    private void Awake()
    {
        //References the hitstun system so we can make the Player immobile when they're stunned
        _hitstunSystem = GetComponent<HitstunSystem>();

        _transform = GetComponent<Transform>();
        //_movement.Enable();
    }

    private void Update()
    {
        _canMove = !_hitstunSystem._isStunned;

        if (_canMove)
            CheckIfMove();
    }

    private void CheckIfMove()
    {
        Vector2 movementVector = _movement;
        float xFactor = 0;
        float yFactor = 0;
        xFactor = movementVector.x;
        yFactor = -movementVector.y;
        _transform.Translate(new Vector3(yFactor, 0, xFactor) * _speed);
    }
}
