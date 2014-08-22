using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

public class BlueNoiser {

	int max_tries;
	float rad;
	//min and max define opposite corners of space to fill
	Vector2 min_pos; 
	Vector2 max_pos;

	int last_used;
	Dictionary<Vector2, int> noise_points; // oh no! my type safety!
	// dict value is how many tries the point has left to find a spot

	Vector2[] np_array;

	public BlueNoiser(): this(1, 30, new Vector2(0, 0), new Vector2(100, 100)){
	}

	public BlueNoiser(float rad, int triesPerPt, Vector2 minPos, Vector2 maxPos){
		this.rad = rad;
		this.max_tries = triesPerPt;
		this.min_pos = minPos;
		this.max_pos = maxPos;
		FindNoisePoints ();
	}

	public Vector2 GetNoisePoint(){
		Debug.Log ("len :" + np_array.Length);
		Debug.Log ("last:" + last_used);
		Vector2 ret = np_array[last_used];
		last_used ++;
		if( last_used == np_array.Length ){
			FindNoisePoints();
		}
		return ret;
	}

	public void FindNoisePoints(){
		Debug.Log ("Finding new noise points.");
		noise_points = new Dictionary<Vector2, int> ();
		noise_points.Add ( RandomPoint(), max_tries );

		while ( !Done () ) {
			List<Vector2> to_add = new List<Vector2>();
			List<Vector2> np_keys = new List<Vector2>(noise_points.Keys);

			foreach ( Vector2 np in np_keys ){
				if( noise_points[np] <= 0 ) continue;
				noise_points[np] = noise_points[np] - 1;
				Vector2 trialPt = GetRandomExt( np );
				if ( IsNoisePointValid( trialPt ) ){
					to_add.Add ( trialPt );
					Debug.Log("Tried" + trialPt + " and succeeded");
				}
				else{
					Debug.Log("Tried" + trialPt + " and failed");
				}
			}

			foreach ( Vector2 np in to_add ){
				noise_points.Add( np, max_tries );
			}
		}
		np_array = new Vector2[ noise_points.Count ];
		noise_points.Keys.CopyTo(np_array, 0);

		Debug.Log ("Noise points generated with count " + np_array.Length);

	}

	public bool IsNoisePointValid( Vector2 pt ){
		if (!IsPointInMinMax (pt)) {
			Debug.Log ("pt " + pt + "failed minmax bounds test");
			return false;
		}
		foreach ( Vector2 np in noise_points.Keys ){
			Vector2 delta = pt - np;
			if ( delta.sqrMagnitude < rad * rad ){
				Debug.Log("pt " + pt + "failed radial test");
				return false;
			}
		}
		return true;
	}

	public bool IsPointInMinMax(Vector2 pt){
		Vector2 diff = max_pos - min_pos;
		Vector2 ptdiff = max_pos - pt;
		return ptdiff.x < diff.x && ptdiff.y < diff.y;
	}

	public bool Done(){
		foreach ( Vector2 np in noise_points.Keys ){
			if ( noise_points[np] > 0 ){
				return false;
			}
		} 
		return true;
	}

	public Vector2 RandomPoint(){
		// white noise
		Vector2 ret = max_pos - min_pos;
		ret.x *= Random.value;
		ret.y *= Random.value;
		ret += min_pos;
		return ret;
	}

	public Vector2 GetRandomExt(Vector2 pt){
		return GetPointAt (pt, GetRandomAngle(), GetRandomRad());
	}

	public float GetRandomRad(){
		// number between rad and 2*rad
		return rad * Random.value + rad;
	}

	public float GetRandomAngle(){
		// random number between 0 and 2*pi
		return Random.value * 2 * Mathf.PI;
	}

	public Vector2 GetPointAt(Vector2 origin, float angle, float radius){
		Vector2 ret = origin;
		ret.x += radius * Mathf.Cos (angle);
		ret.y += radius * Mathf.Sin (angle);
		return ret;
	}

}
