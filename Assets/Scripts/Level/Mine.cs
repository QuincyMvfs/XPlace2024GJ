using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]

public class Mine : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float _amplitude = 0.5f;  // Height of the floating
    [SerializeField] private float _frequency = 1f;    // Speed of the floating
    [SerializeField] private float _minDelay = 0f;
    [SerializeField] private float _maxDelay = 5f;
    [SerializeField] private float _rotationSpeedX = 45f;  
    [SerializeField] private float _rotationSpeedY = 45f;  
    [SerializeField] private float _rotationSpeedZ = 45f;

    [Header("Slow Functionality")]
    [SerializeField] private float _slowDuration = 1.0f;

    [Header("GameObjects")]
    [SerializeField] private GameObject _innerMine;
    [SerializeField] private GameObject _mineVfxPrefab;

    private float _rotationAmountX;
    private float _rotationAmountY;
    private float _rotationAmountZ;
    private float _phaseShift;
    private Vector3 _startPos;
    
    void Start()
    {
        _startPos = transform.position;

        // Record the starting position of the GameObject
        _phaseShift = Random.Range(0f, 1f + Mathf.PI);
        StartCoroutine(StartingDelay(0.1f));
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerMesh>(out PlayerMesh playerMesh))
        {
            if (playerMesh.PlayerGameObject.TryGetComponent<ScoreController>(out ScoreController scoreController))
            {
                scoreController.BreakCombo();
             
                if (playerMesh.PlayerGameObject.TryGetComponent<PlayerMovement3D>(out PlayerMovement3D playerMovement))
                {
                    playerMovement.TemporarySlowDown(_slowDuration);
                }

                this.gameObject.GetComponent<MeshRenderer>().enabled = false;
                this.gameObject.GetComponent<SphereCollider>().enabled = false;
                _innerMine.GetComponent<MeshRenderer>().enabled = false;

                if (_mineVfxPrefab != null)
                {
                    Instantiate(_mineVfxPrefab, _mineVfxPrefab.transform);
                }

                StartCoroutine(DestroyAfterDelay(0.5f));
            }
        }
    }

    private IEnumerator StartingDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(FloatingLoop());
    }

    private IEnumerator FloatingLoop()
    {
        while (true)
        {
            // Calculate the new position
            Vector3 newPos = _startPos;
            newPos.y += Mathf.Sin(Time.time * _frequency + _phaseShift) * _amplitude;

            // Calculate the rotation for this frame
            _rotationAmountX = _rotationSpeedX * Time.deltaTime;
            _rotationAmountY = _rotationSpeedY * Time.deltaTime;
            _rotationAmountZ = _rotationSpeedZ * Time.deltaTime;

            // Apply the rotations
            transform.Rotate(Vector3.right, _rotationAmountX);
            transform.Rotate(Vector3.up, _rotationAmountY);
            transform.Rotate(Vector3.forward, _rotationAmountZ);

            // Apply the new position
            transform.position = newPos;

            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }
}