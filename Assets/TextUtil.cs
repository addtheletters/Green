﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public static class TextUtil {

	/*string intext = "Ï –þ!FŒ©ìAæ­Çá«’0·)•þùÍ%4Ã•‘¾‡unXK§û(x¤’Þmê°Y7#$O­j’zÜÎRkBÅ‡‡®¤œÝ4:°È2ä(…t:n‰e¦Þ+6‡" +
					"§Ê#BYIÜ<k)´Ý“*z³Àÿ f-JÖŒ^!ñÏî–;K©ä,Ã¦«ÿ ±Í÷“®ø¿Sha–IÄ(¦EÜé]É¯J¿ÃSÑvgÓöž&‘ ¼’" +
	      			"Õ»F¼ùSö±×Ç‹¼r|;áöy4¿Æ­{/™”ûL„Oû¹=÷W_O–·;Ù Œî¬Î;ào…áø‹åÈ-;J{•l¼" +
					"{ò:ú)¢»1uÜ&•úNM³¬†Ûn2 4KsHNrExÚÍèMÈµ¢È žÔõÕu&Pvž0qšµ‡h9„»ñÿ 2#*gŽP}vðBð‰>S×ÑO­"+
			"©¢2eºv$nÍ>	$$rk®PJ&‘‰¥ËGü+NÚÿ Í‡xôàWHu,ë´fó!òÞKv !ÆN?¥v¶‰il©mLrJó*^å­ˆîá…Þr_z…@Ž=¼‘ïI";
	*/

	public static string DEFAULT_FONT_NAME = "Pixelate";
	public static string DEFAULT_MATERIAL_NAME = "Font Material";
	public static string DEFAULT_CHARACTER_POOL = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM?!@#$%^&*()+/={}[]|:;<>";

	public static string UTF_CHARACTER_POOL = "ÿØàJFIHÛC!$À"
		+"XÄµ}1AQaq2¡#B±ÁRÑð3br%&()456789:DEGSTUVWYZcdefghijstuvwxyzƒ"
		+"†‡‰Šš¢£¤¥¦§©ª¶ºÂÃÅÆÇÈÉÊÒÓÔÕÖ×ÙÚáâãäåæçèéêñòóôõö÷øùú"
		+"?<LÎ½O{>Í¼Ý¾Ž_ìžÐý+Ë«~|¬¿ßüKopŒ€0®ûþŸïëmœÞ[líÜ;k]ÌÏM»Pnî=N@"
		+"QWERTYUIOPASDFGHJKLZXCVBNM1234567890@#$%&";

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