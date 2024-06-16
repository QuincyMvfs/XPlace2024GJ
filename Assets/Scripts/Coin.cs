using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]

public class Coin : MonoBehaviour
{
    private SphereCollider _collider;
    private bool _hastriggered = false;
    [SerializeField] private float rotationSpeedX = 45f;  // Degrees per second around the x-axis
    [SerializeField] private float rotationSpeedY = 45f;  // Degrees per second around the y-axis
    [SerializeField] private float rotationSpeedZ = 45f;  // Degrees per second around the z-axis
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
        // Calculate the rotation for this frame
        float rotationAmountX = rotationSpeedX * Time.deltaTime;
        float rotationAmountY = rotationSpeedY * Time.deltaTime;
        float rotationAmountZ = rotationSpeedZ * Time.deltaTime;

        // Apply the rotations
        transform.Rotate(Vector3.right, rotationAmountX);
        transform.Rotate(Vector3.up, rotationAmountY);
        transform.Rotate(Vector3.forward, rotationAmountZ);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (_hastriggered == false)
        {
            Debug.Log(collision.gameObject);
            Debug.Log("GOLD");
            if (collision.gameObject.TryGetComponent<ScoreController>(out ScoreController scoreController))
            {
                
            }
            _hastriggered = true;
            Destroy(this.gameObject);
        }
    }
}