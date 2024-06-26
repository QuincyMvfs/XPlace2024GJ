using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrickSuccessUI : MonoBehaviour
{
    [SerializeField] private float _visibleDuration = 0.2f;
    [SerializeField] private float _growShrinkSpeed = 3.0f;
    [SerializeField] private GameObject _trickSuccessText;
    [SerializeField] private TrickController _trickController;
    [SerializeField] private GameObject _trickSuccessSFX;
    [SerializeField] private AudioClip[] _trickSuccessSoundEffects = new AudioClip[0];
    private AudioSource _trickSuccessSFXSource;

    private ScoreController _scoreController;
    private string _perfectText;
    private string _failureText = "FAIL!";
    private float _showTextDelayTime = 0;
    private TextMeshProUGUI _text;
    private RectTransform _rectTransform;
    private Vector3 _startScale;
    private Color _perfectColor = Color.yellow;
    private Color _failureColor = Color.red;

    protected IEnumerator _currentState;

    private void Awake()
    {
        _scoreController = _trickController.GetComponent<ScoreController>();
        _rectTransform = _trickSuccessText.GetComponent<RectTransform>();
        _startScale = _rectTransform.localScale;
        _text = _trickSuccessText.GetComponent<TextMeshProUGUI>();
        _perfectColor = _text.color;
        _perfectText = _text.text;
        _trickSuccessSFXSource = _trickSuccessSFX.GetComponent<AudioSource>();
        _showTextDelayTime = Time.time + 1.0f;
        _trickSuccessText.SetActive(false);
        _trickController.OnDisplayTrickSuccessText.AddListener(ShowPerfectText);
        _scoreController.OnComboBrokenEvent.AddListener(ShowFailureText);
    }

    private void ChangeState(IEnumerator newState)
    {
        if (_currentState != null) StopCoroutine(_currentState);

        _currentState = newState;
        StartCoroutine(_currentState);
    }

    private void ShowPerfectText()
    {
        if (_showTextDelayTime > Time.time) return;

        ChangeState(PerfectTrickState());
    }

    private void ShowFailureText()
    {
        if (_showTextDelayTime > Time.time) return;

        ChangeState(FailureTrickState());
    }

    private IEnumerator PerfectTrickState()
    {
        if (_trickSuccessSFX != null)
        {
            int randomSound = Random.Range(0, _trickSuccessSoundEffects.Length);
            _trickSuccessSFXSource.clip = _trickSuccessSoundEffects[randomSound];
            _trickSuccessSFXSource.Play();
        }

        _trickSuccessText.SetActive(true);
        _text.text = _perfectText;
        _text.color = _perfectColor;
        Vector3 newScale = _rectTransform.localScale;
        while (_rectTransform.localScale.magnitude < _startScale.magnitude)
        {
            newScale = new Vector3(
                newScale.x += Time.deltaTime * _growShrinkSpeed,
                newScale.y += Time.deltaTime * _growShrinkSpeed,
                newScale.z += Time.deltaTime * _growShrinkSpeed);
            _rectTransform.localScale = newScale;
            yield return null;
        }

        yield return new WaitForSeconds(_visibleDuration / 3);

        while (_rectTransform.localScale.magnitude > 0.1)
        {
            newScale = new Vector3(
                newScale.x -= Time.deltaTime * _growShrinkSpeed,
                newScale.y -= Time.deltaTime * _growShrinkSpeed,
                newScale.z -= Time.deltaTime * _growShrinkSpeed);
            _rectTransform.localScale = newScale;
            yield return null;
        }

        _trickSuccessText.SetActive(false);
    }

    private IEnumerator FailureTrickState()
    {
        _trickSuccessText.SetActive(true);
        _text.text = _failureText;
        _text.color = _failureColor;
        _rectTransform.localScale = Vector3.zero;
        Vector3 newScale = _rectTransform.localScale;
        while (_rectTransform.localScale.magnitude < _startScale.magnitude)
        {
            newScale = new Vector3(
                newScale.x += Time.deltaTime * _growShrinkSpeed, 
                newScale.y += Time.deltaTime * _growShrinkSpeed, 
                newScale.z += Time.deltaTime * _growShrinkSpeed );
            _rectTransform.localScale = newScale;
            yield return null;
        }

        yield return new WaitForSeconds(_visibleDuration / 3);

        while (_rectTransform.localScale.magnitude > 0.1)
        {
            newScale = new Vector3(
                newScale.x -= Time.deltaTime * _growShrinkSpeed,
                newScale.y -= Time.deltaTime * _growShrinkSpeed,
                newScale.z -= Time.deltaTime * _growShrinkSpeed);
            _rectTransform.localScale = newScale;
            yield return null;
        }

        _trickSuccessText.SetActive(false);
    }
}
