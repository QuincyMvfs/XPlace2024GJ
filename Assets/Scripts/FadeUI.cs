using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    [SerializeField] private float _fadeSpeed = 5.0f;
    [SerializeField] private float _fadeDelay = 2.0f;

    private IEnumerator _currentState;
    private Image _fillImage;

    private void Awake()
    {
        _fillImage = GetComponent<Image>();
        Color color = Color.black;
        color.a = 1;
        _fillImage.color = color;
        EndOfLevel endOfLevel = FindObjectOfType<EndOfLevel>();
        endOfLevel.LevelFinished.AddListener(FadeIn);
        StartFade(false);
    }

    private void ChangeState(IEnumerator newState)
    {
        if (_currentState != null) StopCoroutine(_currentState);

        _currentState = newState;
        StartCoroutine(_currentState);
    }

    private void FadeIn()
    {
        StartFade(true);
    }

    public void StartFade(bool fadeIn)
    {
        if (fadeIn) { ChangeState(FadeInState()); }
        else { ChangeState(FadeOutState()); }
    }

    private IEnumerator FadeOutState()
    {
        yield return new WaitForSeconds(_fadeDelay);

        Color newColor = Vector4.zero;
        newColor.a = 1;
        while (_fillImage.color.a > 0)
        {
            newColor.a -= Time.deltaTime * _fadeSpeed;
            _fillImage.color = newColor;
            yield return null;
        }
    }

    private IEnumerator FadeInState()
    {
        yield return new WaitForSeconds(_fadeDelay);

        Color newColor = Vector4.zero;
        newColor.a = 0;
        while (_fillImage.color.a < 1)
        {
            newColor.a += Time.deltaTime * _fadeSpeed;
            _fillImage.color = newColor;
            yield return null;
        }
    }
}
