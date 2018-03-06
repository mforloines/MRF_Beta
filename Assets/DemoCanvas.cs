using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrainVR.UnityFramework.UI.InGame;

public class DemoCanvas : MonoBehaviour {

	ExperimentCanvasManager _canvas;

	// Use this for initialization
	void Start () 
	{
		_canvas = ExperimentCanvasManager.Instance;
		_canvas.Show ();
	}
	
	// Update is called once per frame
	void Update () {
		_canvas.SetText ("Instructions", "Hello");
		_canvas.SetText ("Help", "Helping field");


	}
}
