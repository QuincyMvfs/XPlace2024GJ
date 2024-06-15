using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class RampScript : MonoBehaviour
{
    private BoxCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerMesh>(out PlayerMesh playerMesh))
        {
            if (playerMesh.PlayerGameObject.TryGetComponent<PlayerMovement3D>(out PlayerMovement3D playerMovement))
            {
                playerMovement.Jump(JumpPowerType.Large);
            }
        }
    }
}
