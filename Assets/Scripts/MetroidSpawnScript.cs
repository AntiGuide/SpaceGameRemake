using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetroidSpawnScript : MonoBehaviour {

	public GameObject metroidSmall;
	public GameObject metroidMedium;
	public GameObject metroidBig;
	public int desiredSmall;
	public int desiredMedium;
	public int desiredBig;

	private float spawnWidth = 9.3f;
	private float spawnHeigth = 5.3f;
	private float spawnSpace = 1.0f;


	// Use this for initialization
	void Start () {
		SpawnMetroidsInRange(desiredSmall, metroidSmall, spawnWidth, spawnHeigth, spawnSpace);
		SpawnMetroidsInRange(desiredMedium, metroidMedium, spawnWidth, spawnHeigth, spawnSpace);
		SpawnMetroidsInRange(desiredBig, metroidBig, spawnWidth, spawnHeigth, spawnSpace);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SpawnMetroidsInRange(int desired, GameObject metroidPrefab, float borderX, float borderY, float radiusOfMetroid){
		Transform tmpMetroidTransform;
		while (desired > 0) {
			Vector2 wantedPosition = new Vector2 ((Random.value - 0.5f) * borderX * 2, (Random.value - 0.5f) * borderY * 2);
			int attempts = 0;
			do {
				// Get a Random spawn Position
				if(Physics2D.OverlapCircle(wantedPosition, radiusOfMetroid) == null) { // Check the bounds of the spawn position
					tmpMetroidTransform = Instantiate (metroidPrefab).GetComponent<Transform> ();
					tmpMetroidTransform.localPosition = wantedPosition;
					desired--;
					break;
				}else{
					wantedPosition = new Vector2 ((Random.value - 0.5f) * borderX * 2, (Random.value - 0.5f) * borderY * 2);
				}
				attempts++;
			} while (attempts <= 10); // Limit spawn attempts to prevent infinite loop
			if(attempts > 10) {
				desired--;
			}
		}
	}
}
