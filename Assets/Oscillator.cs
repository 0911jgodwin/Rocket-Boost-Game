using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour {

    [SerializeField] float period = 2f;
    [SerializeField] Vector3 movementVector;
    [Range(0, 1)] float movementFactor; // 0 for no movement, 1 for fully moved.
    

    Vector3 startingPosition; //Stored for absolute movement.

	// Use this for initialization
	void Start () {
        startingPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        if (period <= Mathf.Epsilon) { return; } // Prevents dividing by 0
        float cycle = Time.time / period; // Continually increases from 0
        const float twoPi = Mathf.PI * 2; // 2 pi for use in radians
        float sinWave = Mathf.Sin(cycle * twoPi); // -1 to +1

        movementFactor = (sinWave / 2f) + 0.5f; // Ensures the movement factor range is between 0 and 1.
        Vector3 displacement = movementFactor * movementVector;
        transform.position = startingPosition + displacement;
	}
}
