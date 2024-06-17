using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrickUIManager : MonoBehaviour
{
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
    }

    public void InputReceived(TrickButtons button)
    {
        if (_trickImages[0].name == button.ToString())
        {
            Color transparent = new Color(0, 0, 0, 0);
            _trickImages[0].GetComponent<Image>().color = transparent;
            GameObject removedImage = _trickImages[0];
            _trickImages.RemoveAt(0);
            _trickImages.Add(removedImage);
        }
        else
        {
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
}
