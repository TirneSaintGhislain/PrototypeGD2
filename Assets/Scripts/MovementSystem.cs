using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementSystem : MonoBehaviour
{
    [SerializeField]
    private InputAction _movement;

    [SerializeField]
    private float _speed;

    private Transform _transform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _movement.Enable();
    }

    private void Update()
    {
        CheckIfMove();
        
    }

    private void CheckIfMove()
    {
        Vector2 movementVector = _movement.ReadValue<Vector2>();
        float xFactor = 0;
        float yFactor = 0;
        xFactor = movementVector.x;
        yFactor = -movementVector.y;
        _transform.Translate(new Vector3(yFactor, 0, xFactor) * _speed);
    }
}
