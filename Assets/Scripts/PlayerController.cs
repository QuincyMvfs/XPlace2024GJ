using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement3D))]
[RequireComponent(typeof(TrickController))]

public class PlayerController : MonoBehaviour
{
    private PlayerMovement3D _characterMovement;
    private TrickController _trickController;
    private PlayerInput _playerInput;

    private bool _isMovingLeft = false;
    private bool _isMovingRight = false;
    private MovementDirections _currentDirection = MovementDirections.Stop;

    private void Awake()
    {
        _characterMovement = GetComponent<PlayerMovement3D>();
        _trickController = GetComponent<TrickController>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void OnMoveLeft()
    {
        _isMovingLeft = !_isMovingLeft;

        if (_isMovingLeft)
        {
            _currentDirection = MovementDirections.Left;
            _characterMovement.ChangeMovementState(MovementDirections.Left);
        }
        else if (_currentDirection == MovementDirections.Left)
        {
            _characterMovement.ChangeMovementState(MovementDirections.Stop);
        }
    }

    private void OnMoveRight()
    {
        _isMovingRight = !_isMovingRight;

        if (_isMovingRight)
        {
            _currentDirection = MovementDirections.Right;
            _characterMovement.ChangeMovementState(MovementDirections.Right);

        }
        else if (_currentDirection == MovementDirections.Right)
        {
            _characterMovement.ChangeMovementState(MovementDirections.Stop);
        }
    }
    
    private void OnJump()
    {
        _characterMovement.Jump(JumpPowerType.Small);
    }

    private void OnLeftArrow()
    {
        _trickController.ReceiveTrickInput(TrickButtons.Left);
    }

    private void OnRightArrow()
    {
        _trickController.ReceiveTrickInput(TrickButtons.Right);
    }

    private void OnUpArrow()
    {
        _trickController.ReceiveTrickInput(TrickButtons.Up);
    }

    private void OnDownArrow()
    {
        _trickController.ReceiveTrickInput(TrickButtons.Down);
    }

    private void OnPause()
    {
        
        if(_playerInput != null)
        {
            _playerInput.enabled = false;
        }

        GameManager.Instance.PauseGame(this);
    }

    public void UnpauseController()
    {
        _playerInput.enabled = true;
    }
}

public enum MovementDirections
{
    Left, Right, Stop
}