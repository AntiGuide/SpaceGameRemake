using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour {
	
	public float explosionLifetimeFull;

	private float explosionLifetime;

	// Use this for initialization
	void Start () {
		explosionLifetime = explosionLifetimeFull;
	}
	
	// Update is called once per frame
	void Update () {
		explosionLifetime -= Time.deltaTime;
		gameObject.GetComponent<Renderer> ().material.color = new Color(1.0f,1.0f,1.0f,explosionLifetime / explosionLifetimeFull);
		if (explosionLifetime <= 0) {
			Destroy (gameObject);
		}
	}
}
