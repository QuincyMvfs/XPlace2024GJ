using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement3D : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField] private float _moveLeftRightSpeed = 5.0f;
    [SerializeField] private float _airbornLeftRightSpeed = 1.0f;
    [SerializeField] private float _moveForwardSpeed = 5.0f;
    [SerializeField] private float _lowJumpHeight = 5.0f;
    [SerializeField] private float _mediumJumpHeight = 10.0f;
    [SerializeField] private float _rampJumpHeight = 10.0f;
    [SerializeField] private float _gravity = -9.81f;

    [Header("Limiters")]
    [SerializeField] private float _maxDistanceLeftRight = 5.0f;

    [Header("PlayerMesh")]
    [SerializeField] private GameObject _playerGameObject;
    [SerializeField] private LayerMask _layerMask;

    public delegate void JumpHandler(bool isInAir);
    public event JumpHandler OnJumpEvent;

    private Vector3 _gravityDir = new Vector3(0, -9.81f, 0);
    private Vector3 _forwardDir = new Vector3(0, 0, 10);

    protected IEnumerator _currentState;
    private bool _isFalling = false;

    public bool IsFalling => _isFalling;

    private TrickController _trickController;
    private Rigidbody _playerMeshRB;

    public Vector3 PlayerPosition => _playerGameObject.transform.position;

    private void Awake()
    {
        _playerMeshRB = _playerGameObject.GetComponent<Rigidbody>();
        _playerMeshRB.useGravity = false;
        _forwardDir.z = _moveForwardSpeed * 5;

        _gravityDir.y = _gravity;
        _trickController = GetComponent<TrickController>();
    }

    // Custom Gravity
    private void FixedUpdate()
    {
        _playerMeshRB.AddForce(_gravityDir, ForceMode.Acceleration);
        transform.transform.Translate(_forwardDir * Time.fixedDeltaTime);

        if (Physics.Raycast(_playerMeshRB.transform.position, -Vector3.up, out RaycastHit hitInfo, 0.5f, _layerMask)) 
        {
            if (_isFalling)
            {
                _trickController.SetCanTrick(false);
                _isFalling = false;
                OnJumpEvent(false);
            }
        }
        else 
        {
            _isFalling = true; 
        }
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

    public void Jump(JumpPowerType jumpPower)
    {
        if (_isFalling == true) return;

        Vector3 JumpForce = new Vector3(0, CalculateJumpHeight(jumpPower) * 100, 0);
        _playerMeshRB.AddForce(JumpForce, ForceMode.Force);
        _trickController.SetCanTrick(true);
        OnJumpEvent(true);
    }

    public void ForceJump(JumpPowerType jumpPower)
    {
        Vector3 JumpForce = new Vector3(0, CalculateJumpHeight(jumpPower) * 100, 0);
        _playerMeshRB.AddForce(JumpForce, ForceMode.Force);
        _trickController.SetCanTrick(true);
        OnJumpEvent(true);
    }

    private float CalculateJumpHeight(JumpPowerType jumpPower)
    {
        float NewJumpHeight = 0;
        switch (jumpPower)
        {
            case JumpPowerType.Small:
                NewJumpHeight = _lowJumpHeight + (_moveForwardSpeed * 0.5f);
                break;
            case JumpPowerType.Medium:
                NewJumpHeight = _mediumJumpHeight + (_moveForwardSpeed * 0.5f);
                break;
            case JumpPowerType.Large:
                NewJumpHeight = _rampJumpHeight + (_moveForwardSpeed * 0.5f);
                break;
        }


        return NewJumpHeight;
    }
    public void ChangeRampJumpHeight(float newHeight) 
    {
        _rampJumpHeight = newHeight;
    }

}


public enum JumpPowerType
{
    Small, Medium, Large
}
