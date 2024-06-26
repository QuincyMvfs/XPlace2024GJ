using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TrickController : MonoBehaviour
{
    [SerializeField] private TrickSetSO[] _trickSets;
    private TrickSetSO _trickSet;
    [SerializeField] private bool _alwaysTrick = false;

    [Header("SFX")]
    [SerializeField] private GameObject _trickInputSFXGameObject;
    [SerializeField] private AudioClip[] _trickInputSuccessSoundEffects = new AudioClip[0];
    [SerializeField] private AudioClip[] _trickInputFailureSoundEffects = new AudioClip[0];
    private AudioSource _trickInputSFXSource;

    [Header("VFX")]
    [SerializeField] private GameObject _trickSuccessVFX;

    private List<TrickButtons> _inputButtons = new List<TrickButtons>();
    private bool _canReceiveInputs = false;
    private bool _inputGiven = false;
    private PlayerMovement3D _movement;
    private ScoreController _scoreController;


    [System.Serializable]
    public class TrickSuccessEvent : UnityEvent<bool> { }
    [HideInInspector] public TrickSuccessEvent OnTrickSuccessEvent;
    [HideInInspector] public UnityEvent<TrickButtons> OnInputReceived;
    [HideInInspector] public UnityEvent OnDisplayTrickSuccessText;
    [HideInInspector] public UnityEvent OnDisplayTrickFailText;


    public int TrickLength = 0;

    private void Awake()
    {
        GetTrickSet();
        _trickInputSFXSource = _trickInputSFXGameObject.GetComponent<AudioSource>();
        _scoreController = GetComponent<ScoreController>();
        _movement = GetComponent<PlayerMovement3D>();
    }

    private void GetTrickSet()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        switch (currentScene)
        {
            case "Level_1_America":
                _trickSet = _trickSets[0];
                break;
            case "Level_2_Asia":
                _trickSet = _trickSets[1];
                break;
            case "Level_3_MiddleEast":
                _trickSet = _trickSets[2];
                break;
            case "Level_4_Europe":
                _trickSet = _trickSets[3];
                break;
        }
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

        bool isTrickSuccess = CheckTrickSuccess();
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
                if (_trickSet.TrickCombos[i].Combo[j] == _inputButtons[j]) 
                {
                    PlayInputSFX(true);
                    correctInputs++; 
                }
                else { break; }

                // If the amount of correct inputs it equal to the length of the combo and input, return true
                if (correctInputs == _trickSet.TrickCombos[i].Combo.Count && _inputButtons.Count == correctInputs
                    && correctInputs == TrickLength)
                {
                    OnDisplayTrickSuccessText.Invoke();
                    _scoreController.AddScore();
                    _inputButtons.Clear();
                    OnTrickSuccessEvent.Invoke(true);
                    PlayTrickVFX();
                    return true;
                }
            }
        }

        return false;
    }

    public void PlayInputSFX(bool isCorrectInput)
    {
        int randomSound = 0;
        if (isCorrectInput)
        {
            randomSound = Random.Range(0, _trickInputSuccessSoundEffects.Length);
            _trickInputSFXSource.clip = _trickInputSuccessSoundEffects[randomSound];
            _trickInputSFXSource.Play();
        }
        else
        {
            randomSound = Random.Range(0, _trickInputFailureSoundEffects.Length);
            _trickInputSFXSource.clip = _trickInputFailureSoundEffects[randomSound];
            _trickInputSFXSource.Play();
            OnDisplayTrickFailText.Invoke();
        }
    }

    private void PlayTrickVFX()
    {
        if (_trickSuccessVFX != null)
        {
            if (_trickSuccessVFX.activeInHierarchy)
            {
                _trickSuccessVFX.SetActive(false);
                _trickSuccessVFX.SetActive(true);
            }
            else
            {
                _trickSuccessVFX.SetActive(true);
            }
        }
    }
}

public enum TrickButtons
{
    Left, Right, Up, Down
}
