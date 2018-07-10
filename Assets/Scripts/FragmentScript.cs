using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentScript : MonoBehaviour {

	public GameObject damageParticles;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D coll){
		if(coll.gameObject.tag == "Metroids"){
			GameObject particles = Instantiate (damageParticles);
			particles.transform.localPosition = coll.contacts[0].point;
		}
		Destroy (gameObject);
	}
}
