using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public static class TextUtil {

	public static float DEFAULT_PERIOD 		= 0.08f;
	public static int   DEFAULT_MAX_BOLTS 	= 300;

	public static float DEFAULT_X_SPREAD 	= 1000f;
	public static float DEFAULT_Y_SPREAD 	= 0f;
	public static float DEFAULT_Z_SPREAD 	= 40f;

	public static int DEFAULT_FONT_SIZE 	= 10;
	public static float DEFAULT_MOVE_SPEED 	= 40f;
	public static Color DEFAULT_COLOR 		= new Color(71f/255f, 255f/255f, 103f/255f);
	public static double DEFAULT_ALPHA_FADE = 0.005f;

	public static string DEFAULT_FONT_NAME 	= "Pixelate";
	public static string DEFAULT_MATERIAL_NAME  = "Font Material";
	public static string DEFAULT_CHARACTER_POOL = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM?!@#$%^&*()+/={}[]|:;<>";

	public static string UTF_CHARACTER_POOL = "ÿØàJFIHÛC!$À"
		+"XÄµ}1AQaq2¡#B±ÁRÑð3br%&()456789:DEGSTUVWYZcdefghijstuvwxyzƒ"
		+"†‡‰Šš¢£¤¥¦§©ª¶ºÂÃÅÆÇÈÉÊÒÓÔÕÖ×ÙÚáâãäåæçèéêñòóôõö÷øùú"
		+"?<LÎ½O{>Í¼Ý¾Ž_ìžÐý+Ë«~|¬¿ßüKopŒ€0®ûþŸïëmœÞ[líÜ;k]ÌÏM»Pnî=N@"
		+"QWERTYUIOPASDFGHJKLZXCVBNM1234567890@#$%&";

	public static Vector2 GetTextSize( string text, GUIStyle styl ){
		return styl.CalcSize ( new GUIContent("A") ) / 10;
	}

	public static GUIStyle CreateMeasuringStyle(Font f, int size){
		GUIStyle styl 	= new GUIStyle();
		styl.font 		= f;
		styl.fontSize 	= size;
		styl.name     	= "Measuring Style";
		return styl;
	}


	public static Font GetFont(GameObject managers, string NAME){
		FontManager fm  = (FontManager)(managers.GetComponent<FontManager>());
		return (fm).get.GetFont(NAME);
	}
	
	public static Material GetMaterial(GameObject managers, string NAME){
		MaterialManager mm = (MaterialManager)(managers.GetComponent<MaterialManager>());
		return (mm).get.GetMaterial(NAME);
	}
	
	
	public static string GetRandomChar(string char_choices){
		return ""+( char_choices[ Random.Range(0, char_choices.Length-1) ] );
	}

	public static string Isolate(string text){
		HashSet<char> set = new HashSet<char> ();

		foreach(char ch in text){
			set.Add(ch);
		}

		string ret = "";
		
		foreach(char ch in set){
			ret += ch;
		}

		return ret;
	}

	public static string GetTextPool(TextAsset tasset){
		if (tasset == null) {
			return null;
		}
		return Isolate ( tasset.text );
	}

	public static string GenerateTextPool(TextAsset tasset, string filename){
		if (tasset == null) {
			Debug.Log("Text pool gen failed: null asset");
			return null;
		}

		string txt = Isolate ( tasset.text );
		StreamWriter sw = File.CreateText (filename);

		sw.WriteLine (txt);
		sw.Close ();

		Debug.Log ("Text pool generated in file:" + filename);
		return txt;
	}

}
