using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]

public class Mine : MonoBehaviour
{
    private SphereCollider _collider;
    private bool _hastriggered = false;
    [SerializeField] private float amplitude = 0.5f;  // Height of the floating
    [SerializeField] private float frequency = 1f;    // Speed of the floating
    private Vector3 startPos;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    void Start()
    {
        // Record the starting position of the GameObject
        startPos = transform.position;
    }

    private void Update()
    {
        // Calculate the new position
        Vector3 newPos = startPos;
        newPos.y += Mathf.Sin(Time.time * frequency) * amplitude;

        // Apply the new position
        transform.position = newPos;
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