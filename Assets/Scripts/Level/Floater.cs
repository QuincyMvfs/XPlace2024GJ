using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    [SerializeField] private float _amplitude = 0.5f;  // Height of the floating
    [SerializeField] private float _frequency = 1f;    // Speed of the floating

    private float _phaseShift;
    private Vector3 _startPos;

    void Start()
    {
        _startPos = transform.position;

        // Record the starting position of the GameObject
        _phaseShift = Random.Range(0f, 1f + Mathf.PI);
        StartCoroutine(StartingDelay(0.1f));
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
            Vector3 newPos = _startPos;
            newPos.y += Mathf.Sin(Time.time * _frequency + _phaseShift) * _amplitude;


            // Apply the new position
            transform.position = newPos;

            yield return new WaitForSeconds(0.01f);
        }
    }


}
