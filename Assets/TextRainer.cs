using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextRainer : MonoBehaviour {

	public float frequency = 1; //bolts per tick
	public int max_bolts = 50;

	public GameObject MANAGERS; // needs to be passed via editor or initialization
	
	public float xSpread = 5;
	public float zSpread = 5;
	public float ySpread = 0;

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
		AddBolt (transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		while (bolts.Count < max_bolts) {
			AddBolt();
		}
		foreach(GameObject bolt in bolts){
			if(outsideCameraView(bolt)){
				bolt.GetComponent<TextBolt>().SmoothTerminate();
			}
		}
	}

	bool outsideCameraView(GameObject obj){
		return false;
	}
}
