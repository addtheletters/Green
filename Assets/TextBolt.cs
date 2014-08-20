using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextBolt : MonoBehaviour {
	
	class TextCell {

		public GameObject text_mesh;
		public MeshRenderer mr;

		public TextCell( Vector3 pos, int size, Color color,
		                string text, Font font, Material mat){
			text_mesh = new GameObject("Text Cell");
			text_mesh.transform.position = pos;
			TextMesh mesh_component = (TextMesh)(text_mesh.AddComponent<TextMesh>());

			mesh_component.fontSize 	 = size;
			mesh_component.characterSize = size;
			mesh_component.color 		 = color;
			mesh_component.text 		 = text;

			mesh_component.font 		 = font;

			mr = text_mesh.transform.GetComponent<MeshRenderer>();
			mr.material = mat;

			//text_mesh.transform.localScale = text_mesh.transform.localScale * scale;
			Vector3 rot = text_mesh.transform.eulerAngles;
			rot.z -= 90;
			text_mesh.transform.eulerAngles = rot;

		}

		public void removeMeshObject(){
			Destroy (text_mesh);
		}

		public void reduceVisibility(double alpha_fade_amount){
			Color newcolor = mr.material.color;
			newcolor.a = (float)(newcolor.a - alpha_fade_amount);
			mr.material.color = newcolor;
		}

		public bool checkVisibility(){
			if (mr.material.color.a < 0) {
				removeMeshObject();
				return false;
			}
			return true;
		}
	}

	public int SIZE = 10;
	//public int FONT_RES = 100;
	//public float SCALE = 0.1f;	
	public Color BASE_COLOR = new Color(71f/255f, 255f/255f, 103f/255f);
	
	public GameObject MANAGERS; // object containing Font and Material manager components
	// if no font / material given, must be passed

	public float MOVE_SPEED = 20f;
	public Vector3 MOVE_DIR = Vector3.down; //should have magnitude 1
	
	public double ALPHA_FADE = 0.005; // how much alpha decreases every frame
	
	public float CHAR_SPACING; // this ought to be dynamic at some point.
	Vector3 original_pos;

	public bool smooth_terminate = false; // set to true and the bolt will end / kill itself once the existing cells fade

	string CHAR_CHOICES; //should be passed

	Material	TEXT_MATERIAL;
	Font 		TEXT_FONT;

	List<TextCell> cells = new List<TextCell>();
	int cellcounter = 0;
	//SOON //float total_boltlen = 0;

	//use to pass font and material in a single line
	public void TypicalInit(Font font, Material mat, string charChoices){

		TEXT_FONT 		= font;
		TEXT_MATERIAL 	= mat;

		CHAR_SPACING = (3f / 5f) * SIZE;

		CHAR_CHOICES = charChoices;
	}

	void Start () {
		/*if (MANAGERS == null) {
			Debug.LogWarning("[TextBolt] MANAGERS not given, things will break if no font or material is given.");
		}*/

		if (TEXT_FONT == null) {
			Debug.LogWarning("[TextBolt] Font not given, attempting load from manager.");
			TEXT_FONT = TextUtil.GetFont (MANAGERS, TextUtil.DEFAULT_FONT_NAME); //default font
		}

		if (TEXT_MATERIAL == null) {
			Debug.LogWarning("[TextBolt] Material not given, attempting load from manager.");
			TEXT_MATERIAL = TextUtil.GetMaterial (MANAGERS, TextUtil.DEFAULT_MATERIAL_NAME); //default material
		}

		if ( CHAR_CHOICES == null ){
			Debug.LogWarning("[TextBolt] No character choices given, defaulting.");
			CHAR_CHOICES = TextUtil.DEFAULT_CHARACTER_POOL;
		}

		original_pos = transform.position;

	}


	void AddCell(){
		cells.Add( new TextCell (transform.position, SIZE, BASE_COLOR, 
		                         TextUtil.GetRandomChar(CHAR_CHOICES), TEXT_FONT, TEXT_MATERIAL) );
		cellcounter ++;
	}


	void FixedUpdate () {
		if (!smooth_terminate) {
			AddNeededCells();
		}
		Recolor ();
		Move ();
		if (smooth_terminate) {
			if(cells.Count == 0){
				Destroy(gameObject);
			}
		}
	}

	void AddNeededCells(){
		Vector3 deltaPos = transform.position - original_pos;
		if (deltaPos.magnitude > GetBoltLength()) {
			AddCell ();
			//SOON //boltlen += something related to something returned from AddCell related to size of char
		}
	}

	float GetBoltLength(){
		return (cellcounter) * CHAR_SPACING;
	}

	void Recolor(){
		ArrayList toRemove = new ArrayList ();
		foreach( TextCell cell in cells ){
			cell.reduceVisibility(ALPHA_FADE);
			if( !cell.checkVisibility() ){
				toRemove.Add(cell);
			}
		}
		foreach (TextCell cell in toRemove) {
			cells.Remove(cell);
		}
	}

	void Move(){
		transform.Translate(MOVE_DIR * MOVE_SPEED * Time.fixedDeltaTime);
	}

	public void SmoothTerminate(){
		smooth_terminate = true;
	}

}
