using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]

public class Mine : MonoBehaviour
{
    private SphereCollider _collider;
    private bool _hastriggered = false;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (_hastriggered == false)
        {
            Debug.Log(collision.gameObject);
            Debug.Log("boom");
            if (collision.gameObject.TryGetComponent<ScoreController>(out ScoreController scoreController))
            {
                scoreController.BreakCombo();
                Debug.Log("breakCombo");
            }
            _hastriggered = true;
            Destroy(this.gameObject);
        }
    }
}