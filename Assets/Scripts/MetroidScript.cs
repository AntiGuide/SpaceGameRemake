using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetroidScript : MonoBehaviour {

	public MetroidSize size;
	public enum MetroidSize{SMALL,MEDIUM,BIG};
	public float turnSpeed;
	public float movementSpeed = 1.0f;
	public GameObject smallMetroidPrefab;
	public GameObject mediumMetroidPrefab;
	public float explosionSpeed = 1.0f;
	public int damage;
	public int healthFull;
	public GameObject explosionParticles;
	public AudioClip metroidExplode;
	public AudioClip metroidHit;
	public GameObject smokePrefab;
	public float smokeSpeed;

	private AudioSource metroidAudioSource;
	private int health;
	private Rigidbody2D rb;
	private float damageStayCooldownFull = 0.25f;
	private float damageStayCooldown;
	// Use this for initialization
	void Start () {
		metroidAudioSource = GameObject.Find ("Main Camera").GetComponent<AudioSource> ();
		damageStayCooldown = damageStayCooldownFull;
		rb = gameObject.GetComponent<Rigidbody2D> ();
		switch (size) {
		case MetroidSize.SMALL:
			healthFull = 1;
			rb.AddTorque (0.004f);
			break;
		case MetroidSize.MEDIUM:
			healthFull = 8;
			rb.AddTorque (0.15f);
			break;
		case MetroidSize.BIG:
			healthFull = 10;
			rb.AddTorque (1.9f);
			rb.AddForce (new Vector2((Random.value -0.5f) * 2 * movementSpeed,(Random.value -0.5f) * 2 * movementSpeed));
			break;
		default:
			break;
		}
		health = healthFull;
	}

	// Update is called once per frame
	void Update () {

	}

	void LateUpdate(){
		ScreenTeleport (transform);
	}

	public void ScreenTeleport(Transform transform){
		if (Mathf.Abs (transform.localPosition.x) > 9.5f) {
			transform.localPosition = new Vector2 (transform.localPosition.x * -1, transform.localPosition.y);
		}
		if (Mathf.Abs (transform.localPosition.y) > 5.5f) {
			transform.localPosition = new Vector2 (transform.localPosition.x, transform.localPosition.y * -1);
		}
	}

	public void giveDamage(int damage){
		health -= damage;

		if (health <= 0) {
			subLife ();
			health = healthFull;
		}else{
			metroidAudioSource.PlayOneShot (metroidHit);
		}
		float percentLife = (float)health / (float)healthFull;
		gameObject.GetComponent<SpriteRenderer> ().material.SetColor ("_Color",new Color(1.0f,percentLife,percentLife));
	}

	void explode(GameObject selectedSmokePrefab, int desired, float smokeSpeedLocal){
		Transform tmpGameObjectTransform;
		GameObject tmpGameObject;
		while (desired > 0) {
			tmpGameObject = Instantiate (selectedSmokePrefab);
			tmpGameObjectTransform = tmpGameObject.GetComponent<Transform> ();
			tmpGameObjectTransform.localPosition = new Vector2 ((Random.value - 0.5f) / 2 + gameObject.transform.localPosition.x,(Random.value - 0.5f) / 2 + gameObject.transform.localPosition.y);
			tmpGameObject.GetComponent<Rigidbody2D> ().AddForce ((tmpGameObjectTransform.localPosition - gameObject.transform.localPosition) * smokeSpeedLocal);
			desired--;
		}
	}

	public void subLife(){
		int desired = 0;
		GameObject selectedMetroidPrefab = null;

		switch (size) {
		case MetroidSize.SMALL:
			desired = 0;
			break;
		case MetroidSize.MEDIUM:
			desired = 4;
			selectedMetroidPrefab = smallMetroidPrefab;
			metroidAudioSource.PlayOneShot (metroidExplode, 1.0f);
			explode (smokePrefab,5,smokeSpeed);
			break;
		case MetroidSize.BIG:
			desired = 5;
			selectedMetroidPrefab = mediumMetroidPrefab;
			metroidAudioSource.PlayOneShot (metroidExplode,1.0f);
			explode (smokePrefab,5,smokeSpeed);
			break;
		default:
			Application.Quit ();
			break;
		}

		Transform tmpMetroidTransform;
		GameObject tmpMetroid;
		while (desired > 0) {
			tmpMetroid = Instantiate (selectedMetroidPrefab);
			tmpMetroidTransform = tmpMetroid.GetComponent<Transform> ();
			tmpMetroidTransform.localPosition = new Vector2 ((Random.value - 0.5f) / 2 + gameObject.transform.localPosition.x,(Random.value - 0.5f) / 2 + gameObject.transform.localPosition.y);
			tmpMetroid.GetComponent<Rigidbody2D> ().AddForce ((tmpMetroidTransform.localPosition - gameObject.transform.localPosition) * explosionSpeed);
			desired--;
		}
		GameObject particles = Instantiate (explosionParticles);
		particles.transform.localPosition = gameObject.transform.localPosition;
		Destroy (gameObject);
	}

	void OnCollisionEnter2D(Collision2D coll){
		if(coll.gameObject.tag == "Player"){
			coll.gameObject.GetComponent<PlayerScript> ().giveDamage (damage, true);
		}
	}

	void OnCollisionStay2D(Collision2D coll){
		damageStayCooldown -= Time.deltaTime;
		if(damageStayCooldown <= 0.0f){
			coll.gameObject.GetComponent<PlayerScript> ().giveDamage (damage, true);
			damageStayCooldown = damageStayCooldownFull;
		}
	}

	void OnCollisionExit2D(Collision2D coll){
		damageStayCooldown = damageStayCooldownFull;
	}

}
