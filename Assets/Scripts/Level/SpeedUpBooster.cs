using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]

public class SpeedUpBooster : MonoBehaviour
{
    [Header("Speed Up Variables")]
    [SerializeField] private float _speedAddition = 10.0f;
    [SerializeField] private float _lastingTime = 10.0f;

    [Header("GameObjects")]
    [SerializeField] private GameObject _speedBoostVfxPrefab;
    [SerializeField] private GameObject _speedUpSFX;

    private PlayerMovement3D _playerMovementComponent;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerMesh>(out PlayerMesh playerMesh))
        {
            if (playerMesh.PlayerGameObject.TryGetComponent<PlayerMovement3D>(out PlayerMovement3D playerMovementComponent))
            {
                _playerMovementComponent = playerMovementComponent;
                _playerMovementComponent.AddMovementSpeed(_speedAddition, _lastingTime);

                this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
                this.gameObject.GetComponent<MeshRenderer>().enabled = false;

                if (_speedBoostVfxPrefab != null)
                {
                    GameObject instantiatedVfx = Instantiate(_speedBoostVfxPrefab, transform.position, Quaternion.identity);
                    Destroy(instantiatedVfx, 5.0f);
                }

                if (_speedUpSFX != null)
                {
                    GameObject instantiatedSfx = Instantiate(_speedUpSFX, transform.position, Quaternion.identity);
                    Destroy(instantiatedSfx, 5.0f);
                }

                StartCoroutine(DestroyAfterDelay(_lastingTime));
            }
        }
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }
}