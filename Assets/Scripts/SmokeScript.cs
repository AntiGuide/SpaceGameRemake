using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeScript : MonoBehaviour {

	public float smokeLifetimeFull;

	private float smokeLifetime;

	// Use this for initialization
	void Start () {
		smokeLifetime = smokeLifetimeFull;
	}

	// Update is called once per frame
	void Update () {
		smokeLifetime -= Time.deltaTime;
		gameObject.GetComponent<Renderer> ().material.color = new Color(1.0f,1.0f,1.0f,smokeLifetime / smokeLifetimeFull);
		float scaleVector = ((smokeLifetime / smokeLifetimeFull) + 0.5f) / 2;
		gameObject.transform.localScale = new Vector3 (scaleVector,scaleVector,1.0f);
		if (smokeLifetime <= 0) {
			Destroy (gameObject);
		}
	}
}
