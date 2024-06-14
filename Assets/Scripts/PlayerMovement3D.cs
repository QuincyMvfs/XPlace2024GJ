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
    [SerializeField] private float _airTime = 0.50f;

    [Header("Limiters")]
    [SerializeField] private float _maxDistanceLeftRight = 5.0f;

    [Header("PlayerMesh")]
    [SerializeField] private GameObject _playerGameObject;

    protected Vector3 _startPosition;
    protected IEnumerator _currentState;
    private bool _isFalling = false;

    public Vector3 PlayerPosition => _playerGameObject.transform.position;

    private void Start()
    {
        _startPosition = PlayerPosition;
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

        StartCoroutine(Jumping());
    }

    private IEnumerator Jumping()
    {
        _isFalling = true;
        float JumpHeight = CalculateJumpHeight();
        while (PlayerPosition.y < JumpHeight)
        {
            float NewY;
            if (PlayerPosition.y / JumpHeight > 0.8f) 
            {
                NewY = Mathf.Lerp(PlayerPosition.y, JumpHeight + 0.1f, _jumpApexSpeed * Time.fixedUnscaledDeltaTime);
            }
            else
            {
                NewY = Mathf.Lerp(PlayerPosition.y, JumpHeight + 0.1f, _jumpSpeed * Time.fixedUnscaledDeltaTime);
            }
            Vector3 NewPosition = new Vector3(PlayerPosition.x, NewY, PlayerPosition.z);
            _playerGameObject.transform.position = NewPosition;
            yield return null;
        }

        StartCoroutine(Falling());
    }

    private IEnumerator Falling()
    {
        while (PlayerPosition.y > _startPosition.y)
        {
            float NewY = Mathf.Lerp(PlayerPosition.y, _startPosition.y - 0.1f, (_jumpSpeed * Time.fixedUnscaledDeltaTime));
            Vector3 NewPosition = new Vector3(PlayerPosition.x, NewY, PlayerPosition.z);
            _playerGameObject.transform.position = NewPosition;
            yield return null;
        }

        _isFalling = false;
    }

    private float CalculateJumpHeight()
    {
        float NewJumpHeight = 0;
        NewJumpHeight = _jumpHeight + (_moveForwardSpeed * 0.5f);

        return NewJumpHeight;
    }

    //private void Update()
    //{
    //    if (PlayerPosition.y > CalculateJumpHeight())
    //    {
    //        _playerGameObject.transform.position = new Vector3(PlayerPosition.x, CalculateJumpHeight() + _startPosition.y, PlayerPosition.z);
    //    }
    //    else if (PlayerPosition.y < _startPosition.y)
    //    {
    //        _playerGameObject.transform.position = new Vector3(PlayerPosition.x, _startPosition.y, PlayerPosition.z);

    //    }
    //}
}
