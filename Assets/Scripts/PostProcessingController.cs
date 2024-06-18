using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingController : MonoBehaviour
{
    [SerializeField] private float _midSpeedRatio = 0.4f;
    [SerializeField] private float _highSpeedRatio = 0.8f;

    private float _currentSpeed = 0;
    private float _maxSpeed = 0;

    private Volume _globalVolume;
    private VolumeProfile _profile;
    private Vignette _vignette;
    private MotionBlur _motionBlur;
    private SpeedMeterUI _playerSpeed;
    private PlayerMovement3D _playerMovement;
    private bool _isPlaying = false;
    protected IEnumerator _currentState;


    private void Awake()
    {
        _playerSpeed = FindObjectOfType<SpeedMeterUI>();
        _playerMovement = FindObjectOfType<PlayerMovement3D>();
        _globalVolume = GetComponent<Volume>();
        _profile = _globalVolume.profile;

        _maxSpeed = _playerMovement.MaxSpeed * 7;

        if (_profile.TryGet<Vignette>(out Vignette vignette))
        {
            _vignette = vignette;
        }
        if (_profile.TryGet<MotionBlur>(out MotionBlur motionBlur))
        {
            _motionBlur = motionBlur;
        }

        StartCoroutine(GetSpeedState());
    }

    private IEnumerator GetSpeedState()
    {
        yield return new WaitForSeconds(2.0f);

        while (true)
        {
            _currentSpeed = _playerSpeed.Speed;

            if (_currentSpeed / _maxSpeed > _midSpeedRatio && !_isPlaying)
            {
                ChangeState(UpdateVignette());
            }
            else if (_currentSpeed / _maxSpeed < _midSpeedRatio && _isPlaying)
            {
                ChangeState(DeUpdateVignette());
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void ChangeState(IEnumerator newState)
    {
        if (_currentState != null) StopCoroutine(_currentState);

        _isPlaying = false;
        _currentState = newState;
        StartCoroutine(_currentState);
    }

    private IEnumerator UpdateVignette()
    {
        _isPlaying = true;
        ClampedFloatParameter _currentVignetteParameter = new ClampedFloatParameter(0, 0, 1);
        ClampedFloatParameter _currentMotionBlur = new ClampedFloatParameter(0, 0, 1);
        while (_currentSpeed / _maxSpeed > _midSpeedRatio && _currentVignetteParameter.value < 0.3f)
        {
            Debug.Log($"UPDATE VIGNETTE: {_currentSpeed / _maxSpeed}");
            _currentVignetteParameter.value += Time.deltaTime;
            _currentMotionBlur.value += Time.deltaTime * 2;
            _vignette.intensity.SetValue(_currentVignetteParameter);
            _motionBlur.intensity.SetValue(_currentMotionBlur);
            yield return null;
        }
    }

    private IEnumerator DeUpdateVignette()
    {
        yield return new WaitForSeconds(1.2f);
        
        ClampedFloatParameter _currentVignetteParameter = _vignette.intensity;
        ClampedFloatParameter _currentMotionBlur = _motionBlur.intensity;
        while (_currentVignetteParameter.value > 0)
        {
            Debug.Log($"DIEEEE: {_currentSpeed / _maxSpeed}");
            _currentVignetteParameter.value -= Time.deltaTime;
            _currentMotionBlur.value -= Time.deltaTime * 2;

            _vignette.intensity.SetValue(_currentVignetteParameter);
            _motionBlur.intensity.SetValue(_currentMotionBlur);
            yield return null;
        }

        _isPlaying = false;
    }

}
