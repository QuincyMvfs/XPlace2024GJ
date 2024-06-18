using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplierCoin : MonoBehaviour
{
    [SerializeField] private int _addToMultiplier = 1;
    [SerializeField] private GameObject _vfxPrefab;
    [SerializeField] private GameObject _pickupSFX;


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerMesh>(out PlayerMesh playerMesh))
        {
            if (playerMesh.PlayerGameObject.TryGetComponent<ScoreController>(out ScoreController scoreController))
            {
                scoreController.AddToStreak(_addToMultiplier - 1);

                this.gameObject.GetComponent<SphereCollider>().enabled = false;
                this.gameObject.GetComponent<MeshRenderer>().enabled = false;

                if (_vfxPrefab != null)
                {
                    GameObject instantiatedVfx = Instantiate(_vfxPrefab, transform.position, Quaternion.identity);
                    Destroy(instantiatedVfx, 1.0f);
                }

                if (_pickupSFX != null)
                {
                    GameObject spawnedSFX = Instantiate(_pickupSFX, transform.position, Quaternion.identity);
                    Destroy(spawnedSFX, 5.0f);
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
