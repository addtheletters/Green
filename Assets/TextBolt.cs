using UnityEngine;
using System.Collections;

public class TextBolt : MonoBehaviour {


	class TextCell {

		public GameObject text_mesh;
		public MeshRenderer mr;

		public TextCell( int size, int font_res, float scale, Color color, GameObject managers, 
		                string fontname = "Pixelate", string materialname = "Font Material"){
			text_mesh = new GameObject();

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
		}

		public string getText(){
			//a single random character
			return "UNIMPLEMENTED";
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
	public float MOVE_SPEED = 0.01f;
	public float SCALE = 0.1f;
	public Vector3 MOVE_DIR = Vector3.down; //should have magnitude 1
	
	public double ALPHA_FADE = 0.001; // how much alpha decreases every frame
	public Color BASE_COLOR = new Color(1, 1, 1);

	ArrayList cells = new ArrayList();
	public GameObject MANAGERS;


	//initialization
	void Start () {
		//FONT = fm.get.GetFont("Pixelate");
		AddCell ();
	}

	void AddCell(){
		cells.Add( new TextCell (SIZE, FONT_RES, SCALE, BASE_COLOR, MANAGERS) );
	}

	//called once per frame
	void Update () {
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
		// Hah! I used deltaTime like a serious programmer dude!
		transform.Translate(MOVE_DIR * MOVE_SPEED * Time.deltaTime);
	}

}
