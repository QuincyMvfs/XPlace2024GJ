using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickController : MonoBehaviour
{
    [SerializeField] private TrickSetSO _trickSet;

    private List<TrickButtons> _inputButtons = new List<TrickButtons>();

    private bool _canReceiveInputs = false;

    public void SetCanTrick(bool canTrick)
    {
        _canReceiveInputs = canTrick;
    }

    public void ReceiveTrickInput(TrickButtons Button)
    {
        if (!_canReceiveInputs) return;

        _inputButtons.Add(Button);
    }

    public bool CheckTrickSuccess()
    {
        // No buttons were pressed
        if (_inputButtons.Count <= 0) return false;

        // For each combo in the trick set
        for (int i = 0; i < _trickSet.TrickCombos.Count; i++)
        {
            int correctInputs = 0;
            for (int j = 0; j < _inputButtons.Count; j++)
            {
                // If the input amount of buttons is more than the given combo, no match
                if (_trickSet.TrickCombos[i].Combo.Count != _inputButtons.Count) { break; }

                // If the given combos j index of button is the same as the input j, thats the correct input.
                if (_trickSet.TrickCombos[i].Combo[j] == _inputButtons[j]) { correctInputs++; }
                else { break; }

                // If the amount of correct inputs it equal to the length of the combo and input, return true
                if (correctInputs == _trickSet.TrickCombos[i].Combo.Count && _inputButtons.Count == correctInputs)
                {
                    _inputButtons.Clear();
                    return true;
                }
            }
        }

        _inputButtons.Clear();
        return false;
    }
}

public enum TrickButtons
{
    Left, Right, Up, Down
}
