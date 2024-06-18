using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtterAnimator : MonoBehaviour
{
    [SerializeField] private PlayerMovement3D _playerMovement;
    private TrickController _trickController;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _trickController = _playerMovement.GetComponent<TrickController>();

        _playerMovement.OnMoveLeftEvent.AddListener(OnMoveLeft);
        _playerMovement.OnMoveRightEvent.AddListener(OnMoveRight);
        _playerMovement.OnMoveStraightEvent.AddListener(OnMoveStraight);
        _trickController.OnDisplayTrickSuccessText.AddListener(OnTrick);
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool("IsAirborne", _playerMovement.IsFalling);
    }

    private void OnMoveLeft()
    {
        _animator.SetBool("RidingLeft", true);
        _animator.SetBool("RidingRight", false);
    }

    private void OnMoveRight()
    {
        _animator.SetBool("RidingLeft", false);
        _animator.SetBool("RidingRight", true);
    }

    private void OnMoveStraight()
    {
        _animator.SetBool("RidingLeft", false);
        _animator.SetBool("RidingRight", false);
    }

    private void OnTrick()
    {
        _animator.SetTrigger("Trick");
    }
}
