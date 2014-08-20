using UnityEngine;
using System.Collections;

public class TextBolt : MonoBehaviour {
	
	class TextCell {

		public GameObject text_mesh;
		public MeshRenderer mr;

		public TextCell( Vector3 pos, int size, int font_res, float scale, Color color, GameObject managers, 
		                string fontname = "Pixelate", string materialname = "Font Material"){
			text_mesh = new GameObject("text cell");
			text_mesh.transform.position = pos;
			TextMesh mesh_component = (TextMesh)(text_mesh.AddComponent("TextMesh"));

			mesh_component.fontSize 	 = font_res;
			mesh_component.characterSize = size;
			mesh_component.color 		 = color;
			mesh_component.text 		 = getText();

			FontManager fm = (FontManager)(managers.GetComponent("FontManager"));
			mesh_component.font 		 = (fm).get.GetFont(fontname);


			mr = text_mesh.transform.GetComponent<MeshRenderer>();
			MaterialManager mm = (MaterialManager)(managers.GetComponent("MaterialManager"));
			mr.material = (mm).get.GetMaterial(materialname);

			/*if (mr.material == null){
				print("NONONONOull");
			}*/

			text_mesh.transform.localScale = text_mesh.transform.localScale * scale;
			Vector3 rot = text_mesh.transform.eulerAngles;
			rot.z -= 90;
			text_mesh.transform.eulerAngles = rot;
		}

		public string getText(string charChoices = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM?!@#$%^&*()_+-={}[]|:;<>"){
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
	
	//public Component fm;

	public int SIZE = 5;
	public int FONT_RES = 10;
	public Font FONT;
	public float MOVE_SPEED = 1f;
	public float SCALE = 0.1f;
	public Vector3 MOVE_DIR = Vector3.down; //should have magnitude 1
	
	public double ALPHA_FADE = 0.001; // how much alpha decreases every frame
	public Color BASE_COLOR = new Color(1, 1, 1);

	float CHAR_SPACING = 1f;
	Vector3 original_pos;

	ArrayList cells = new ArrayList();
	public GameObject MANAGERS;


	//initialization
	void Start () {
		//FONT = fm.get.GetFont("Pixelate");
		original_pos = transform.position;
		//AddCell ();
	}

	void AddCell(){
		cells.Add( new TextCell (transform.position, SIZE, FONT_RES, SCALE, BASE_COLOR, MANAGERS) );
	}

	//called once per frame
	void Update () {
		Vector3 deltaPos = transform.position - original_pos;
		if (deltaPos.magnitude > cells.Count * CHAR_SPACING){

			print ("mag:"+deltaPos.magnitude);
			print ("space:"+cells.Count * CHAR_SPACING);

			AddCell();
		}
		Recolor ();
		Move ();
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
		// Hah! I used fixedDeltaTime like a serious programmer dude!
		transform.Translate(MOVE_DIR * MOVE_SPEED * Time.fixedDeltaTime);
	}

}
