using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextBolt : MonoBehaviour {
	
	class TextCell {

		public GameObject text_mesh;
		public MeshRenderer mr;

		public TextCell( Vector3 pos, int size, int font_res, float scale, Color color, GameObject managers, 
		                string fontname = "Pixelate", string materialname = "Font Material"){
			text_mesh = new GameObject("Text Cell");
			text_mesh.transform.position = pos;
			TextMesh mesh_component = (TextMesh)(text_mesh.AddComponent<TextMesh>());

			mesh_component.fontSize 	 = font_res;
			mesh_component.characterSize = size;
			mesh_component.color 		 = color;
			mesh_component.text 		 = getText();

			FontManager fm = (FontManager)(managers.GetComponent<FontManager>());
			mesh_component.font 		 = (fm).get.GetFont(fontname);


			mr = text_mesh.transform.GetComponent<MeshRenderer>();
			MaterialManager mm = (MaterialManager)(managers.GetComponent<MaterialManager>());
			mr.material = (mm).get.GetMaterial(materialname);

			/*if (mr.material == null){
				print("NONONONOull");
			}*/

			text_mesh.transform.localScale = text_mesh.transform.localScale * scale;
			Vector3 rot = text_mesh.transform.eulerAngles;
			rot.z -= 90; //undynamic. Perhaps change to be dependent on move direction?
			text_mesh.transform.eulerAngles = rot;
		}

		public string getText(string charChoices = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM?!@#$%^&*()+-={}[]|:;<>"){
			//a single random character
			return ""+(charChoices[ Random.Range(0, charChoices.Length) ]);
			//return "UNIMPLEMENTED";
		}

		public void removeMeshObject(){
			Destroy (text_mesh);
		}

		public void reduceVisibility(double alpha_fade_amount){
			//mr.material.color.a = mr.material.color.a - alpha_fade_amount;
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

	public int SIZE = 1;
	public int FONT_RES = 100;
	public float SCALE = 0.1f;	
	public Color BASE_COLOR = new Color(71f/255f, 255f/255f, 103f/255f);
	
	public GameObject MANAGERS; // object containing Font and Material manager components
	// must be passed in initialization or through editor

	public float MOVE_SPEED = 1f;
	public Vector3 MOVE_DIR = Vector3.down; //should have magnitude 1
	
	public double ALPHA_FADE = 0.01; // how much alpha decreases every frame
	
	public float CHAR_SPACING = 0.7f; // this ought to be dynamic at some point. later
	Vector3 original_pos;

	public bool smooth_terminate = false; // set to true and the bolt will end / kill itself once the existing cells fade

	List<TextCell> cells = new List<TextCell>();
	int cellcounter = 0;
	
	void Start () {
		original_pos = transform.position;
	}

	void AddCell(){
		cells.Add( new TextCell (transform.position, SIZE, FONT_RES, SCALE, BASE_COLOR, MANAGERS) );
		cellcounter ++;
	}

	void FixedUpdate () {
		if (!smooth_terminate) {
			Vector3 deltaPos = transform.position - original_pos;
			if (deltaPos.magnitude > (cellcounter) * CHAR_SPACING) {
				AddCell ();
			}
		}
		Recolor ();
		Move ();
		if (smooth_terminate) {
			if(cells.Count == 0){
				Destroy(gameObject);
			}
		}
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
