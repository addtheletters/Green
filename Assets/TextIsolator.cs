using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class TextIsolator : MonoBehaviour {

	public TextAsset source;
	/*string intext = "Ï –þ!FŒ©ìAæ­Çá«’0·)•þùÍ%4Ã•‘¾‡unXK§û(x¤’Þmê°Y7#$O­j’zÜÎRkBÅ‡‡®¤œÝ4:°È2ä(…t:n‰e¦Þ+6‡" +
					"§Ê#BYIÜ<k)´Ý“*z³Àÿ f-JÖŒ^!ñÏî–;K©ä,Ã¦«ÿ ±Í÷“®ø¿Sha–IÄ(¦EÜé]É¯J¿ÃSÑvgÓöž&‘ ¼’" +
	      			"Õ»F¼ùSö±×Ç‹¼r|;áöy4¿Æ­{/™”ûL„Oû¹=÷W_O–·;Ù Œî¬Î;ào…áø‹åÈ-;J{•l¼" +
					"{ò:ú)¢»1uÜ&•úNM³¬†Ûn2 4KsHNrExÚÍèMÈµ¢È žÔõÕu&Pvž0qšµ‡h9„»ñÿ 2#*gŽP}vðBð‰>S×ÑO­"+
			"©¢2eºv$nÍ>	$$rk®PJ&‘‰¥ËGü+NÚÿ Í‡xôàWHu,ë´fó!òÞKv !ÆN?¥v¶‰il©mLrJó*^å­ˆîá…Þr_z…@Ž=¼‘ïI";
	*/
	

	public string Isolate(string text){
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

	string LoadString(TextAsset tasset){
		return tasset.text;
	}

	public string GetTextPool(){
		return Isolate ( LoadString (source) );
	}

}
