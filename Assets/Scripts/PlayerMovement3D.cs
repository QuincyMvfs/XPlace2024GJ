using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField] private float _moveLeftRightSpeed = 5.0f;
    [SerializeField] private float _airbornLeftRightSpeed = 1.0f;
    [SerializeField] private float _moveForwardSpeed = 5.0f;
    [SerializeField] private float _jumpHeight = 5.0f;
    [SerializeField] private float _jumpSpeed = 1.25f;
    [SerializeField] private float _jumpApexSpeed = 0.25f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _airTime = 0.50f;

    [Header("Limiters")]
    [SerializeField] private float _maxDistanceLeftRight = 5.0f;

    [Header("PlayerMesh")]
    [SerializeField] private GameObject _playerGameObject;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private LayerMask _layerMask;

    protected Vector3 _startPosition;
    private Vector3 _gravityDir = new Vector3(0, -9.81f, 0);
    protected IEnumerator _currentState;
    private bool _isFalling = false;


    public Vector3 PlayerPosition => _playerGameObject.transform.position;

    private void Awake()
    {
        _rb.useGravity = false;
        _gravityDir.y = _gravity;
    }

    private void Start()
    {
        _startPosition = PlayerPosition;
    }

    // Custom Gravity
    private void FixedUpdate()
    {
        _rb.AddForce(_gravityDir, ForceMode.Acceleration);

        if (Physics.Raycast(_rb.transform.position, -Vector3.up, out RaycastHit hitInfo, 0.5f, _layerMask)) {
            _isFalling = false;
        }
        else { _isFalling = true; }
    }

    public void ChangeMovementState(MovementDirections Direction)
    {
        switch (Direction)
        {
            case MovementDirections.Left:
                ChangeState(MoveLeft());
                break;

            case MovementDirections.Right:
                ChangeState(MoveRight());
                break;

            case MovementDirections.Stop:
                ChangeState(Stop());
                break;
        }
    }

    private void ChangeState(IEnumerator newState)
    {
        if (_currentState != null) StopCoroutine(_currentState);

        _currentState = newState;
        StartCoroutine(_currentState);
    }

    // LEFT RIGHT
    private IEnumerator MoveLeft()
    {
        while (PlayerPosition.x > -_maxDistanceLeftRight)
        {
            if (_isFalling)
            {
                _playerGameObject.transform.Translate(new Vector3(-(_airbornLeftRightSpeed * Time.fixedUnscaledDeltaTime), 0, 0), Space.Self);
            }
            else
            {
                _playerGameObject.transform.Translate(new Vector3(-(_moveLeftRightSpeed * Time.fixedUnscaledDeltaTime), 0, 0), Space.Self);
            }

            yield return null;
        }
    }

    private IEnumerator MoveRight()
    {
        while (PlayerPosition.x < _maxDistanceLeftRight)
        {
            if (_isFalling) 
            {
                _playerGameObject.transform.Translate(new Vector3((_airbornLeftRightSpeed * Time.fixedUnscaledDeltaTime), 0, 0), Space.Self);
            }
            else 
            {
                _playerGameObject.transform.Translate(new Vector3((_moveLeftRightSpeed * Time.fixedUnscaledDeltaTime), 0, 0), Space.Self);
            }

            yield return null;
        }
    }
    //

    private IEnumerator Stop()
    {
        yield return null;
    }

    public void Jump()
    {
        if (_isFalling == true) return;

        Vector3 JumpForce = new Vector3(0, CalculateJumpHeight() * 100, 0);
        _rb.AddForce(JumpForce, ForceMode.Force);
    }

    private float CalculateJumpHeight()
    {
        float NewJumpHeight = 0;
        NewJumpHeight = _jumpHeight + (_moveForwardSpeed * 0.5f);

        return NewJumpHeight;
    }
}
