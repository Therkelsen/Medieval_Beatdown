#pragma warning disable 0414
#pragma warning disable 0649

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
using System.Collections;

[RequireComponent(typeof(Animator))]
[ExecuteInEditMode]
public class PlatformAnimator2D : MonoBehaviour {

	[Header("-- CREATE ANIMATIONS --")]
	[SerializeField] private bool createAnimations = false;

	[Header("-- Animation Settings --")]
	[SerializeField] private bool changesMade = false;
	[SerializeField] private AnimInfo idle, walk, jump, fall;

	private AnimatorState idleState, walkState, jumpState, fallState;




	// Use this for initialization
	void Start () {
		//OnValidate ();
	}


	void OnValidate() {
		changesMade = true;

		Animator anim = GetComponent<Animator>();
		AnimatorController controller = (AnimatorController) anim.runtimeAnimatorController;

		if (createAnimations) {
			changesMade = false;
			createAnimations = false;
			// Check if the Animator has an AnimatorController attached to it or not
			if (anim.runtimeAnimatorController == null) {
				string nameOfController = gameObject.name + "_Controller.controller";

				// Find all assets with the same name as the gameObject and are of the AnimatorController type
				string[] animControl = AssetDatabase.FindAssets (gameObject.name + " t:AnimatorController");

				bool addedController = false;

				// Find an already existing AnimatorController and add it to the gameObject
				if (animControl.Length > 0) {
					foreach (string s in animControl) {
						string path = AssetDatabase.GUIDToAssetPath (s);
						string[] filename = path.Split ('/');

						if (filename [filename.Length - 1].Equals (gameObject.name + "_Controller.controller")) {
							// The exact AnimatorController has been found...!!!
							AnimatorController newAnim = (AnimatorController) AssetDatabase.LoadAssetAtPath (path, typeof(AnimatorController));
							anim.runtimeAnimatorController = newAnim;
							addedController = true;
						}
					}
				}

				// If no existing AnimatorController was found, create a new and add it to the gameObject
				if (addedController == false) {
					AnimatorController newAnim = AnimatorController.CreateAnimatorControllerAtPath ("Assets/" + nameOfController);
					anim.runtimeAnimatorController = newAnim;
					idle.clip = new AnimationClip ();	AssetDatabase.CreateAsset (idle.clip, "Assets/" + gameObject.name + "_Idle.anim");				
					walk.clip = new AnimationClip ();	walk.clip.wrapMode = WrapMode.Loop;		AssetDatabase.CreateAsset (walk.clip, "Assets/" + gameObject.name + "_Walk.anim");
					jump.clip = new AnimationClip ();	jump.clip.wrapMode = WrapMode.Once;		AssetDatabase.CreateAsset (jump.clip, "Assets/" + gameObject.name + "_Jump.anim");
					fall.clip = new AnimationClip ();	fall.clip.wrapMode = WrapMode.Loop;		AssetDatabase.CreateAsset (fall.clip, "Assets/" + gameObject.name + "_Fall.anim");
					jump.animationLoop = false;
				}
			}

			controller = (AnimatorController) anim.runtimeAnimatorController;

			// Removes all old parameters, so they can be added on new
			AnimatorControllerParameter[] controlParams = controller.parameters;
			foreach(AnimatorControllerParameter acp in controlParams) {
				controller.RemoveParameter (acp);
			}

			// Adding the needed parameters
			controller.AddParameter ("IsWalking", AnimatorControllerParameterType.Bool);
			controller.AddParameter ("Grounded", AnimatorControllerParameterType.Bool);
			controller.AddParameter ("Jump", AnimatorControllerParameterType.Trigger);


			// Removes all old states, so they can be added on new
			AnimatorStateMachine rootState = controller.layers [0].stateMachine;
			ChildAnimatorState[] states = rootState.states;
			foreach(ChildAnimatorState state in states) {
				rootState.RemoveState (state.state);
			}

			// Adding the needed states
			idleState = rootState.AddState ("Idle", new Vector3 (270.0f, 20.0f, 0.0f));		idleState.speed = idle.clipSpeed;
			walkState = rootState.AddState ("Walk", new Vector3 (270.0f, 120.0f, 0.0f));	walkState.speed = walk.clipSpeed;
			jumpState = rootState.AddState ("Jump", new Vector3 (520.0f, 20.0f, 0.0f));		jumpState.speed = jump.clipSpeed;
			fallState = rootState.AddState ("Fall", new Vector3 (520.0f, 120.0f, 0.0f));	fallState.speed = fall.clipSpeed;

			// Adding the needed transitions
			AnimatorStateTransition transition = null;

			// From IDLE transitions
			// To WALK
			transition = idleState.AddTransition (walkState);
			transition.hasExitTime = false;
			transition.AddCondition (AnimatorConditionMode.If, 1, "IsWalking");
			// To JUMP
			transition = idleState.AddTransition (jumpState);
			transition.hasExitTime = false;
			transition.AddCondition (AnimatorConditionMode.If, 1, "Jump");
			// To FALL
			transition = idleState.AddTransition (fallState);
			transition.hasExitTime = false;
			transition.AddCondition (AnimatorConditionMode.IfNot, 1, "Grounded");

			// From WALK transitions
			// To IDLE
			transition = walkState.AddTransition (idleState);
			transition.hasExitTime = false;
			transition.AddCondition (AnimatorConditionMode.IfNot, 1, "IsWalking");
			// To JUMP
			transition = walkState.AddTransition (jumpState);
			transition.hasExitTime = false;
			transition.AddCondition (AnimatorConditionMode.If, 1, "Jump");
			// To FALL
			transition = walkState.AddTransition (fallState);
			transition.hasExitTime = false;
			transition.AddCondition (AnimatorConditionMode.IfNot, 1, "Grounded");

			// From JUMP transitions
			// To FALL
			transition = jumpState.AddTransition (fallState);
			transition.hasExitTime = true;

			// From FALL transitions
			// To IDLE
			transition = fallState.AddTransition (idleState);
			transition.hasExitTime = false;
			transition.AddCondition (AnimatorConditionMode.IfNot, 1, "IsWalking");
			transition.AddCondition (AnimatorConditionMode.If, 1, "Grounded");
			// To WALK
			transition = fallState.AddTransition (walkState);
			transition.hasExitTime = false;
			transition.AddCondition (AnimatorConditionMode.If, 1, "IsWalking");
			transition.AddCondition (AnimatorConditionMode.If, 1, "Grounded");


			// Assigns the AnimationClips to the appropriate states
			idleState.motion = idle.clip;
			walkState.motion = walk.clip;
			jumpState.motion = jump.clip;
			fallState.motion = fall.clip;


			// Assigns the Animation Loop (Loop Time) bool to the AnimationClips
			// Making some of the AnimationClips looping their animation
			AnimationClipSettings clipSettings = new AnimationClipSettings();
			clipSettings.loopTime = idle.animationLoop;
			AnimationUtility.SetAnimationClipSettings (idle.clip, clipSettings);

			clipSettings.loopTime = walk.animationLoop;
			AnimationUtility.SetAnimationClipSettings (walk.clip, clipSettings);

			clipSettings.loopTime = jump.animationLoop;
			AnimationUtility.SetAnimationClipSettings (jump.clip, clipSettings);

			clipSettings.loopTime = fall.animationLoop;
			AnimationUtility.SetAnimationClipSettings (fall.clip, clipSettings);
		}


	}

}

#endif

[System.Serializable]
public class AnimInfo {
	public AnimationClip clip;
	public float clipSpeed = 1.0f;
	public bool animationLoop = true;
}


/*
[CustomPropertyDrawer(typeof(AnimInfo))]
public class AnimInfoDrawer : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty (position, label, property);

		position = EditorGUI.PrefixLabel (position, GUIUtility.GetControlID (FocusType.Passive), label);

		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		Rect speedLabelRect = new Rect (position.x - 40, position.y, 40, position.height);
		Rect clipSpeedRect = new Rect (position.x + 5, position.y, 40, position.height);
		Rect clipLabelRect = new Rect (position.x + 60, position.y, 30, position.height);
		Rect clipRect = new Rect (position.x + 90, position.y, position.width - 90, position.height);

		EditorGUI.LabelField (speedLabelRect, "Speed");
		EditorGUI.PropertyField (clipRect, property.FindPropertyRelative ("clip"), GUIContent.none);
		EditorGUI.LabelField (clipLabelRect, "Clip");
		EditorGUI.PropertyField (clipSpeedRect, property.FindPropertyRelative ("clipSpeed"), GUIContent.none);


		EditorGUI.indentLevel = indent;

		EditorGUI.EndProperty ();
	}

}
*/