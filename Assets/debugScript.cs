using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrainVR.UnityFramework.UI.InGame;

public class debugScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//ShowcaseCanvas ();
	}

	void ShowcaseCanvas(){
		var canvas = ExperimentCanvasManager.Instance;
		canvas.Show ();
		canvas.SetText ("Help", "This is helping field");
		canvas.SetText ("Instructions", "This is instructions filed");
		canvas.SetText ("Trial number", "This is trial number");
		canvas.SetText ("Message", "This is message field");
	}
}
