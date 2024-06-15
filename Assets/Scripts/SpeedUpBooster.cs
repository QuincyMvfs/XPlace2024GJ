using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]

public class SpeedUpBooster : MonoBehaviour
{
    private CapsuleCollider _collider;
    private bool _hastriggered = false;

    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (_hastriggered == false)
        {
            Debug.Log(collision.gameObject);
            Debug.Log("SPEED UP!");
            if (collision.gameObject.TryGetComponent<PlayerMovement3D>(out PlayerMovement3D speedController))
            {
                //player speed up go to here
            }
            _hastriggered = true;
            Destroy(this.gameObject);
        }
        
    }
}