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
    [SerializeField] private float _minDelay = 0f;
    [SerializeField] private float _maxDelay = 5f;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    void Start()
    {
        startPos = transform.position;
        float randomDelay = Random.Range(_minDelay, _maxDelay);
        // Record the starting position of the GameObject
        StartCoroutine(StartingDelay(randomDelay));
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerMesh>(out PlayerMesh playerMesh))
        {
            if (playerMesh.PlayerGameObject.TryGetComponent<ScoreController>(out ScoreController scoreController))
            {
                scoreController.BreakCombo();
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
            newPos.y += Mathf.Sin(Time.time * frequency) * amplitude;

            // Apply the new position
            transform.position = newPos;

            yield return new WaitForSeconds(0.01f);
        }
    }
}