using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickSuccessUI : MonoBehaviour
{
    [SerializeField] private float _visibleDuration = 0.2f;
    [SerializeField] private GameObject _trickSuccessText;
    [SerializeField] private TrickController _trickController;
    [SerializeField] private GameObject _trickSuccessGameObject;
    [SerializeField] private AudioClip[] _trickSuccessSoundEffects = new AudioClip[0];
    private AudioSource _trickSuccessSFXSource;


    private void Awake()
    {
        _trickSuccessSFXSource = _trickSuccessGameObject.GetComponent<AudioSource>();

        _trickSuccessText.SetActive(false);
        _trickController.OnDisplayTrickSuccessText.AddListener(StartTextSequence);
    }

    private void StartTextSequence()
    {
        StartCoroutine(PerfectTrickState());
    }

    private IEnumerator PerfectTrickState()
    {
        if (_trickSuccessGameObject != null)
        {
            int randomSound = Random.Range(0, _trickSuccessSoundEffects.Length);
            _trickSuccessSFXSource.clip = _trickSuccessSoundEffects[randomSound];
            _trickSuccessSFXSource.Play();
        }

        _trickSuccessText.SetActive(true);
        yield return new WaitForSeconds(_visibleDuration);
        _trickSuccessText.SetActive(false);
    }
}
