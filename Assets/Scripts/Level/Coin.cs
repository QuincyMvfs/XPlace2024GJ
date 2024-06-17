using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]

public class Coin : MonoBehaviour
{
    [SerializeField] private float _rotationSpeedX = 45f; 
    [SerializeField] private float _rotationSpeedY = 45f; 
    [SerializeField] private float _rotationSpeedZ = 45f; 
    [SerializeField] private int _scoreAmount = 50;
    [SerializeField] private GameObject _coinVfxPrefab;

    private float _rotationAmountX;
    private float _rotationAmountY;
    private float _rotationAmountZ;

    void Start()
    {
        // Record the starting position of the GameObject
        StartCoroutine(SpinLoop());
    }

    private IEnumerator SpinLoop()
    {
        while (true) 
        {
            // Calculate the rotation for this frame
            _rotationAmountX = _rotationSpeedX * Time.deltaTime;
            _rotationAmountY = _rotationSpeedY * Time.deltaTime;
            _rotationAmountZ = _rotationSpeedZ * Time.deltaTime;

            // Apply the rotations
            transform.Rotate(Vector3.right, _rotationAmountX);
            transform.Rotate(Vector3.up, _rotationAmountY);
            transform.Rotate(Vector3.forward, _rotationAmountZ);

            yield return new WaitForSeconds(0.01f);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerMesh>(out PlayerMesh playerMesh))
        {
            if(playerMesh.PlayerGameObject.TryGetComponent<ScoreController>(out ScoreController scoreController))
            {
                scoreController.AddCoinScore(_scoreAmount);

                this.gameObject.GetComponent<SphereCollider>().enabled = false;
                this.gameObject.GetComponent<MeshRenderer>().enabled = false;

                if(_coinVfxPrefab != null)
                {
                    GameObject instantiatedVfx = Instantiate(_coinVfxPrefab, transform.position, Quaternion.identity);
                    Destroy(instantiatedVfx, 1.0f);
                }

                StartCoroutine(DestroyAfterDelay(0.5f));
            }
        }
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }
}