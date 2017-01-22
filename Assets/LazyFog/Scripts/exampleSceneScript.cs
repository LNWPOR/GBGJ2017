using UnityEngine;
using System.Collections;

public class exampleSceneScript : MonoBehaviour {

	// Use this for initialization
	
	private float scale = 0.6f;
	private float intensity = 0.828f;
	private float alpha = 0.23f;
	private float alphasub = 0.074f;
	private float pow = 0.6f;
	private Color color = new Color(1f, 0.95f, 0.95f, 1f);
	private Material fogMaterial;
	
	void Start () {
		fogMaterial.SetFloat("_Scale", scale);
		fogMaterial.SetFloat("_Intensity", intensity);
		fogMaterial.SetFloat("_Alpha", alpha);
		fogMaterial.SetFloat("_AlphaSub", alphasub);
		fogMaterial.SetFloat("_Pow", pow);
		fogMaterial.SetColor("_Color", color);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	
}
