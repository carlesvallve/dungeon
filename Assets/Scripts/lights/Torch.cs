using UnityEngine;
using System.Collections;

public class Torch : MonoBehaviour {

	private Vector3 center;
	private float d = 0.25f;

	void Start () {
		center = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		

		int prob = Random.Range(1, 100);
		if (prob <= 25) {
			transform.localPosition = center + new Vector3(Random.Range(-d, d), Random.Range(-d, d), Random.Range(-d, d));
		}
		
	}
}
