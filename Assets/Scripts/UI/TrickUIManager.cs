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
    [SerializeField] private PlayerMovement3D _playerMovementComponent;
    
    private TrickController _trickControllerComponent;

    private Dictionary<TrickButtons, Sprite> _arrowSprites = new Dictionary<TrickButtons, Sprite>();

    private void Start()
    {
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
                Debug.Log(chosenTrick.Combo[i] + " " + sprite);
                GameObject newTrickImage = Instantiate(_trickImage, transform);
                newTrickImage.GetComponent<Image>().sprite = sprite;
            }
        }
    }

    private void ClearChildren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
