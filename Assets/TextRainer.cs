using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextRainer : MonoBehaviour {

	public float period = 0.5f; //time between bolts
	public int max_bolts = 50;

	public GameObject MANAGERS; // needs to be passed via editor or initialization

	// can either be passed or will be loaded
	public Material TEXT_MATERIAL;
	public Font 	TEXT_FONT;
	
	public float xSpread = 110;
	public float zSpread = 40;
	public float ySpread = 0;

	float time_since_spawn = 0;

	List<GameObject> bolts = new List<GameObject>();
	int bolt_counter  = 0;

	//public TextAsset TEXT_SOURCE;
	string CHAR_CHOICES; //gets loaded from asset

	// Use this for initialization
	void Start () {
		if (MANAGERS == null) {
			Debug.LogWarning("[TextRaindeer] MANAGERS not given, things will break if font and material are missing!");
		}

		if (TEXT_FONT == null) {
			Debug.LogWarning("[TextRainer] Font not given, attempting load from manager.");
			TEXT_FONT = TextUtil.GetFont (MANAGERS, TextUtil.DEFAULT_FONT_NAME); //default font
		}
		
		if (TEXT_MATERIAL == null) {
			Debug.LogWarning("[TextRainer] Material not given, attempting load from manager.");
			TEXT_MATERIAL = TextUtil.GetMaterial (MANAGERS, TextUtil.DEFAULT_MATERIAL_NAME); //default material
		}

		/*if (TEXT_SOURCE == null) {
			Debug.LogWarning("[TextRainer] TEXT_SOURCE not given, bolts will default.");
		}*/
		CHAR_CHOICES = TextUtil.UTF_CHARACTER_POOL;

		AddBolt (transform.position);
	}

	void LoadFont(string fontname){
		TEXT_FONT = TextUtil.GetFont (MANAGERS, fontname);
	}
	void LoadMaterial(string materialname){
		TEXT_MATERIAL = TextUtil.GetMaterial (MANAGERS, materialname);
	}

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
		tb.TypicalInit (TEXT_FONT, TEXT_MATERIAL, CHAR_CHOICES);
		
		bolt_counter ++;
	}

	
	// Update is called once per frame
	void FixedUpdate () {

		time_since_spawn += Time.fixedDeltaTime;

		//print ("TSS" + time_since_spawn);

		int spawns_needed 	= 0;
		int bolts_this_tick = 0;

		if (time_since_spawn > period) {
			spawns_needed = (int)(time_since_spawn/period);
			//print (time_since_spawn + "/" + period + ":" + spawns_needed);
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
			if(!willEnterView(bolt)){
				bolt.GetComponent<TextBolt>().SmoothTerminate();
				toRemove.Add (bolt);
			}
		}

		foreach (GameObject bolt in toRemove) {
			bolts.Remove(bolt);
		}
	}

	static bool outsideCameraView(GameObject obj){
		Vector2 viewportPoint = Camera.main.WorldToViewportPoint (obj.transform.position);
		if (viewportPoint.x < 0 ||
						viewportPoint.x > 1 ||
						viewportPoint.y < 0 ||
						viewportPoint.y > 1) {
			return true;
		}
		return false;
	}

	static bool willEnterView(GameObject obj_going_down ){
		//or is in view
		Vector2 viewportPoint = Camera.main.WorldToViewportPoint (obj_going_down.transform.position);
		if (viewportPoint.x < 0 ||
		    viewportPoint.x > 1 ||
		    viewportPoint.y < 0) {
			return false;
		}
		return true;
	}

}
