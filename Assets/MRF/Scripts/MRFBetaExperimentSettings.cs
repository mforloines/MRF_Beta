using BrainVR.UnityFramework.Experiments.Helpers;
using BrainVR.UnityFramework.Helpers;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

namespace BrainVR.UnityFramework.Scripts.Experiments.MRFBetaExperiment
{
	public class MRFBetaExperimentSettings : ExperimentSettings
	{
		public int[] GoalOrder = {0, 1, 2, 3, 0, 1, 2, 3};
		public int NumberOfTrainingGoals = 6;
		public int NumberOfBlocks = 4;

		public string[] StoreTypes = { "Dentist", "Gym", "Pizzeria", "Florist", "Ice Cream", "Cell Phones" };

		public Dictionary<string, string> Messages;

		public MRFBetaExperimentSettings(){
			Messages = new Dictionary<string, string>{
				{"goto", "Walk towards "},
				{"point", "Point towards "}
			};
		}
		#if UNITY_EDITOR
		[MenuItem("Assets/MRF_New/ExperimentSettings")]
		public static void CreateDialogueLine()
		{
			ScriptableObjectUtility.CreateAsset<MRFBetaExperimentSettings>();
		}
		[CustomEditor(typeof(MRFBetaExperimentSettings))]
		public class SettingsEditor : Editor
		{
			public override void OnInspectorGUI()
			{
				DrawDefaultInspector();
				MRFBetaExperimentSettings myScript = (MRFBetaExperimentSettings)target;
				if (GUILayout.Button("Serialise settings")) Debug.Log(myScript.SerialiseOut());
			}
		}
		#endif
	}
}
