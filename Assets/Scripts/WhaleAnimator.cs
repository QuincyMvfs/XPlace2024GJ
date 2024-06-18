using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleAnimator : MonoBehaviour
{
    [SerializeField] private PlayerMovement3D _playerMovement;
    private TrickController _trickController;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerMovement.OnAirborneEvent.AddListener(OnJump);
        _trickController = _playerMovement.GetComponent<TrickController>();
        _trickController.OnDisplayTrickSuccessText.AddListener(OnTrick);

    }

    private void Update()
    {
        _animator.SetBool("IsAirborne", _playerMovement.IsFalling);
    }

    private void OnJump()
    {
        _animator.SetTrigger("HitRamp");
    }

    private void OnTrick()
    {
        _animator.SetTrigger("Trick");
    }
}
