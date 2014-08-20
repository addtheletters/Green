using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextRainer : MonoBehaviour {

	public float period = 1f; //time between bolts
	public int max_bolts = 50;

	public GameObject MANAGERS; // needs to be passed via editor or initialization
	
	public float xSpread = 10;
	public float zSpread = 5;
	public float ySpread = 0;

	float time_since_spawn = 0;

	List<GameObject> bolts = new List<GameObject>();
	int bolt_counter  = 0;
	
	void AddBolt(){
		Vector3 spawnPos = transform.position;
		spawnPos.x += xSpread * Random.value - xSpread/2;
		spawnPos.y += ySpread * Random.value - ySpread/2;
		spawnPos.z += zSpread * Random.value - zSpread/2;
		AddBolt (spawnPos);
	}

	void AddBolt(Vector3 startPos){
		GameObject bolt = new GameObject ("Text Bolt");
		bolt.transform.position = startPos;
		bolts.Add (bolt);

		TextBolt tb = (TextBolt)(bolt.AddComponent<TextBolt>());
		tb.MANAGERS = MANAGERS;

		bolt_counter ++;
	}


	// Use this for initialization
	void Start () {
		if (MANAGERS == null) {
			Debug.LogWarning("[TextRainer] MANAGERS not given, things will break!");
		}
		AddBolt (transform.position);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		time_since_spawn += Time.fixedDeltaTime;

		print ("TSS" + time_since_spawn);

		int spawns_needed 	= 0;
		int bolts_this_tick = 0;

		if (time_since_spawn > period) {
			spawns_needed = (int)(time_since_spawn/period);
			print (time_since_spawn + "/" + period + ":" + spawns_needed);
			time_since_spawn = 0;
		}

		while (bolts_this_tick < spawns_needed) {
			if(bolts.Count >= max_bolts){
				break;	
			}
			AddBolt();
			bolts_this_tick ++;
		}


		ArrayList toRemove = new ArrayList ();

		foreach(GameObject bolt in bolts){
			if(outsideCameraView(bolt)){
				bolt.GetComponent<TextBolt>().SmoothTerminate();
				toRemove.Add (bolt);
			}
		}

		foreach (GameObject bolt in toRemove) {
			bolts.Remove(bolt);
		}
	}

	bool outsideCameraView(GameObject obj){
		Vector2 viewportPoint = Camera.main.WorldToViewportPoint (obj.transform.position);
		if (viewportPoint.x < 0 ||
						viewportPoint.x > 1 ||
						viewportPoint.y < 0 ||
						viewportPoint.y > 1) {
			return true;
		}
		return false;
	}

	/*
	 * bool insideCameraView(){
	 * // below method requires that obj have a renderer
		//print ("obj is " + obj.renderer.isVisible + "ly visible at " + obj.transform.position);
		//return obj.renderer.isVisible;
	 * 
	 * }
	 * 
	 * 
	 */

}
