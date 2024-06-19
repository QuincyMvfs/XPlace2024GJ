using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrickUIManager : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float _growShrinkSpeed = 3.0f; 

    [Header("Slots")]
    [SerializeField] private TrickSetSO[] _trickSets;
    [SerializeField] private TrickSetSO _trickSet;
    [SerializeField] private Sprite _upArrow;
    [SerializeField] private Sprite _downArrow;
    [SerializeField] private Sprite _leftArrow;
    [SerializeField] private Sprite _rightArrow;
    [SerializeField] private GameObject _trickImage;
    [SerializeField] private GameObject _trickSuccessText;
    [SerializeField] private PlayerMovement3D _playerMovementComponent;
    
    private TrickController _trickControllerComponent;
    private List<GameObject> _trickImages = new List<GameObject>();

    private Dictionary<TrickButtons, Sprite> _arrowSprites = new Dictionary<TrickButtons, Sprite>();
    private int _trickLength = 0;
    private RectTransform _rectTransform;
    private Vector3 _startScale;
    protected IEnumerator _currentState;


    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _startScale = _rectTransform.localScale;
        GetTrickSet();
    }

    private void Start()
    {
        _trickSuccessText.SetActive(false);
        _arrowSprites.Add(TrickButtons.Up, _upArrow);
        _arrowSprites.Add(TrickButtons.Down, _downArrow);
        _arrowSprites.Add(TrickButtons.Right, _rightArrow);
        _arrowSprites.Add(TrickButtons.Left, _leftArrow);
        
        if(_playerMovementComponent!= null)
        {
            _playerMovementComponent.OnJumpEvent.AddListener(DisplayTrick);

            if(_trickControllerComponent = _playerMovementComponent.GetComponent<TrickController>())
            {
                _trickControllerComponent.OnTrickSuccessEvent.AddListener(DisplayTrick);
                _trickControllerComponent.OnInputReceived.AddListener(InputReceived);

            }
        }

        this.gameObject.SetActive(false);
        ClearChildren();
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

    public void DisplayTrick(bool isInAir)
    {
        if(!isInAir) 
        {
            this.gameObject.SetActive(false);
            ClearChildren();
            return;
        }



        ClearChildren();

        this.gameObject.SetActive(true);
        int randomTrick = Random.Range(0, _trickSet.TrickCombos.Count);

        TrickSO chosenTrick = _trickSet.TrickCombos[randomTrick];
        _trickLength = chosenTrick.Combo.Count;
        _trickControllerComponent.TrickLength = _trickLength;
        for (int i = 0; i < chosenTrick.Combo.Count; i++)
        {
            if(_arrowSprites.TryGetValue(chosenTrick.Combo[i], out Sprite sprite))
            {
                //Debug.Log(chosenTrick.Combo[i] + " " + sprite);
                GameObject newTrickImage = Instantiate(_trickImage, transform);
                newTrickImage.name = chosenTrick.Combo[i].ToString();
                newTrickImage.GetComponent<Image>().sprite = sprite;
                _trickImages.Add(newTrickImage);
            }
        }

        ChangeState(EnlargeState());
    }

    public void InputReceived(TrickButtons button)
    {
        if (_trickImages[0].name == button.ToString())
        {
            Color transparent = new Color(0, 0, 0, 0);
            _trickImages[0].GetComponent<Image>().color = Color.red;
            GameObject removedImage = _trickImages[0];
            _trickImages.RemoveAt(0);
            _trickImages.Add(removedImage);
            _trickControllerComponent.PlayInputSFX(true);
        }
        else
        {
            _trickControllerComponent.PlayInputSFX(false);
            _trickControllerComponent.BreakCurrentTrick();
        }
    }

    private void ClearChildren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        _trickImages.Clear();
    }

    private void ChangeState(IEnumerator newState)
    {
        if (_currentState != null) StopCoroutine(_currentState);

        _currentState = newState;
        StartCoroutine(_currentState);
    }

    private IEnumerator EnlargeState()
    {
        _rectTransform.localScale = Vector3.zero;
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
    }
}
