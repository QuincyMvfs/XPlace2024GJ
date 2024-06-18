using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEditor.ShaderGraph.Internal;
using static UnityEngine.Rendering.DebugUI;
using Unity.VisualScripting;

public class PlayerMovement3D : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] private float _moveLeftRightSpeed = 5.0f;
    [SerializeField] private float _airbornLeftRightSpeed = 1.0f;
    [SerializeField] private float _moveForwardSpeed = 5.0f;
    [SerializeField] private float _slowedForwardMoveSpeed = 3.0f;
    [SerializeField] private float _maxSpeed = 15.0f;
    [SerializeField] private float _accelerationSpeed = 10.0f;

    [Header("Jumping Variables")]
    [SerializeField] private float _lowJumpHeight = 5.0f;
    [SerializeField] private float _mediumJumpHeight = 10.0f;
    [SerializeField] private float _rampJumpHeight = 10.0f;
    [SerializeField] private float _gravity = -9.81f;

    [Header("Limiters")]
    [SerializeField] private float _maxDistanceLeftRight = 5.0f;

    [Header("PlayerMesh")]
    [SerializeField] private GameObject _playerGameObject;
    [SerializeField] private LayerMask _layerMask;

    [Header("VFX")]
    [SerializeField] private GameObject _wakeVFX;
    [SerializeField] private GameObject _airTrailVFX;
    [SerializeField] private GameObject _landingSplashVFX;

    [Header("Audio")]
    [SerializeField] private GameObject _movingSFXGameObject;
    private AudioSource _movingSFXSource;
    private AudioClip _normalMovingSFX;
    [SerializeField] private AudioClip _fastMovingSFX;
    [SerializeField] private AudioClip _slowMovingSFX;

    [SerializeField] private AudioClip _smallJumpSFX;
    [SerializeField] private AudioClip _bigJumpSFX;
    [SerializeField] private GameObject _jumpSFX;
    private AudioSource _jumpSFXSource;

    [SerializeField] private AudioClip _smallLandingSFX;
    [SerializeField] private AudioClip _largeLandingSFX;
    [SerializeField] private GameObject _landingSFXGameObject;
    private AudioSource _landingSFXSource;
    private JumpPowerType _previousJumpPower;
    private float _nextLandingSFXTime = 0;

    [System.Serializable]
    public class JumpEvent : UnityEvent<bool> { }
    [HideInInspector] public JumpEvent OnJumpEvent;
    [HideInInspector] public UnityEvent OnAirborneEvent;
    [HideInInspector] public UnityEvent OnLandedEvent;
    [HideInInspector] public UnityEvent OnMoveLeftEvent;
    [HideInInspector] public UnityEvent OnMoveRightEvent;
    [HideInInspector] public UnityEvent OnMoveStraightEvent;

    private Vector3 _gravityDir = new Vector3(0, -9.81f, 0);
    private Vector3 _forwardDir = new Vector3(0, 0, 10);
    private Vector3 _newForwardDir = new Vector3(0, 0, 0);
    private float _currentAcceleration = 0;

    protected IEnumerator _currentState;
    protected IEnumerator _currentMovementState;
    private bool _isFalling = false;

    public bool IsFalling => _isFalling;
    private TrickController _trickController;
    private Rigidbody _playerMeshRB;

    private Vector3 _previousPosition = Vector3.zero;

    public Vector3 PlayerPosition => _playerGameObject.transform.position;

    private void Awake()
    {
        _landingSFXSource = _landingSFXGameObject.GetComponent<AudioSource>();
        _landingSFXSource.Stop();
        _jumpSFXSource = _jumpSFX.GetComponent<AudioSource>();
        _jumpSFXSource.Stop();
        _landingSplashVFX.SetActive(false);

        _airTrailVFX.SetActive(false);

        _movingSFXSource = _movingSFXGameObject.GetComponent<AudioSource>();
        _normalMovingSFX = _movingSFXSource.clip;

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

        if (_currentAcceleration < _forwardDir.z)
        {
            _currentAcceleration += Time.deltaTime * _accelerationSpeed;
            Mathf.Clamp(_currentAcceleration, 0, _forwardDir.y);
            _newForwardDir.z = _currentAcceleration;
        }

        
    }

    // Forward movement and falling 
    private void Update()
    {
        transform.transform.Translate(_newForwardDir * Time.deltaTime);

        if (Physics.Raycast(_playerMeshRB.transform.position, -Vector3.up, out RaycastHit hitInfo, 0.5f, _layerMask))
        {
            if (_isFalling)
            {
                _trickController.SetCanTrick(false);
                _isFalling = false;
                OnJumpEvent.Invoke(false);

                if (Time.time > _nextLandingSFXTime)
                {
                    _nextLandingSFXTime = Time.time + 0.5f;
                    if (_previousJumpPower == JumpPowerType.Small) { PlayLandingSFX(_smallLandingSFX); }
                    else { PlayLandingSFX(_largeLandingSFX); }
                }

                if (_jumpSFXSource != null && _jumpSFXSource.clip == _bigJumpSFX) { }
                if (!_movingSFXGameObject.activeInHierarchy) { _movingSFXGameObject.SetActive(true); }

                if (!_wakeVFX.activeInHierarchy) { _wakeVFX.SetActive(true); _airTrailVFX.SetActive(false); }
                if (!_landingSplashVFX.activeInHierarchy) { _landingSplashVFX.SetActive(true); }
                else { _landingSplashVFX.SetActive(false); _landingSplashVFX.SetActive(true); }
            }
        }
        else
        {
            _isFalling = true;
            if (_movingSFXGameObject.activeInHierarchy) { _movingSFXGameObject.SetActive(false); }
            if (!_airTrailVFX.activeInHierarchy) { _wakeVFX.SetActive(false); _airTrailVFX.SetActive(true); }
        }
    }

    private void ChangeMovementState(IEnumerator newState)
    {
        if (_currentMovementState != null) StopCoroutine(_currentMovementState);

        _currentMovementState = newState;
        StartCoroutine(_currentMovementState);
    }

    public void AddMovementSpeed(float value, float delay)
    {
        _currentAcceleration = _forwardDir.z * 5;

        _newForwardDir.z = _moveForwardSpeed * 5;
        _newForwardDir.z += value * 5;
        if (_newForwardDir.z > _maxSpeed * 5)
        {
            _newForwardDir.z = _maxSpeed * 5 ;
        }

        ChangeMovingSFX(_fastMovingSFX);
        ChangeMovementState(ResetSpeedUpState(delay));
    }

    private IEnumerator ResetSpeedUpState(float delay)
    {
        yield return new WaitForSeconds(delay);

        ResetMovementSpeed();
    }

    public void ResetMovementSpeed()
    {
        ChangeMovingSFX(_normalMovingSFX);

        _newForwardDir.z = _moveForwardSpeed * 5;
    }

    public void TemporarySlowDown(float slowDuration)
    {
        ChangeMovementState(SlowDownState(slowDuration));
    }

    private IEnumerator SlowDownState(float slowDuration)
    {
        ChangeMovingSFX(_slowMovingSFX);
        _currentAcceleration = _forwardDir.z * 5;
        _newForwardDir.z = _slowedForwardMoveSpeed * 5;

        yield return new WaitForSeconds(slowDuration);

        _currentAcceleration = _slowedForwardMoveSpeed * 5;
        _newForwardDir.z = _moveForwardSpeed * 5;
        _movingSFXSource.clip = _normalMovingSFX;
        ChangeMovingSFX(_normalMovingSFX);
    }

    public float GetCurrentSpeed(float interval)
    {
        float distance = Vector3.Distance(_previousPosition, PlayerPosition);
        _previousPosition = PlayerPosition;
        float multiplier = Mathf.Pow(10, 2);
        return Mathf.Round((distance / interval) * multiplier) / multiplier;
    }

    private void ChangeMovingSFX(AudioClip newClip)
    {
        _movingSFXSource.clip = newClip;
        if (_movingSFXGameObject.activeInHierarchy) { _movingSFXSource.Play(); }
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
        OnMoveLeftEvent.Invoke();

        while (PlayerPosition.x > -_maxDistanceLeftRight)
        {
            if (_isFalling)
            {
                _playerGameObject.transform.Translate(new Vector3(-(_airbornLeftRightSpeed * Time.deltaTime), 0, 0), Space.Self);
            }
            else
            {
                _playerGameObject.transform.Translate(new Vector3(-(_moveLeftRightSpeed * Time.deltaTime), 0, 0), Space.Self);
            }

            yield return null;
        }

        ChangeState(Stop());
    }

    private IEnumerator MoveRight()
    {
        OnMoveRightEvent.Invoke();

        while (PlayerPosition.x < _maxDistanceLeftRight)
        {
            if (_isFalling) 
            {
                _playerGameObject.transform.Translate(new Vector3((_airbornLeftRightSpeed * Time.deltaTime), 0, 0), Space.Self);
            }
            else 
            {
                _playerGameObject.transform.Translate(new Vector3((_moveLeftRightSpeed * Time.deltaTime), 0, 0), Space.Self);
            }

            yield return null;
        }
    }

    private IEnumerator Stop()
    {
        OnMoveStraightEvent.Invoke();

        yield return null;
    }

    public void Jump(JumpPowerType jumpPower)
    {
        if (_isFalling == true) return;

        OnAirborneEvent.Invoke();

        _previousJumpPower = jumpPower;
        Vector3 JumpForce = new Vector3(0, CalculateJumpHeight(jumpPower) * 100, 0);
        _playerMeshRB.AddForce(JumpForce, ForceMode.Force);

        if (jumpPower != JumpPowerType.Large) return;

        OnJumpEvent.Invoke(true);
        _trickController.SetCanTrick(true);
    }

    public void ForceJump(JumpPowerType jumpPower)
    {
        OnAirborneEvent.Invoke();

        _previousJumpPower = jumpPower;

        Vector3 JumpForce = new Vector3(0, CalculateJumpHeight(jumpPower) * 100, 0);
        _playerMeshRB.AddForce(JumpForce, ForceMode.Force);
        _trickController.SetCanTrick(true);
        OnJumpEvent.Invoke(true);
    }

    private float CalculateJumpHeight(JumpPowerType jumpPower)
    {
        float NewJumpHeight = 0;
        switch (jumpPower)
        {
            case JumpPowerType.Small:
                NewJumpHeight = _lowJumpHeight + ((_newForwardDir.z / 5) * 0.5f);
                ChangeJumpSFX(_smallJumpSFX);
                break;
            case JumpPowerType.Medium:
                NewJumpHeight = _mediumJumpHeight + ((_newForwardDir.z / 5) * 0.5f);
                ChangeJumpSFX(_smallJumpSFX);
                break;
            case JumpPowerType.Large:
                NewJumpHeight = _rampJumpHeight + ((_newForwardDir.z / 5) * 0.5f);
                ChangeJumpSFX(_bigJumpSFX);
                break;
        }

        return NewJumpHeight;
    }

    private void ChangeJumpSFX(AudioClip newClip)
    {
        _jumpSFXSource.clip = newClip;
        _jumpSFXSource.Play();
    }

    private void StopJumpSFX()
    {
        if (_jumpSFX == null) return;

        _jumpSFXSource.Stop();
    }

    private void PlayLandingSFX(AudioClip newClip)
    {
        _landingSFXSource.clip = newClip;
        _landingSFXSource.Play();

        OnLandedEvent.Invoke();
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
