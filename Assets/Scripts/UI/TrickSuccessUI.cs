using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickSuccessUI : MonoBehaviour
{
    [SerializeField] private float _visibleDuration = 0.2f;
    [SerializeField] private GameObject _trickSuccessText;
    [SerializeField] private TrickController _trickController;

    private void Awake()
    {
        _trickSuccessText.SetActive(false);
        _trickController.OnDisplayTrickSuccessText.AddListener(StartTextSequence);
    }

    private void StartTextSequence()
    {
        Debug.Log("START ME UP!");
        StartCoroutine(PerfectTrickState());
    }

    private IEnumerator PerfectTrickState()
    {
        _trickSuccessText.SetActive(true);
        yield return new WaitForSeconds(_visibleDuration);
        _trickSuccessText.SetActive(false);
    }
}
