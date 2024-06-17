using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]

public class SpeedUpBooster : MonoBehaviour
{
    [SerializeField] private float _speedAddition = 10.0f;
    [SerializeField] private float _lastingTime = 10.0f;
    [SerializeField] private GameObject _speedBoostVfxPrefab;
    [SerializeField] private MeshRenderer _mesh;

    private PlayerMovement3D _playerMovementComponent;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerMesh>(out PlayerMesh playerMesh))
        {
            if (playerMesh.PlayerGameObject.TryGetComponent<PlayerMovement3D>(out PlayerMovement3D playerMovementComponent))
            {
                _playerMovementComponent = playerMovementComponent;
                _playerMovementComponent.AddMovementSpeed(_speedAddition, _lastingTime);

                _mesh.enabled = false;
                this.gameObject.GetComponent<CapsuleCollider>().enabled = false;

                if (_speedBoostVfxPrefab != null)
                {
                    GameObject instantiatedVfx = Instantiate(_speedBoostVfxPrefab, transform.position, Quaternion.identity);
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