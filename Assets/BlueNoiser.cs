using UnityEngine;
using System.Collections;
using System.Collections.Specialized;

public class BlueNoiser {

	const int MAX_TRIES = 64;

	float rad = 1;
	//min and max define opposite corners of space to fill
	public Vector2 minPos = new Vector2(0, 0); 
	public Vector2 maxPos = new Vector2(100, 100);

	int lastUsed = 0;
	OrderedDictionary noise_points; // oh no! my type safety!
	// dict value is how many tries the point has left to find a spot

	Vector2[] np_array;

	public Vector2 GetNoisePoint(){
		Vector2 ret = np_array[lastUsed];
		lastUsed ++;
		if( lastUsed == np_array.Length ){
			FindNoisePoints();
		}
		return ret;
	}

	public void FindNoisePoints(){
		Debug.Log ("Finding new noise points.");
		noise_points = new OrderedDictionary ();
		noise_points.Add ( RandomPoint(), MAX_TRIES );

		while ( !Done () ) {
			foreach ( DictionaryEntry np in noise_points ){
				noise_points[np.Key] = (int)(np.Value) - 1;
				Vector2 trialPt = GetRandomExt( (Vector2)(np.Key) );
				if ( IsNoisePointValid( trialPt ) ){
					noise_points.Add ( trialPt, MAX_TRIES);
				}
			}
		}
		np_array = new Vector2[ noise_points.Count ];
		noise_points.Keys.CopyTo(np_array, 0);

	}

	public bool IsNoisePointValid( Vector2 pt ){
		foreach ( Vector2 np in np_array ){
			Vector2 delta = pt - np;
			if ( delta.sqrMagnitude > rad * rad ){
				return false;
			}
		}
		return true;
	}

	public bool Done(){
		foreach ( DictionaryEntry np in noise_points ){
			if ( (int)(np.Value) > 0 ){
				return false;
			}
		} 
		return true;
	}

	public Vector2 RandomPoint(){
		// white noise
		Vector2 ret = maxPos - minPos;
		ret.x *= Random.value;
		ret.y *= Random.value;
		ret += minPos;
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
