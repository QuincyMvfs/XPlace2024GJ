using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrickController : MonoBehaviour
{
    [SerializeField] private TrickSetSO _trickSet;
    [SerializeField] private bool _alwaysTrick = false;
    [SerializeField] private float _inputWaitTime = 0.2f;

    private List<TrickButtons> _inputButtons = new List<TrickButtons>();

    private bool _canReceiveInputs = false;
    private bool _inputGiven = false;
    private PlayerMovement3D _movement;
    private ScoreController _scoreController;

    [System.Serializable]
    public class TrickSuccessEvent : UnityEvent<bool> { }
    [HideInInspector] public TrickSuccessEvent OnTrickSuccessEvent;

    public UnityEvent<TrickButtons> OnInputReceived;
    public UnityEvent OnDisplayTrickSuccessText;

    private void Awake()
    {
        _scoreController = GetComponent<ScoreController>();
        _movement = GetComponent<PlayerMovement3D>();
    }

    public void SetCanTrick(bool canTrick)
    {
        _canReceiveInputs = canTrick;
        if (!canTrick)
        {
            if (!CheckTrickSuccess() && _alwaysTrick && _inputButtons.Count <= 0)
            {
                _scoreController.BreakCombo();
            }
            else if (!CheckTrickSuccess() && _inputButtons.Count > 0)
            {
                _scoreController.BreakCombo();
            }

            _inputButtons.Clear();
        }

        _inputGiven = false;
    }

    public void BreakCurrentTrick()
    {
        _inputButtons.Clear();
        _scoreController.BreakCombo();
        OnTrickSuccessEvent.Invoke(true);
        _inputGiven = false;
    }

    public void ReceiveTrickInput(TrickButtons Button)
    {
        if (!_canReceiveInputs) return;

        _inputGiven = true;
        _inputButtons.Add(Button);
        OnInputReceived.Invoke(Button);

        bool isTrickSuccess = !CheckTrickSuccess();
        if (!isTrickSuccess && !_movement.IsFalling)
        {
            _inputButtons.Clear();
            _scoreController.BreakCombo();
        }
    }

    public bool CheckTrickSuccess()
    {
        // No buttons were pressed
        if (_inputButtons.Count <= 0 && !_inputGiven)
        {
            if (_alwaysTrick) { _scoreController.BreakCombo(); }
            _inputButtons.Clear();

            return false;
        }
        else if (_inputButtons.Count <= 0 && _inputGiven) return true;

        // For each combo in the trick set
        int tooLongInputs = 0;
        for (int i = 0; i < _trickSet.TrickCombos.Count; i++)
        {
            int correctInputs = 0;
            for (int j = 0; j < _inputButtons.Count; j++)
            {
                // If the input amount of buttons is more than the given combo, no match
                if (_trickSet.TrickCombos[i].Combo.Count != _inputButtons.Count) 
                {
                    if (_trickSet.TrickCombos[i].Combo.Count < _inputButtons.Count)
                    {
                        tooLongInputs++;
                        if (tooLongInputs >= _trickSet.TrickCombos.Count)
                        {
                            _scoreController.BreakCombo();
                            _inputButtons.Clear();
                            return false;
                        }
                    }

                    break; 
                }

                // If the given combos j index of button is the same as the input j, thats the correct input.
                if (_trickSet.TrickCombos[i].Combo[j] == _inputButtons[j]) { correctInputs++; }
                else { break; }

                // If the amount of correct inputs it equal to the length of the combo and input, return true
                if (correctInputs == _trickSet.TrickCombos[i].Combo.Count && _inputButtons.Count == correctInputs)
                {
                    Debug.Log($"Correct Count: {correctInputs} | Combo Count: {_trickSet.TrickCombos[i].Combo.Count} | Input Count: {_inputButtons.Count}");
                    OnDisplayTrickSuccessText.Invoke();
                    _scoreController.AddScore();
                    _inputButtons.Clear();
                    OnTrickSuccessEvent.Invoke(true);
                    return true;
                }
            }
        }

        return false;
    }
}

public enum TrickButtons
{
    Left, Right, Up, Down
}