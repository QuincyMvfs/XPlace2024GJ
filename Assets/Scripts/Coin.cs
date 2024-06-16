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
    [SerializeField] private int scoreAmount = 50;
    private Vector3 startPos;

    private float rotationAmountX;
    private float rotationAmountY;
    private float rotationAmountZ;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    void Start()
    {
        // Record the starting position of the GameObject
        startPos = transform.position;
        StartCoroutine(SpinLoop());
    }

    private IEnumerator SpinLoop()
    {
        while (true) 
        {
            // Calculate the rotation for this frame
            rotationAmountX = rotationSpeedX * Time.deltaTime;
            rotationAmountY = rotationSpeedY * Time.deltaTime;
            rotationAmountZ = rotationSpeedZ * Time.deltaTime;

            // Apply the rotations
            transform.Rotate(Vector3.right, rotationAmountX);
            transform.Rotate(Vector3.up, rotationAmountY);
            transform.Rotate(Vector3.forward, rotationAmountZ);

            yield return new WaitForSeconds(0.01f);
        }
    }


    private void OnTriggerEnter(Collider collision)
    {
            if (collision.gameObject.TryGetComponent<PlayerMesh>(out PlayerMesh playerMesh))
            {
                if(playerMesh.PlayerGameObject.TryGetComponent<ScoreController>(out ScoreController scoreController))
                {
                scoreController.AddCoinScore(scoreAmount);
                Destroy(this.gameObject);
                }
            }
    }
}