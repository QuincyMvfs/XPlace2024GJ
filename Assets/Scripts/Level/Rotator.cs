using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float _rotationSpeedX = 45f;
    [SerializeField] private float _rotationSpeedY = 45f;
    [SerializeField] private float _rotationSpeedZ = 45f;

    private float _rotationAmountX;
    private float _rotationAmountY;
    private float _rotationAmountZ;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpinLoop());
    }

    private IEnumerator SpinLoop()
    {
        while (true)
        {
            // Calculate the rotation for this frame
            _rotationAmountX = _rotationSpeedX * Time.deltaTime;
            _rotationAmountY = _rotationSpeedY * Time.deltaTime;
            _rotationAmountZ = _rotationSpeedZ * Time.deltaTime;

            // Apply the rotations
            transform.Rotate(Vector3.right, _rotationAmountX);
            transform.Rotate(Vector3.up, _rotationAmountY);
            transform.Rotate(Vector3.forward, _rotationAmountZ);

            yield return new WaitForSeconds(0.01f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
