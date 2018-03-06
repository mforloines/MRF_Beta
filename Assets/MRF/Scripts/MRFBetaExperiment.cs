using System;
using System.Collections.Generic;
using BrainVR.UnityFramework.Scripts.Objects.Beeper;
using UnityEngine;
using BrainVR.UnityFramework.Experiments.Helpers;
using BrainVR.UnityFramework.Objects.Goals;
using BrainVR.UnityFramework.Scripts.Objects.Goals;
using BrainVR.UnityFramework.Player;
using BrainVR.UnityFramework.InputControl;
using BrainVR.UnityFramework.Menu;

namespace BrainVR.UnityFramework.Scripts.Experiments.MRFBetaExperiment
{
	public enum TrialType {Training, Pointing};
	public class MRFBetaExperiment : Experiment
	{
		private GoalManager goalManager;
		private StoresManager storesManager;
		private PlayerController _player;
		protected float TrialStartTime;
		protected float TrialEndTime;
		protected List<float> TrialTimes = new List<float>();
		public new MRFBetaExperimentSettings Settings;

		private TrialType _currentTrialType;
		private int _trialNumberWithinBlock;
		private int _totalNumberOfTrials;

		private Vector2 CENTER = new Vector2(0, -34);

		#region Forced API
		public override void AddSettings(ExperimentSettings settings)
		{
			Settings = (MRFBetaExperimentSettings)settings;
		}
		#endregion
		#region Experiment Logic
		protected override void OnExperimentInitialise() 
		{
			_currentTrialType = TrialType.Training;
			_totalNumberOfTrials = Settings.NumberOfBlocks * Settings.GoalOrder.Length;
		}
		protected override void AfterExperimentInitialise() { }
		protected override void ExperimentUpdate() { }
		protected override void OnExperimentSetup()
		{
			CanvasManager.Show();
			goalManager = GoalManager.Instance;
			storesManager = StoresManager.Instance;
			_player = PlayerController.Instance;
			//setup store types
			for(var i = 0; i < Settings.StoreTypes.Length; i++){
				storesManager.Objects [i].SetType (Settings.StoreTypes [i]);
			}
			goalManager.HideAll ();
		}
		protected override void AfterExperimentSetup() { }
		protected override void OnExperimentStart() { }
		protected override void AfterExperimentStart() { 
			// Instructions screen needs to be loaded
			CanvasManager.Show();
		}
		protected override void OnTrialSetup() {
			if (_trialNumberWithinBlock >= Settings.NumberOfTrainingGoals) {
				_currentTrialType = TrialType.Pointing;
			}
			if (_currentTrialType == TrialType.Training) {
				SetupTraining ();
			} else {
				SetupPointing ();
			}
		}
		protected override void AfterTrialSetup()
		{
			TrialStart ();
		}
		protected override void OnTrialStart()
		{
			TrialStartTime = Time.realtimeSinceStartup;
			if (_currentTrialType == TrialType.Training) {
				StartTraining ();
			} else {
				StartPointing ();
			}
		}
		protected override void AfterTrialStart(){}
		protected override void OnTrialFinished()
		{
			TrialEndTime = Time.realtimeSinceStartup;
			var trialTime = TrialEndTime - TrialStartTime;
			TrialTimes.Add(trialTime);
			CanvasManager.SetText ("Message", trialTime.ToString ());
			if (_currentTrialType == TrialType.Training) {
				FinishTraining ();
			} else {
				FinishPointing ();
			}
		}
		protected override void AfterTrialFinished()
		{
			TrialClose ();
			TrialSetNext ();
		}
		protected override void OnTrialClosed() { 
			_trialNumberWithinBlock += 1;
		}
		protected override void AfterExperimentClosed() { }
		protected override void OnExperimentFinished() { }
		protected override void AfterExperimentFinished() { 
			// need to load next scene, and repeat the whole process 3 times
		}
		protected override void OnExperimentClosed()
		{
			CanvasManager.Show(false);
			Application.LoadLevel ("MainMenu");
		}
		public override string ExperimentHeaderLog()
		{
			return "";
		}
		protected override void AfterTrialClosed()
		{
			if (_trialNumberWithinBlock >= Settings.GoalOrder.Length) {
				StartNewBlock ();
			}
		}
		protected override bool CheckForEnd()
		{
			return TrialNumber >= _totalNumberOfTrials - 1;
		}
		#endregion
		#region Experiment flow functions
		void GoalEntered(GoalController goal, EventArgs e){
			TrialFinish ();
		}

		void StartNewBlock(){
			_trialNumberWithinBlock = 0;
			_currentTrialType = TrialType.Training;
		}
		#endregion
		#region Test functions
		private void SetupTraining(){
			CanvasManager.SetText("Trial number", TrialNumber.ToString());
			var instructionText = Settings.Messages ["goto"] + currentStoreName;
			CanvasManager.SetText("Instructions", instructionText);
		}

		private void SetupPointing(){
			_player.MoveToPosition (CENTER);
			_player.EnableMovement (false);
			storesManager.HideAll ();
			CanvasManager.SetText("Trial number", TrialNumber.ToString());
			var instructionText = Settings.Messages ["point"] + currentStoreName;
			CanvasManager.SetText("Instructions", instructionText);
			InputManager.PointButtonPressed += TrialFinish;
		}
		private void StartTraining(){
			currentGoal.OnEnter += GoalEntered;
		}

		private void StartPointing(){

		}
		private void FinishTraining(){
			currentGoal.OnEnter -= GoalEntered;
		}
		private void FinishPointing(){
			InputManager.PointButtonPressed -= TrialFinish;
			_player.EnableMovement ();
			storesManager.ShowAll ();
		}

		#endregion
		#region Helpers
		private string currentStoreName{get{return Settings.StoreTypes[currentGoalIndex];}}
		private int currentGoalIndex {get{return Settings.GoalOrder[_trialNumberWithinBlock];}}
		private GoalController currentGoal {get{return goalManager.Goals[currentGoalIndex];}}
		#endregion
	}
}
