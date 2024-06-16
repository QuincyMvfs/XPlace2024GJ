using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]

public class Mine : MonoBehaviour
{
    private SphereCollider _collider;
    [SerializeField] private float amplitude = 0.5f;  // Height of the floating
    [SerializeField] private float frequency = 1f;    // Speed of the floating
    private Vector3 startPos;
    [SerializeField] private float _minDelay = 0f;
    [SerializeField] private float _maxDelay = 5f;
    [SerializeField] private float rotationSpeedX = 45f;  // Degrees per second around the x-axis
    [SerializeField] private float rotationSpeedY = 45f;  // Degrees per second around the y-axis
    [SerializeField] private float rotationSpeedZ = 45f;  // Degrees per second around the z-axis
    private float rotationAmountX;
    private float rotationAmountY;
    private float rotationAmountZ;
    private float phaseShift;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    void Start()
    {
        startPos = transform.position;

        // Record the starting position of the GameObject
        phaseShift = Random.Range(0f, 1f + Mathf.PI);
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
                    playerMovement.ResetMovementSpeed();
                }
                Destroy(this.gameObject);
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
            Vector3 newPos = startPos;
            newPos.y += Mathf.Sin(Time.time * frequency + phaseShift) * amplitude;

            // Calculate the rotation for this frame
            rotationAmountX = rotationSpeedX * Time.deltaTime;
            rotationAmountY = rotationSpeedY * Time.deltaTime;
            rotationAmountZ = rotationSpeedZ * Time.deltaTime;

            // Apply the rotations
            transform.Rotate(Vector3.right, rotationAmountX);
            transform.Rotate(Vector3.up, rotationAmountY);
            transform.Rotate(Vector3.forward, rotationAmountZ);

            // Apply the new position
            transform.position = newPos;

            yield return new WaitForSeconds(0.01f);
        }
    }
}