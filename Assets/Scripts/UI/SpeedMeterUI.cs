using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedMeterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _speedText;
    [SerializeField] private PlayerMovement3D _playerMovement;
    [SerializeField] private float _checkInterval = 0.3f;

    private string _startText;

    // Start is called before the first frame update
    void Start()
    {
        _startText = _speedText.text;
        StartCoroutine(SpeedCheckState());
    }

    private IEnumerator SpeedCheckState()
    {
        while (true)
        {
            _speedText.text = _startText + _playerMovement.GetCurrentSpeed(_checkInterval).ToString();

            yield return new WaitForSeconds(_checkInterval);
        }
    }
}
