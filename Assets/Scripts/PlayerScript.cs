using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	public float torque;
	public float speed;
	public PlayerType playerType;
	public enum PlayerType{LEFT,RIGHT};
	public Vector2 spawnPointLeft;
	public GameObject bullet;
	public float shootingRate;
	public int healthFull;
	public int lifes = 3;
	public int damage;
	public float explosionSpeed;
	public GameObject explosionPrefab;
	public GameObject fragmentPrefab;
	public float fragmentSpeed;
	public AudioClip shoot;
	public AudioClip explodeAudio;

	private AudioSource playerAudioSource;
	private int health;
	private float aktShootingRate = 0;
	private Vector2 spawnPoint;
	private Rigidbody2D rb;
	private string rotationAxis;
	private string speedAxis;
	private string fireAxis;
	private Transform shipExhaust;
	private GameObject[] allPlayers;

	// Use this for initialization
	void Start () {
		playerAudioSource = GetComponent<AudioSource> ();
		rb = gameObject.GetComponent<Rigidbody2D>();
		health = healthFull;
		allPlayers = GameObject.FindGameObjectsWithTag ("Player");
		shipExhaust = transform.Find ("ShipExhaust");
		shipExhaust.localScale = Vector3.zero;
		if (playerType == PlayerType.LEFT) {
			rotationAxis = "HorizontalLeft";
			speedAxis = "VerticalLeft";
			fireAxis = "FireLeft";
			spawnPoint = spawnPointLeft;
		}else if (playerType == PlayerType.RIGHT) {
			rotationAxis = "HorizontalRight";
			speedAxis = "VerticalRight";
			fireAxis = "FireRight";
			spawnPoint = spawnPointLeft * -1;
		}
		Respawn ();
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetAxis (rotationAxis) != 0){
			float turn = Input.GetAxis(rotationAxis);
			rb.AddTorque (torque * turn * -1);
		}
		if (Input.GetAxis (speedAxis) > 0) {
			rb.AddForce (transform.up * speed * Input.GetAxis (speedAxis));
		}
		if (Input.GetAxisRaw (speedAxis) > 0) {
			shipExhaust.localScale = Vector3.one;
		} else if (shipExhaust.localScale != Vector3.zero) {
			shipExhaust.localScale = Vector3.zero;
		}
		if (Input.GetAxisRaw (fireAxis) != 0 && aktShootingRate <= 0) {
			Shoot ();
			aktShootingRate = shootingRate;
		}else{
			aktShootingRate -= Time.deltaTime;
		}
	}

	void LateUpdate(){
		ScreenTeleport (transform);
	}

	public void Respawn(){
		transform.localPosition = spawnPoint;
		transform.localRotation = Quaternion.Euler (0, 0, 0);
		gameObject.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		gameObject.GetComponent<SpriteRenderer> ().material.SetColor ("_Color",new Color(1.0f,1.0f,1.0f));
	}

	public void ScreenTeleport(Transform transform){
		if (Mathf.Abs (transform.localPosition.x) > 9.3f) {
			transform.localPosition = new Vector2 (transform.localPosition.x * -1, transform.localPosition.y);
		}
		if (Mathf.Abs (transform.localPosition.y) > 5.3f) {
			transform.localPosition = new Vector2 (transform.localPosition.x, transform.localPosition.y * -1);
		}
	}

	void Shoot(){
		Transform tmpBullet;
		GameObject go;
		go = Instantiate (bullet);
		Physics2D.IgnoreCollision (go.GetComponent<Collider2D> (), gameObject.GetComponent<Collider2D>());
		tmpBullet = go.GetComponent<Transform> ();
		tmpBullet.parent = transform;
		tmpBullet.localScale = new Vector3 (0.5f,0.5f,1f);
		tmpBullet.eulerAngles = transform.eulerAngles;
		tmpBullet.localPosition = new Vector2 (0.186f, -0.1f);

		go = Instantiate (bullet);
		Physics2D.IgnoreCollision (go.GetComponent<Collider2D> (), gameObject.GetComponent<Collider2D>());
		tmpBullet = go.GetComponent<Transform> ();
		tmpBullet.parent = transform;
		tmpBullet.localScale = new Vector3 (0.5f,0.5f,1f);
		tmpBullet.eulerAngles = transform.eulerAngles;
		tmpBullet.localPosition = new Vector2 (-0.154f, -0.1f);

		playerAudioSource.PlayOneShot (shoot,1.0f);
	}

	public void giveDamage(int damage, bool metroid){
		health -= damage;
		if (health <= 0) {
			subLife (metroid);
		}
		float percentLife = (float)health / (float)healthFull;
		gameObject.GetComponent<SpriteRenderer> ().material.SetColor ("_Color",new Color(1.0f,percentLife,percentLife));
	}

	public void subLife(bool metroid){
		lifes -= 1;
		playerAudioSource.PlayOneShot (explodeAudio);
		explode (explosionPrefab,5, explosionSpeed);
		if (metroid) {
			explode (fragmentPrefab, 2, fragmentSpeed);
		}
		health = healthFull;
		if (lifes > 0) {
			string iconPath = "Canvas/Lifes";

			if (playerType == PlayerType.LEFT) {
				iconPath += "Left";
			} else if (playerType == PlayerType.RIGHT) {
				iconPath += "Right";
			}
			iconPath += "/Life" + (lifes + 1);
			GameObject.Find (iconPath).transform.localScale = Vector3.zero;
			for (int i = 0; i < allPlayers.Length; i++) {
				allPlayers [i].GetComponent<PlayerScript> ().Respawn ();
			}
		}else{
			SceneManager.LoadScene ("Hauptmenue");
		}
	}

	void OnCollisionEnter2D(Collision2D coll){
		if(coll.gameObject.tag == "Metroids"){
			coll.gameObject.GetComponent<MetroidScript> ().giveDamage (damage);
		}
	}

	void explode(GameObject selectedExplosionPrefab, int desired, float explosionSpeedLocal){
		Transform tmpGameObjectTransform;
		GameObject tmpGameObject;
		while (desired > 0) {
			tmpGameObject = Instantiate (selectedExplosionPrefab);
			tmpGameObjectTransform = tmpGameObject.GetComponent<Transform> ();
			tmpGameObjectTransform.localPosition = new Vector2 ((Random.value - 0.5f) / 2 + gameObject.transform.localPosition.x,(Random.value - 0.5f) / 2 + gameObject.transform.localPosition.y);
			tmpGameObject.GetComponent<Rigidbody2D> ().AddForce ((tmpGameObjectTransform.localPosition - gameObject.transform.localPosition) * explosionSpeedLocal);
			desired--;
		}
	}
}
