using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField] private float _moveLeftRightSpeed = 5.0f;
    [SerializeField] private float _moveForwardSpeed = 5.0f;
    [SerializeField] private float _jumpHeight = 5.0f;

    [Header("Limiters")]
    [SerializeField] private float _maxDistanceLeftRight = 5.0f;

    [Header("PlayerMesh")]
    [SerializeField] private GameObject _playerGameObject;

    private Vector3 _startPosition;
    protected IEnumerator _currentState;

    public Vector3 PlayerPosition => _playerGameObject.transform.position;

    private void Start()
    {
        _startPosition = _playerGameObject.transform.position;
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

    private IEnumerator MoveLeft()
    {
        while (true)
        {
            if (PlayerPosition.x > -_maxDistanceLeftRight)
            {
                _playerGameObject.transform.Translate(new Vector3(-(_moveLeftRightSpeed * Time.fixedDeltaTime), PlayerPosition.y, PlayerPosition.z));
            }

            yield return null;
        }
    }

    private IEnumerator MoveRight()
    {
        while (true)
        {
            if (PlayerPosition.x < _maxDistanceLeftRight)
            {
                _playerGameObject.transform.Translate(new Vector3((_moveLeftRightSpeed * Time.fixedDeltaTime), PlayerPosition.y, PlayerPosition.z));
            }

            yield return null;
        }
    }

    private IEnumerator Stop()
    {
        yield return null;
    }
}
