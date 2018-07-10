using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	public float shootingSpeed;
	public int damage;
	public GameObject damageParticles;

	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D> ();
		rb.AddForce (transform.up * shootingSpeed);
	}
	
	// Update is called once per frame
	void Update () {
		if (Mathf.Abs (transform.position.x) > 9.3f || Mathf.Abs (transform.position.y) > 5.3f) {
			Destroy (gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D coll){
		if(coll.gameObject.tag == "Metroids"){
			Destroy (gameObject);
			coll.gameObject.GetComponent<MetroidScript> ().giveDamage (damage);
		}else if(coll.gameObject.tag == "Player"){
			Destroy (gameObject);
			coll.gameObject.GetComponent<PlayerScript> ().giveDamage (damage,false);
		}

		GameObject particles = Instantiate (damageParticles);
		particles.transform.localPosition = coll.contacts[0].point;
		//Particles
	}

}
