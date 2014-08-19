using UnityEngine;
using System.Collections;

public class MaterialManager : MonoBehaviour {
	public Material[] materials;
	
	private MaterialManager instance;
	public MaterialManager get {
		get {
			return instance;
		}
	}
	
	public void Awake() {
		instance = this;
	}
	
	public Material GetMaterial(string materialName) {
		foreach (Material material in materials)
			if (material.name == materialName)
				return material;
		
		return null;
	}
}